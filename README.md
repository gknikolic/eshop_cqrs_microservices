# eShop Microservices

A .NET 8 e-commerce reference application built as a set of independently deployable microservices behind a YARP API gateway, with a Razor Pages storefront. It demonstrates **service-level CQRS** (a read/write-split Catalog kept in sync by events), event-driven integration over RabbitMQ, polyglot persistence, JWT authentication, and both synchronous (gRPC) and asynchronous (messaging) inter-service communication.

## Architecture

```
                          ┌─────────────────┐
                          │  Shopping.Web   │  Razor Pages (cookie auth + Refit)
                          └────────┬────────┘
                                   │ HTTP + Bearer token
                          ┌────────▼────────┐
                          │  YarpApiGateway │  routing · rate limiting · JWT validation
                          └──┬───┬───┬───┬──┘
        GET /catalog ───────┘   │   │   └─────── /customer (issues JWT)
        write /catalog ─────┐   │   └───── /ordering        │
                            │   └─ /basket, /inventory      ▼
              ┌─────────────▼──┐  ┌──────────────┐   ┌──────────────┐
              │ Catalog.Write  │  │  Catalog.Read│   │  Customer.API│
              │ SQL Server,DDD │  │  PostgreSQL  │   │  SQL Server  │
              └───────┬────────┘  │  (read model)│   │  (Identity)  │
                      │           └──────▲───────┘   └──────────────┘
                      │  Product events  │
                      └──────────────────┘  (RabbitMQ / MassTransit)
                      │                                  ▲
                      ▼                                  │
              ┌──────────────┐   BasketCheckout   ┌──────────────┐
              │ Inventory.API│◄──OrderCreated──────│ Ordering.API │
              │  PostgreSQL  │                     │  SQL Server  │
              └──────────────┘                     └──────▲───────┘
              ┌──────────────┐  gRPC                      │ BasketCheckout
              │ Discount.Grpc│◄──── Basket.API ───────────┘
              │   SQLite     │      PostgreSQL + Redis
              └──────────────┘
```

### Services

| Service | Responsibility | API | Data store |
|---|---|---|---|
| **Catalog.Read.API** | Product queries (read model) | Minimal API (Carter) + CQRS | PostgreSQL via Marten |
| **Catalog.Write.API** | Product commands (DDD) | Minimal API (Carter) + CQRS + DDD | SQL Server via EF Core |
| **Basket.API** | Shopping cart & checkout | Minimal API (Carter) + CQRS | PostgreSQL via Marten + Redis cache |
| **Inventory.API** | Stock levels | Minimal API (Carter) + CQRS | PostgreSQL via Marten |
| **Customer.API** | Identity, login, JWT issuance | Minimal API (Carter) + ASP.NET Identity | SQL Server via EF Core |
| **Discount.Grpc** | Discount coupons | gRPC | SQLite |
| **Ordering.API** | Order placement & fulfillment | Minimal API (Carter) + CQRS + DDD | SQL Server via EF Core |
| **YarpApiGateway** | Single entry point, routing, rate limiting, JWT validation | YARP | — |
| **Shopping.Web** | Customer-facing storefront | Razor Pages + Refit | — |

### Key patterns

- **Read/write CQRS for Catalog.** Writes go to a DDD-modeled SQL Server service; the gateway routes catalog `GET`s to a separate Marten/PostgreSQL read service. The write side raises domain events that become integration events, and the read side rebuilds its denormalized store by consuming them.
- **Event-driven integration.** Services communicate asynchronously through RabbitMQ (MassTransit). Product, customer, order, and inventory lifecycle events fan out to interested services. Contracts are shared in `BuildingBlocks.Messaging`.
- **JWT auth.** `Customer.API` issues tokens (ASP.NET Identity); every API and the gateway validate them with a shared signing key. The web app uses cookie auth and forwards the bearer token, refreshing it transparently.

**Shared building blocks** (`src/BuildingBlocks`): CQRS abstractions, MediatR validation/logging behaviors, DDD base types, JWT authorization helpers, global exception handling, pagination, and the MassTransit messaging setup with shared integration-event contracts.

## Tech stack

