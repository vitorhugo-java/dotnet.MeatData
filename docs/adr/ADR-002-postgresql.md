# ADR-002 — PostgreSQL as the primary database

- Status: Accepted
- Date: 2026-06-22

## Context

The project needs a relational, open-source database with first-class .NET support and a clean story for integration testing with Testcontainers.

## Decision

Use **PostgreSQL 16** via `Npgsql.EntityFrameworkCore.PostgreSQL`. Local instance runs in Docker (`postgres:16-alpine`) through `docker-compose`.

## Consequences

- Requires Docker locally.
- Excellent fit for later roadmap projects (e.g. `pgvector` in Project 4).
- Mature EF Core provider and Testcontainers module.

## Alternatives considered

- **SQL Server** — works just as well with EF Core, but heavier in Docker and less relevant for an open-source portfolio.
- **SQLite** — trivial to run, but diverges from production behavior (types, concurrency) and wouldn't exercise a real provider.
