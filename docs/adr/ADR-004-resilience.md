# ADR-004 — `Microsoft.Extensions.Http.Resilience` for HTTP resilience

- Status: Accepted
- Date: 2026-06-25

## Context

External APIs fail, time out, and return `429 Too Many Requests`. Calls need retry, circuit breaking, and timeouts without hand-rolling the plumbing.

## Decision

Use **`Microsoft.Extensions.Http.Resilience`** with a custom pipeline per external client, registered on the typed `HttpClient`. For FoodData Central the pipeline applies exponential-backoff retry (on 429/503/504), a circuit breaker, and a per-attempt timeout.

## Consequences

- Cleaner API than wiring Polly policies directly (it uses Polly under the hood).
- Built-in OpenTelemetry instrumentation.
- The `AddStandardResilienceHandler()` defaults can be too aggressive for rate-limited APIs — hence the custom pipeline.

## Alternatives considered

- **Polly directly via `AddPolicyHandler`** — more verbose and an older API surface.
- **No resilience** — unacceptable; the whole point of the project is integrating with flaky external services.
