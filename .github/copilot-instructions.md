# Copilot Instructions

## 1. Scope and repository topology

Single repository (`antshc/benefits`): docs and codebase live together. There is no `workspace/` split.

- **Docs live at the repo root:** `GLOSSARY.md` (domain glossary) and `ARCHITECTURE.md` (system map, ADR/Concept indexes), with ADRs under `docs/adr/` and Crosscutting Concepts under `docs/concepts/`.
- **Source code lives in `benefits-calc/`:** the .NET 10 `PaylocityBenefitsCalculator` solution (`Api/` and `ApiTests/`).

Within this file, resolve conflicts in this order: **safety and repository targeting → authoritative sources → navigation → build and validation → documentation conventions.** This ordering scopes only the rules in this file; it does not override `AGENTS.md`, path-scoped instructions, or user instructions.

## 2. Safety and repository targeting

Before the first `git`/`gh pr` action in a session, verify `origin` is `antshc/benefits`:

```bash
git remote get-url origin
git rev-parse --show-toplevel
```

All `gh issue`, `gh pr`, and `gh api` commands target `antshc/benefits`. GitHub issues are the only backlog backend.

## 3. Authoritative sources

Consult these before searching the code:

- **Domain glossary:** [`GLOSSARY.md`](../GLOSSARY.md) — domain terminology.
- **Architecture:** [`ARCHITECTURE.md`](../ARCHITECTURE.md) — system overview and indexes.
- **Requirements:** [`README.md`](../README.md) — coding-challenge requirements and benefit-cost rules.

## 4. Code navigation

The code is under `benefits-calc/Api/`, organised by layer: `Application/` (controllers, payloads, queries, services), `Domain/` (employees, paychecks, deduction policies, business rules), `Data/` (EF Core context, repository, migrations), `SharedKernel/`. Tests are in `benefits-calc/ApiTests/` (`UnitTests/`, `IntegrationTests/`).

Read the relevant authoritative source first, then scope searches to `benefits-calc/`. Exclude `bin/`, `obj/`, and `graphify-out/` from text search.

## 5. Build and validation

Run from the repo root:

```bash
dotnet build benefits-calc/PaylocityBenefitsCalculator.sln
dotnet test benefits-calc/PaylocityBenefitsCalculator.sln
```

## 6. Documentation conventions

- Keep `GLOSSARY.md` and `ARCHITECTURE.md` high-level — no implementation details, specs, or scratch notes.
- Document code decisions in comments explaining _why_ (per the README requirements).

## 7. Skills

- Create every skill under `.github/skills/<skill-name>/` — each in its own folder holding `SKILL.md` plus any reference files.
- Do not reference a skill's own files via bare relative markdown links; load them via the skill's absolute base directory.
