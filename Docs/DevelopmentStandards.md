# Development Standards

Standards for all code written or modified in this repository, by people or coding agents.

## Engineering philosophy

- Priorities, in order: correctness, maintainability, the smallest technically sound change, then implementation speed.
- Prefer reusable, generic, readable systems and SOLID/OOP principles.
- Systems should be independently usable building blocks where practical.
- Prefer Unity's intended mechanisms over bypasses.
- Integrate with the existing framework architecture rather than replacing or duplicating it.
- Parameterise existing suitable systems rather than rewriting them.
- Do not introduce speculative abstractions or unrelated refactors.

## C# conventions

- Fields, variables and properties: camelCase.
- Methods and classes: PascalCase.
- Never use `var`; use explicit types.
- Use full descriptive words. Permitted acronyms: AI, UI, ID, NPC.
- Handle null intentionally; no silent null checks that hide invalid state.
- Use the appropriate Unity or framework base class wherever one exists and is relevant. Plain C# data models and utility classes do not require artificial inheritance.
- No duplicated logic.
- Prefer `[SerializeField] private` over public serialized fields.
- Avoid magic numbers. Use named constants or configuration data where appropriate.
- Avoid magic strings for identifiers, lookups, states and other program logic. Use constants, enums or strongly typed references where appropriate. Player-facing dialogue, narrative content, UI labels and log messages are exempt. Prefer keeping authored game content in data assets rather than scattering it through gameplay code.
- Single Responsibility Principle. Aim for roughly 25 lines per method as guidance, not a hard limit.
- K&R braces: opening braces on the same line; `else` follows the closing brace.
- Conditions and parameters normally stay on one line unless genuinely unreadable.
- No redundant `else` after `return`.

## Comments and documentation

- Do not add code comments or XML documentation unless explicitly requested.
- Prefer self-explanatory naming, types and structure.
- Do not add comments explaining obvious logic, architecture, implementation history or task identifiers.
- If a hidden constraint genuinely requires explanation, propose the comment and its justification rather than adding it automatically.
- Preserve existing comments unless the current change makes them incorrect or obsolete.
- Concise Unity authoring tooltips are allowed.

## Change discipline

- Understand relevant existing code before structural changes.
- Audit when material uncertainty affects implementation, but do not repeat established investigations without reason.
- Use the smallest technically sound change, not a superficial patch.
- Preserve framework ownership and existing architectural contracts.
- Avoid unnecessary static state outside appropriate managers.
- Do not bypass the framework for convenience.
- Prefer verifiable behaviour over assumed correctness.
- Match testing to the actual regression surface.
- Do not weaken tests to make them pass.
- Do not claim runtime verification without runtime evidence.

## Implementation handoff

Report:

- Changed files and why.
- Meaningful implementation decisions.
- Verification performed and actual results.
- Remaining risks and manual checks.
- What was deliberately not changed.

Use reliable diffs rather than printing entire files unnecessarily.
