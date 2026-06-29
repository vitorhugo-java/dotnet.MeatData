# ADR-001 — Clean Architecture as the base organization

- Status: Accepted
- Date: 2026-06-22

## Context

The project needed a structure that clearly demonstrates separation of concerns and makes the domain easy to test in isolation. As a portfolio piece, the layout should be familiar to reviewers and easy to explain in an interview.

## Decision

Adopt Clean Architecture with four projects and an inward-pointing dependency rule:

- `MeatData.Domain` — entities, enums, exceptions. Zero external dependencies.
- `MeatData.Application` — use cases (CQRS commands/queries), DTOs, interfaces. Depends only on `Domain`.
- `MeatData.Infrastructure` — EF Core, repositories, HTTP clients, cache. Implements `Application` interfaces.
- `MeatData.Api` — controllers, DI wiring, pipeline. Composition root.

## Consequences

- The domain is testable without spinning up a database or HTTP.
- Swapping PostgreSQL for SQL Server, or EF Core for Dapper, touches only `Infrastructure`.
- More projects and boilerplate than a simple CRUD needs.
- Risk of an anemic domain model if entities degrade into bare POCOs — mitigated by keeping behavior (factory methods, invariants) on `Product`.

## Alternatives considered

- **Vertical Slice Architecture** — more modern and arguably a better fit for an app this size, but less familiar as a starting point and less illustrative of layered separation for a portfolio.
- **Single-project minimal API** — the right call for an app this small in production, but it wouldn't demonstrate the patterns this project exists to practice.
