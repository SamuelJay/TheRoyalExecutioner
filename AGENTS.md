# Agent Guidance

The Royal Executioner is a dark-fantasy executioner management game. The Unity project lives in the nested `TheRoyalExecutioner/` folder; `Docs/` and planning files sit at the repository root.

- Read `Docs/DevelopmentStandards.md` before writing or modifying code.
- Current design and scope: `Docs/GDD.md` and `Docs/PrototypePlan.md`. Consult `Docs/IdeaBank.md` only when creative content is relevant.
- Reuse the relevant existing framework (`com.notoriouspig.framework`) and character-package (`com.notoriouspig.character`) conventions.
- The game is being developed text-first; gameplay resolution stays independent of presentation.
- Follow existing Unity testing guidance where present.
- Use Unity MCP for specific, useful integration or runtime checks, not automatically for every task.
- Work on the current branch and preserve existing changes. Do not commit or push without explicit permission.
- Own the outcome: investigate where needed, implement, verify, fix failures caused by your change, and report actual results.
- Escalate only for material contradictions, consequential decisions, substantial scope expansion or serious unresolved risks.
