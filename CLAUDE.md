# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

`eshop-microservices` is a .NET 8 e-commerce reference application built from independently deployable microservices behind a YARP API gateway, with a Razor Pages storefront. It demonstrates **service-level CQRS** (Catalog is split into separate read and write services kept in sync by events), event-driven integration over RabbitMQ, polyglot persistence (a different store per service, on purpose), and JWT-based auth issued by a dedicated Customer service. Everything is orchestrated with Docker Compose; Kubernetes manifests live in `deployments/`.

The solution lives under `src/` — that directory is the working root for builds and `docker compose` (the compose build context is `.`).

## Commands

All paths assume you are in `src/`.

```powershell
# Build the whole solution
dotnet build eshop-microservices.sln

# Run the entire system (builds images on first run)
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
docker compose -f docker-compose.yml -f docker-compose.override.yml down

# Run a single service against locally-running infra
dotnet run --project Services/Catalog/CatalogRead/Catalog.Read.API
```

**Tests:** there are no automated test projects. APIs are exercised manually via `src/EShopMicroservices.postman_collection.json` and the per-service `/health` endpoints.

### EF Core migrations

Services using EF Core: **Catalog.Write** (SQL Server), **Ordering** (SQL Server), **Customer** (SQL Server, via ASP.NET Identity), **Discount** (SQLite). **Catalog.Read**, **Basket**, and **Inventory** use Marten and create their schema at runtime — no migrations.

```powershell
# Catalog.Write & Ordering — migrations live in the Infrastructure project, API is the startup project
dotnet ef migrations add <Name> --project Services/Catalog/CatalogWrite/Catalog.Write.Infrastructure --startup-project Services/Catalog/CatalogWrite/Catalog.Write.API
dotnet ef migrations add <Name> --project Services/Ordering/Ordering.Infrastructure --startup-project Services/Ordering/Ordering.API

# Discount (SQLite) — single project
dotnet ef migrations add <Name> --project Services/Discount/Discount.Grpc
```

Migrations are applied automatically on startup in Development: Catalog.Write and Ordering via `InitialiseDatabaseAsync()`, Customer via `ApplyMigrations()` + `SeedData()` (seeds roles and an admin user), Discount via `app.UseMigration()`.

### Kubernetes

Manifests are in the top-level `deployments/` folder (not under `src/`).

```bat
cd deployments
deploy_services.bat   :: creates the `eshop` namespace, applies DBs then APIs (with waits)
clear_services.bat    :: tears the namespace down
```

`ingress.yaml` exposes the storefront via nginx ingress at host `shopping-web.local`.

## Architecture

### Services

| Service | Responsibility | API style | Data store | Auth |
|---|---|---|---|---|
| `Catalog.Read.API` | Product **reads** (queries) | Carter + CQRS | Marten / PostgreSQL | JWT validate |
| `Catalog.Write.API` | Product **writes** (DDD) | Carter + CQRS + DDD | EF Core / SQL Server | JWT validate |
| `Basket.API` | Shopping cart & checkout | Carter + CQRS | Marten / PostgreSQL + Redis | JWT validate |
| `Inventory.API` | Stock levels | Carter + CQRS | Marten / PostgreSQL | JWT validate |
| `Customer.API` | Identity, login, JWT issuance | Carter + CQRS + ASP.NET Identity | EF Core / SQL Server | **issuer** |
| `Discount.Grpc` | Discount coupons | gRPC | EF Core / SQLite | JWT validate |
| `Ordering.API` | Order placement & fulfillment | Carter + CQRS + DDD | EF Core / SQL Server | JWT validate |
| `YarpApiGateway` | Single entry point, routing, rate limiting, JWT validate | YARP | — | JWT validate |
| `Shopping.Web` | Storefront | Razor Pages + Refit | — | cookie + JWT |

### Catalog read/write split (the core CQRS pattern)

Catalog is two services, not one:

- **Catalog.Write.API** is a full DDD stack (`Catalog.Write.Domain` / `.Application` / `.Infrastructure` / `.API`). It owns the SQL Server write model. Command handlers mutate the `Product` aggregate, which raises **domain events**; the `DispatchDomainEventsInterceptor` publishes them through MediatR on `SaveChanges`. Domain event handlers in `Catalog.Write.Application/EventHandlers/Domain` then translate them into **integration events** (`ProductCreated/Updated/Deleted/Reviewed IntegrationEvent`) and publish to RabbitMQ.
- **Catalog.Read.API** owns a denormalized Marten/PostgreSQL read model. Its consumers in `EventHandlers/` (`*IntegrationEventHandler_CatalogReadService`) subscribe to those product integration events and upsert the read store. It only serves GET queries.
- **The gateway routes `/catalog-service` by HTTP method**: `GET` → read cluster, `POST/PUT/DELETE` → write cluster (see `YarpApiGateway/appsettings.json`). Clients see one catalog API; the split is invisible to them.

When changing the product shape, update it in **three places**: the write domain model + EF config, the integration event contract in `BuildingBlocks.Messaging`, and the read model + its consumer.

### Event-driven integration (RabbitMQ via MassTransit)

Integration event contracts are shared in `BuildingBlocks/BuildingBlocks.Messaging/Events`, grouped by domain (`ProductEvents`, `CustomerEvents`, `InventoryEvents`, `OrderFullfilment`, `BasketCheckout`). Broker setup is centralized in `BuildingBlocks.Messaging/MassTransit/Extentions.cs` (`AddMessageBroker(configuration, assembly)` — pass the assembly so its `IConsumer`s are registered; kebab-case endpoint names).

