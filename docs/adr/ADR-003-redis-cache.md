# ADR-003 — Redis for caching external API responses

- Status: Accepted
- Date: 2026-06-25

## Context

FoodData Central is rate-limited (1,000 requests/hour per key) and its nutritional data changes rarely. Repeatedly hitting it for the same query is wasteful and fragile.

## Decision

Cache external responses in **Redis** via `IDistributedCache`, applied with the **Cache-Aside** pattern through a **Decorator** (`CachedFoodDataCentralClient`) that wraps the real typed client. TTL is configurable per data type (search vs. detail) through `FoodDataCentralOptions`.

## Consequences

- Drastically reduces external calls and speeds up repeated queries.
- Adds Redis to the local infrastructure footprint.
- Stale data until the TTL expires — acceptable given how rarely the source changes.
- The decorator keeps caching out of the real client, preserving single responsibility.

> Implementation note: Redis is wired in `Program.cs` today, but a Redis service still needs to be added to `docker-compose.yml`.

## Alternatives considered

- **`IMemoryCache`** — simpler, but no persistence across restarts and doesn't scale to multiple API instances.
- **Caching inside the Application handler** — works, but leaks an infrastructure concern into the use case and muddies responsibilities.
