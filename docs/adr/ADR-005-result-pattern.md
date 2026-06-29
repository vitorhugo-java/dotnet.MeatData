# ADR-005 — Result pattern for expected business errors

- Status: Accepted
- Date: 2026-06-28

## Context

Expected business failures (duplicate SKU, missing category, product not found) shouldn't be modeled as exceptions. Exceptions are for the unexpected; throwing them for ordinary control flow is expensive and hides intent.

## Decision

Return a custom **`Result<T>`** (`Application/Common/Result.cs`) from Application handlers for expected outcomes, carrying an error message and an error code. Controllers switch on the error code to translate into the correct HTTP status (e.g. `DUPLICATE_SKU` → 409, `PRODUCT_NOT_FOUND` → 404). Infrastructure failures (DB down, timeouts) still throw.

## Consequences

- Business errors become explicit in the method contract instead of hiding in `catch` blocks.
- More verbose — every fallible method returns `Result<T>` instead of `T`.
- Requires team discipline to not mix the two styles.

## Alternatives considered

- **FluentResults** — capable, but adds a dependency for something small enough to own.
- **Exceptions for everything** — simpler to write, but conflates expected and exceptional flows and pushes error handling far from where it occurs.