.NET 8 · Carter · MediatR (CQRS) · Marten · Entity Framework Core · ASP.NET Identity · JWT Bearer · MassTransit + RabbitMQ · gRPC · Redis · YARP · Refit · FluentValidation · Mapster · Docker · Kubernetes.

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Run everything

```powershell
cd src
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

This builds the service images and starts all microservices plus their infrastructure (PostgreSQL ×3, SQL Server ×3, Redis, RabbitMQ). Databases are migrated/seeded automatically in the Development environment — including a default admin user created by Customer.API on startup.

Stop with:

```powershell
docker compose -f docker-compose.yml -f docker-compose.override.yml down
```

### Endpoints

| Component | HTTP | HTTPS |
|---|---|---|
| Storefront (Shopping.Web) | http://localhost:6005 | https://localhost:6065 |
| API Gateway | http://localhost:6004 | https://localhost:6064 |
| Catalog.Read.API | http://localhost:6000 | https://localhost:6060 |
| Basket.API | http://localhost:6001 | https://localhost:6061 |
| Discount.Grpc | http://localhost:6002 | https://localhost:6062 |
| Ordering.API | http://localhost:6003 | https://localhost:6063 |
| Inventory.API | http://localhost:6006 | https://localhost:6066 |
| Customer.API | http://localhost:6007 | https://localhost:6067 |
| Catalog.Write.API | http://localhost:6008 | https://localhost:6068 |
| RabbitMQ management | http://localhost:15672 (guest/guest) | |

Through the gateway, services are reachable under path prefixes: `/catalog-service` (GET → read service, POST/PUT/DELETE → write service), `/basket-service`, `/inventory-service`, `/ordering-service`, `/customer-service`.

Each API exposes a health check at `/health`.

## Development

Build the solution:

```powershell
cd src
dotnet build eshop-microservices.sln
```

Run a single service (against infrastructure started via Docker):

```powershell
dotnet run --project Services/Catalog/CatalogRead/Catalog.Read.API
```

### Testing the APIs

Import `src/EShopMicroservices.postman_collection.json` into Postman. (There are no automated test projects in this repository.)

### Database migrations

EF Core is used by Catalog.Write, Ordering, Customer (SQL Server) and Discount (SQLite). Catalog.Read, Basket, and Inventory use Marten and create their schemas at runtime.

```powershell
cd src
dotnet ef migrations add <Name> --project Services/Catalog/CatalogWrite/Catalog.Write.Infrastructure --startup-project Services/Catalog/CatalogWrite/Catalog.Write.API
dotnet ef migrations add <Name> --project Services/Ordering/Ordering.Infrastructure --startup-project Services/Ordering/Ordering.API
dotnet ef migrations add <Name> --project Services/Discount/Discount.Grpc
```

## Kubernetes

Manifests live in the top-level `deployments/` folder.

```bat
cd deployments
deploy_services.bat   :: creates the `eshop` namespace and applies all manifests
clear_services.bat    :: tears it down
```

`ingress.yaml` exposes the storefront via nginx ingress at host `shopping-web.local`.

## Project layout

```
src/
├── ApiGateways/YarpApiGateway/          # YARP reverse proxy (method-based catalog routing)
├── BuildingBlocks/
│   ├── BuildingBlocks/                  # CQRS, DDD base types, behaviors, JWT auth, exceptions
│   └── BuildingBlocks.Messaging/        # MassTransit setup + integration event contracts
├── Services/
│   ├── Catalog/
│   │   ├── CatalogWrite/                # Domain / Application / Infrastructure / API (SQL Server)
│   │   └── CatalogRead/Catalog.Read.API # Marten read model (PostgreSQL)
│   ├── Basket/Basket.API/
│   ├── Inventory/Inventory.API/
│   ├── Customer/Customer.API/           # ASP.NET Identity + JWT issuer
│   ├── Discount/Discount.Grpc/
│   └── Ordering/                        # Domain / Application / Infrastructure / API
├── WebApps/Shopping.Web/                # Razor Pages storefront (cookie auth)
└── docker-compose*.yml
deployments/                             # Kubernetes manifests + deploy/clear scripts
```
