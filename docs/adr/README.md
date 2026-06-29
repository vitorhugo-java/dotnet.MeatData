# Architecture Decision Records (ADRs)

This folder records the significant architectural decisions for **MeatData Portal**, why they were made, and what they cost. Each ADR is immutable once accepted — if a decision changes, a new ADR supersedes the old one rather than editing history.

> Reminder: this is a learning/portfolio project and is deliberately over-engineered. Several decisions below are honest about being overkill at this scale — that's intentional. See the [root README](../../README.md#️-a-note-before-you-judge-the-architecture).

| ADR | Decision | Status |
| :-- | :------- | :----- |
| [ADR-001](ADR-001-clean-architecture.md) | Clean Architecture as the base organization | Accepted |
| [ADR-002](ADR-002-postgresql.md) | PostgreSQL as the primary database | Accepted |
| [ADR-003](ADR-003-redis-cache.md) | Redis for caching external API responses | Accepted |
| [ADR-004](ADR-004-resilience.md) | `Microsoft.Extensions.Http.Resilience` for HTTP resilience | Accepted |
| [ADR-005](ADR-005-result-pattern.md) | Result pattern for expected business errors | Accepted |

## Template

```text
# ADR-NNN — <title>
- Status: Proposed | Accepted | Superseded by ADR-XXX
- Date: YYYY-MM-DD

## Context
What problem are we solving? What constraints apply?

## Decision
What we chose to do.

## Consequences
The good, the bad, and the trade-offs we accept.

## Alternatives considered
What we rejected and why.
```
