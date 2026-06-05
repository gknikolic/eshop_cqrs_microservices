# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

`eshop-microservices` is a .NET 8 e-commerce reference application built from independently deployable microservices behind a YARP API gateway, with a Razor Pages storefront. Each service owns its own database and the chosen persistence/communication tech differs per service on purpose (polyglot persistence). Everything is orchestrated with Docker Compose; Kubernetes manifests are also provided.

The solution lives under `src/` — the `src` directory is the working root for builds and `docker compose` (the compose build context is `.`).

## Commands

All paths below assume you are in `src/`.

```powershell
# Build the whole solution
dotnet build eshop-microservices.sln

# Run the entire system (builds images on first run)
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
docker compose -f docker-compose.yml -f docker-compose.override.yml down

# Run a single service against locally-running infra
dotnet run --project Services/Catalog/Catalog.API
```

**Tests:** there are no automated test projects. API behavior is exercised manually via `src/EShopMicroservices.postman_collection.json` (import into Postman) and the per-service health endpoints at `/health`.

### EF Core migrations

Only **Ordering** (SQL Server) and **Discount** (SQLite) use EF Core. Catalog and Basket use Marten, which creates its schema automatically at runtime.

```powershell
# Ordering — migrations live in Ordering.Infrastructure but the API is the startup project
dotnet ef migrations add <Name> --project Services/Ordering/Ordering.Infrastructure --startup-project Services/Ordering/Ordering.API

# Discount
dotnet ef migrations add <Name> --project Services/Discount/Discount.Grpc
```

Migrations are applied automatically on startup: Ordering seeds + migrates via `InitialiseDatabaseAsync()` (Development only), Discount via `app.UseMigration()`.

### Kubernetes

`Kubernetes/Kubernetes/kubernetes-compose.bat` applies every `*-deployment.yaml` to the `eshop-microservices` namespace via `kubectl`.

## Architecture

### Services and their stacks

| Service | API style | Persistence | Notes |
|---|---|---|---|
| `Catalog.API` | Minimal API (Carter) + CQRS | Marten / PostgreSQL (document DB) | Auto-seeds via `CatalogInitialData` in Dev |
| `Basket.API` | Minimal API (Carter) + CQRS | Marten / PostgreSQL + Redis cache | gRPC client to Discount; publishes checkout events |
| `Discount.Grpc` | gRPC | EF Core / SQLite (`discountdb` file) | Contract in `Protos/discount.proto` |
| `Ordering.API` | Minimal API (Carter) + CQRS + DDD | EF Core / SQL Server | Layered (Domain/Application/Infrastructure/API); consumes checkout events |
| `YarpApiGateway` | YARP reverse proxy | — | Routes + fixed-window rate limiting |
| `Shopping.Web` | Razor Pages | — | Calls the gateway via Refit typed clients |

### Vertical slice + CQRS pattern (Catalog, Basket)

These services use a **feature-folder** layout: each operation (e.g. `Products/CreateProduct/`) contains its own `*Endpoint.cs` (Carter `ICarterModule`) and `*Handler.cs` (MediatR command/query handler, validator, and request/response records — all in one file). The flow is:

`Carter endpoint` → maps request to command via Mapster `.Adapt<>()` → `ISender.Send` → MediatR handler → Marten `IDocumentSession`.

Shared CQRS plumbing lives in `BuildingBlocks/BuildingBlocks`:
- `CQRS/` — `ICommand`, `IQuery`, `ICommandHandler`, `IQueryHandler` (thin wrappers over MediatR's `IRequest`).
- `Behaviors/` — `ValidationBehavior` (runs FluentValidation validators) and `LoggingBehavior` are registered as MediatR open behaviors in every service's `Program.cs`.
- `Exceptions/Handler/CustomExceptionHandler` — global `IExceptionHandler` mapping domain exceptions to Problem Details; wired with `app.UseExceptionHandler(...)`.

When adding a feature to Catalog/Basket, copy an existing slice folder — the endpoint, handler, validator, and DTOs are expected to co-locate.

### Ordering — Clean/DDD architecture

Ordering is deliberately heavier and split into four projects:
- `Ordering.Domain` — aggregates (`Order`), entities, strongly-typed value-object IDs (`OrderId`, `CustomerId`, …), and domain events. No external dependencies.
- `Ordering.Application` — CQRS handlers (`Orders/Commands`, `Orders/Queries`), DTOs, and event handlers. `DependencyInjection.AddApplicationServices` registers MediatR behaviors, Feature Management, and the MassTransit message broker (with this assembly's consumers).
- `Ordering.Infrastructure` — `ApplicationDbContext`, EF configurations, and two `ISaveChangesInterceptor`s: `AuditableEntityInterceptor` (timestamps) and `DispatchDomainEventsInterceptor` (publishes domain events through MediatR on save).
- `Ordering.API` — Carter endpoints + composition root (`Program.cs` chains `AddApplicationServices` → `AddInfrastructureServices` → `AddApiServices`).

### Cross-service communication

- **Sync (gRPC):** Basket calls Discount. The proto contract is `Services/Discount/Discount.Grpc/Protos/discount.proto`; Basket consumes the generated `DiscountProtoService.DiscountProtoServiceClient` (TLS validation is bypassed for the self-signed dev cert).
- **Async (RabbitMQ via MassTransit):** Basket's `CheckoutBasketHandler` publishes a `BasketCheckoutEvent`; Ordering's `BasketCheckoutEventHandler` (`IConsumer`) creates an order from it. Integration event contracts are shared in `BuildingBlocks/BuildingBlocks.Messaging/Events`. Broker setup is centralized in `BuildingBlocks.Messaging/MassTransit/Extentions.cs` (`AddMessageBroker`, kebab-case endpoint names).
- **Gateway routing:** `YarpApiGateway/appsettings.json` maps public path prefixes (`/catalog-service`, `/basket-service`, `/ordering-service`) to internal service addresses and strips the prefix. The ordering route is rate-limited (`fixed` policy: 5 requests / 10s).

### Caching (Basket)

`CachedBasketRepository` decorates `BasketRepository` via Scrutor's `Services.Decorate<IBasketRepository, CachedBasketRepository>()` — a cache-aside layer over Redis. Don't add caching logic to `BasketRepository`; it belongs in the decorator.

## Ports & connection details (docker-compose)

External service ports: Catalog `6000`, Basket `6001`, Discount `6002`, Ordering `6003`, **Gateway `6004`**, Web `6005` (HTTP); add `60xx` for the HTTPS variants. Infra: PostgreSQL catalog `5432` / basket `5433`, Redis `6379`, SQL Server `1433`, RabbitMQ `5672` / management UI `15672`. Connection strings, broker creds, and feature flags are all set as environment variables in `docker-compose.override.yml` (not in `appsettings.json`).

## Conventions

- `GlobalUsing.cs` per project holds shared `using`s; new files rely on those implicit usings rather than re-importing common namespaces.
- Request/command/response/validator for a slice live in a single file alongside the handler.
- Strongly-typed IDs in Ordering wrap `Guid` — construct them via their factory rather than passing raw `Guid`s across the domain boundary.