Key flows:
- **Catalog.Write → Catalog.Read & Inventory & Ordering:** product lifecycle events fan out to the read model, inventory (seed new stock / remove), and ordering's local product cache.
- **Inventory → Catalog.Write:** `ProductQuantityUpdatedIntegrationEvent` keeps the write model's stock in sync.
- **Basket → Ordering:** checkout publishes `BasketCheckoutIntegrationEvent`; Ordering creates the order.
- **Ordering → Inventory:** `OrderCreatedIntegrationEvent` decrements stock (gated by the `UpdateInventory` feature flag).
- **Customer → Catalog.Write & Ordering:** `UserRegisteredIntegrationEvent` propagates new customers.

Cross-cutting behavior is gated with Feature Management flags set in `docker-compose.override.yml` (`FeatureManagement__OrderFullfilment`, `FeatureManagement__UpdateInventory`).

### Authentication & authorization

- **Customer.API is the token issuer.** It uses ASP.NET Identity over SQL Server and exposes identity slices under `Identity/` (Register, Login, RefreshToken, ChangePassword[ForUser], AssignRoleToUser, ImpersonateUser, DeleteUser, GetAllUsers). Roles are defined by `BuildingBlocks.Authorization.RoleEnum` (`Admin`, `User`, `Manager`).
- **Every API and the gateway validate JWTs** via the shared `BuildingBlocks.Authorization.JwtExtensions.AddJwtAuthentication(configuration)`. The signing key, issuer, and audience come from `JwtSettings` and **must match across all services** — they are injected identically as env vars in `docker-compose.override.yml`.
- **Shopping.Web** uses cookie authentication holding the JWT. `AuthTokenPassingHandler` (a Refit `DelegatingHandler`) attaches the bearer token to every gateway call; `AuthService.ValidateAndRefreshToken` (wired via `OnValidatePrincipal`) silently refreshes expired access tokens using the refresh token.

### Synchronous calls (gRPC)

Basket calls Discount over gRPC. The contract is `Services/Discount/Discount.Grpc/Protos/discount.proto`; Basket consumes the generated `DiscountProtoService.DiscountProtoServiceClient` (TLS validation bypassed for the self-signed dev cert).

### Vertical-slice + CQRS conventions

The Carter-based services (Catalog.Read, Basket, Inventory, Customer, Ordering.API) use **feature folders**. An operation's endpoint (`ICarterModule`) and its MediatR handler/command/query/validator live together. Note the layout differs slightly by service — Basket and Inventory use a `CQRS/{Commands,Queries}/<Feature>/` + separate `Endpoints/` layout; Customer uses `Identity/<Feature>/`; Catalog.Read uses `Products/<Feature>/`. Copy an existing sibling slice when adding a feature.

Shared CQRS plumbing in `BuildingBlocks/BuildingBlocks`:
- `CQRS/` — `ICommand`, `IQuery`, `ICommandHandler`, `IQueryHandler` (thin wrappers over MediatR).
- `Behaviors/` — `ValidationBehavior` (FluentValidation) and `LoggingBehavior`, registered as MediatR open behaviors in each service's `Program.cs`.
- `DDD_Abstractions/` — `Aggregate`, `Entity`, `ValueObject`, `IDomainEvent` base types, **shared** by the Ordering and Catalog.Write domain layers (no longer duplicated per service).
- `Exceptions/Handler/CustomExceptionHandler` — global `IExceptionHandler`; wired with `app.UseExceptionHandler(...)`.
- `Authorization/` — JWT setup, `ClaimsPrincipal` extensions, `RoleEnum`.

### Basket caching

`CachedBasketRepository` decorates `BasketRepository` via Scrutor's `Services.Decorate<IBasketRepository, CachedBasketRepository>()` — cache-aside over Redis. Keep caching logic in the decorator, not in `BasketRepository`.

## Ports & infrastructure (docker-compose)

External service ports (HTTP, +`60xx` for HTTPS): Catalog.Read `6000`, Basket `6001`, Discount `6002`, Ordering `6003`, **Gateway `6004`**, Web `6005`, Inventory `6006`, Customer `6007`, Catalog.Write `6008`.

Infra: catalog_read_db Postgres `5432`, basketdb Postgres `5433`, inventorydb Postgres `5434`, catalog_write_db SQL Server `1434`, orderdb SQL Server `1435`, customerdb SQL Server `1436`, Redis `6379`, RabbitMQ `5672` / management UI `15672`.

Gateway public routes: `/catalog-service` (GET→read, write→write), `/basket-service`, `/inventory-service`, `/ordering-service` (rate-limited), `/customer-service` (rate-limited). The `fixed` rate-limiter policy is 5 requests / 10s.

All connection strings, broker creds, `JwtSettings`, and feature flags are set as environment variables in `docker-compose.override.yml` — not in `appsettings.json`.

## Conventions

- `GlobalUsing.cs` per project holds shared `using`s; new files rely on those implicit usings.
- Request/command/response/validator for a slice live alongside the handler.
- Strongly-typed IDs in the Ordering domain wrap `Guid` — construct them via their factory rather than passing raw `Guid`s across the domain boundary.
- A product/order shape change is rarely local: trace it through the owning domain model, the integration event contract, and every consumer before considering it done.
