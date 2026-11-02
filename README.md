# The Royal Executioner

*(working title)*

A small, adults-only, dark-fantasy management game. You are the official executioner of an absurdly dysfunctional kingdom. Each day, condemned prisoners arrive with royal instructions, bureaucratic complications and unreasonable demands. Do the job, earn your keep, keep the axe sharp, navigate corruption, and survive the political machinery of the kingdom. The world takes its own ridiculous laws very seriously.

Tone: black comedy, grotesque absurdity, dark fantasy and the occasional genuine horror. Fictional gore is part of the appeal.

## Status

Concept/design and a first text-based playable prototype (open `Assets/Scenes/TextPrototype.unity` and press Play). There is no art. See the planning documents for what is agreed and what is still open.

## Documents

- [Game Design Document](Docs/GDD.md): living design doc (agreed / provisional / unresolved)
- [Idea Bank](Docs/IdeaBank.md): exploratory ideas, not commitments
- [Prototype Plan](Docs/PrototypePlan.md): smallest meaningful playable prototype

## Repository layout

```
TheRoyalExecutioner/        repository root
  Docs/                     planning documents
  TheRoyalExecutioner/      Unity project (open this folder in Unity Hub)
  .mcp.json                 Claude Code MCP config (repo-scoped)
  .codex/config.toml        Codex MCP config (repo-scoped)
```

## Unity project

- Unity **6000.6.7f1**, URP, 3D project template, Input System.
- Project path: `TheRoyalExecutioner/` (nested one level below the repository root).

## Dependencies (Unity Package Manager, Git URLs, pinned tags)

| Package | Version | Source |
| --- | --- | --- |
| `com.notoriouspig.framework` | v0.3.0 | `https://github.com/SamuelJay/NotoriousPIGFramework.git?path=/Package#v0.3.0` |
| `com.notoriouspig.character` | v0.3.0 | `https://github.com/SamuelJay/NotoriousPIGCharacter.git?path=/Package#v0.3.0` |
| `com.coplaydev.unity-mcp` (MCP for Unity) | v10.3.0 | `https://github.com/CoplayDev/unity-mcp.git?path=/MCPForUnity#v10.3.0` |

The framework and character repositories are private; Git must be able to authenticate to GitHub as `SamuelJay` for Unity to resolve them. The character package is installed but no character behaviour is used yet.

## Setup

1. Install Unity 6000.6.7f1 and make sure Git is on your PATH and authenticated to GitHub.
2. Open the `TheRoyalExecutioner/` subfolder (not the repository root) in Unity Hub. Let Package Manager resolve the Git packages.
3. Commit the updated `Packages/packages-lock.json` after the first successful resolve.

### Unity MCP (optional, for AI-assisted editing)

Requires [`uv`](https://docs.astral.sh/uv/) (`uvx` on PATH).

1. In Unity: **Window > MCP for Unity**, and start the bridge.
2. Claude Code: run `claude` from the repository root and approve the project-scoped `UnityMCP` server from `.mcp.json`.
3. Codex: mark the repository as trusted so `.codex/config.toml` is loaded.

The server version in `.mcp.json` / `.codex/config.toml` (`mcpforunityserver==10.3.0`) should match the Unity package version.
