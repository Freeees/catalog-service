# CatalogService (.NET 8) — Production-ready microservice template

A small but production-minded backend microservice that demonstrates:
- REST API (CRUD)
- Clean Architecture / Hexagonal style
- Dependency Injection
- PostgreSQL + EF Core Migrations
- Swagger / OpenAPI
- Serilog logging + error handling
- Unit + Integration tests (xUnit + Testcontainers)
- Docker + docker-compose (with persistent volume)

---

## Tech stack

- .NET 8 / ASP.NET Core
- PostgreSQL 16
- EF Core 8 + Npgsql
- xUnit + FluentAssertions
- DotNet.Testcontainers (PostgreSQL)
- Serilog
- Swashbuckle (Swagger)

---

## Project structure

```
src/
  CatalogService.Api/             # HTTP layer (controllers, swagger, DI, middleware, Serilog)
  CatalogService.Application/     # Use cases (commands/queries/services, DTOs)
  CatalogService.Domain/          # Domain model (entities, value objects, domain rules)
  CatalogService.Infrastructure/  # EF Core, repositories, persistence, migrations, adapters
tests/
  CatalogService.UnitTests/       # fast tests for Domain/Application
  CatalogService.IntegrationTests/# API + Postgres via Testcontainers
docker-compose.yml
global.json
CatalogService.sln
```

**Architecture idea (Clean / Hexagonal):**
- `Domain` has no dependencies.
- `Application` depends only on `Domain`.
- `Infrastructure` implements ports/adapters (EF Core, repositories).
- `Api` is the entrypoint (HTTP) and wires everything via DI.

---

## Prerequisites

- .NET SDK **8.x**
- Docker Desktop

Check versions:

```bash
dotnet --version
docker --version
docker compose version
```

---

## Run locally (Docker PostgreSQL)

### 1. Start PostgreSQL

```bash
docker compose up -d postgres
```

Uses a **persistent Docker volume**.

### 2. Apply migrations

```bash
dotnet ef database update \
  --project src/CatalogService.Infrastructure \
  --startup-project src/CatalogService.Api
```

### 3. Run API

```bash
dotnet run --project src/CatalogService.Api
```

### 4. Swagger UI

```
http://localhost:8080/swagger
```

---

## Configuration

Connection string key:

```
ConnectionStrings:Postgres
```

Injected via:
- appsettings (local/dev)
- environment variables (tests, CI)
- Testcontainers (integration tests)

---

## Logging

Serilog is configured in `CatalogService.Api`:
- structured logs
- request/response pipeline logging
- exception details

---

## Tests

### Run all tests

```bash
dotnet test
```

### Unit tests

```bash
dotnet test tests/CatalogService.UnitTests
```

### Integration tests (API + PostgreSQL)

```bash
dotnet test tests/CatalogService.IntegrationTests
```

Integration tests:
- start PostgreSQL via Testcontainers
- use dynamic port binding
- inject connection string at runtime

> Docker Desktop must be running.

---

## Docker data persistence

- Database data is stored in a named Docker volume
- Containers are disposable; data persists

Commands:

```bash
docker compose down        # containers removed, data kept
docker compose down -v     # containers + data removed
```

---

## Typical developer workflow

1. `docker compose up -d postgres`
2. `dotnet ef database update`
3. `dotnet run --project src/CatalogService.Api`
4. Use Swagger
5. `dotnet test`

---

## Notes

- Keep EF Core / Npgsql within the same major (.NET 8)
- `global.json` pins SDK to avoid .NET 9/10 conflicts
- Integration tests require Docker

---

## License

MIT