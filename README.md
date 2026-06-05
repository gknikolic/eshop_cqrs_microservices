# eShop Microservices

A .NET 8 e-commerce reference application built as a set of independently deployable microservices behind a YARP API gateway, with a Razor Pages storefront. It demonstrates CQRS, vertical-slice and Clean/DDD architectures, polyglot persistence, and both synchronous (gRPC) and asynchronous (RabbitMQ) inter-service communication.

## Architecture

```
                        ┌─────────────────┐
                        │  Shopping.Web   │  Razor Pages (Refit clients)
                        └────────┬────────┘
                                 │ HTTP
                        ┌────────▼────────┐
                        │  YarpApiGateway │  reverse proxy + rate limiting
                        └───┬────────┬────┘
              /catalog      │        │      /ordering
             ┌──────────────▼─┐  ┌───▼──────────────┐
   /basket   │  Catalog.API   │  │   Ordering.API   │
  ┌──────────▼──┐  PostgreSQL │  │  SQL Server      │
  │  Basket.API │  (Marten)   │  │  (EF Core, DDD)  │
  │ PostgreSQL  └─────────────┘  └───────▲──────────┘
  │ + Redis     │                        │
  │      │ gRPC ▼                         │ RabbitMQ (MassTransit)
  │  ┌───────────────┐                    │  BasketCheckoutEvent
  │  │ Discount.Grpc │      Basket ───────┘──► Ordering
  │  │  SQLite       │
  │  └───────────────┘
  └─────────────┘
```

### Services

| Service | Responsibility | API | Data store |
|---|---|---|---|
| **Catalog.API** | Product catalog | Minimal API (Carter) + CQRS | PostgreSQL via Marten |
| **Basket.API** | Shopping cart & checkout | Minimal API (Carter) + CQRS | PostgreSQL via Marten + Redis cache |
| **Discount.Grpc** | Product discount coupons | gRPC | SQLite |
| **Ordering.API** | Order placement & fulfillment | Minimal API (Carter) + CQRS + DDD | SQL Server via EF Core |
| **YarpApiGateway** | Single public entry point, routing, rate limiting | YARP | — |
| **Shopping.Web** | Customer-facing storefront | Razor Pages + Refit | — |

**Shared building blocks** (`src/BuildingBlocks`): CQRS abstractions, MediatR validation/logging behaviors, global exception handling, pagination, and the MassTransit/RabbitMQ messaging setup with shared integration-event contracts.

## Tech stack

.NET 8 · Carter (minimal-API modules) · MediatR (CQRS) · Marten · Entity Framework Core · MassTransit + RabbitMQ · gRPC · Redis · YARP · Refit · FluentValidation · Mapster · Docker · Kubernetes.

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Run everything

```powershell
cd src
docker compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

This builds the service images and starts all microservices plus their infrastructure (PostgreSQL, Redis, SQL Server, RabbitMQ). Databases are migrated/seeded automatically on startup in the Development environment.

Stop with:

```powershell
docker compose -f docker-compose.yml -f docker-compose.override.yml down
```

### Endpoints

| Component | HTTP | HTTPS |
|---|---|---|
| Storefront (Shopping.Web) | http://localhost:6005 | https://localhost:6065 |
| API Gateway | http://localhost:6004 | https://localhost:6064 |
| Catalog.API | http://localhost:6000 | https://localhost:6060 |
| Basket.API | http://localhost:6001 | https://localhost:6061 |
| Discount.Grpc | http://localhost:6002 | https://localhost:6062 |
| Ordering.API | http://localhost:6003 | https://localhost:6063 |
| RabbitMQ management | http://localhost:15672 (guest/guest) | |

Through the gateway, services are reachable under path prefixes: `/catalog-service`, `/basket-service`, `/ordering-service`.

Each API exposes a health check at `/health`.

## Development

Build the solution:

```powershell
cd src
dotnet build eshop-microservices.sln
```

Run a single service (against infrastructure started via Docker):

```powershell
dotnet run --project Services/Catalog/Catalog.API
```

### Testing the APIs

Import `src/EShopMicroservices.postman_collection.json` into Postman to exercise the endpoints. (There are no automated test projects in this repository.)

### Database migrations

Ordering (SQL Server) and Discount (SQLite) use EF Core:

```powershell
cd src
dotnet ef migrations add <Name> --project Services/Ordering/Ordering.Infrastructure --startup-project Services/Ordering/Ordering.API
dotnet ef migrations add <Name> --project Services/Discount/Discount.Grpc
```

Catalog and Basket use Marten and create their schemas automatically at runtime.

## Kubernetes

Deployment manifests live alongside each service (`*-deployment.yaml`) and under `src/Kubernetes`. Deploy them all to the `eshop-microservices` namespace:

```bat
cd src\Kubernetes\Kubernetes
kubernetes-compose.bat
```

## Project layout

```
src/
├── ApiGateways/YarpApiGateway/        # YARP reverse proxy
├── BuildingBlocks/
│   ├── BuildingBlocks/                # CQRS, behaviors, exceptions, pagination
│   └── BuildingBlocks.Messaging/      # MassTransit setup + integration events
├── Services/
│   ├── Catalog/Catalog.API/
│   ├── Basket/Basket.API/
│   ├── Discount/Discount.Grpc/
│   └── Ordering/                      # Domain / Application / Infrastructure / API
├── WebApps/Shopping.Web/              # Razor Pages storefront
├── Kubernetes/                        # k8s manifests + deploy script
└── docker-compose*.yml
```
