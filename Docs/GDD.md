# The Royal Executioner: Game Design Document

*Working title. Living document.*

Legend: **[Agreed]** current working decision · **[Provisional]** an idea being considered, not committed · **[Open]** unresolved question.

---

## 1. High concept

**[Agreed]** A small, independently released, adults-only dark-fantasy management game. The player is the official executioner of an absurdly dysfunctional kingdom. Condemned prisoners arrive daily with royal instructions, bureaucratic complications and unreasonable demands. The player does the job, earns money, manages equipment, navigates corruption and survives the political machinery of the kingdom.

**Player fantasy [Agreed]:** a functionary who is not the judge, but whose position offers quiet power over what actually happens on the scaffold.

## 2. Genre, scope, platform

- **[Agreed]** Management game with simple controls; casual in feel.
- **[Agreed]** Relatively small, achievable indie scope. Small does not mean trivial: content, writing, animation and balancing remain real workload.
- **[Agreed]** Unity; initial target PC/Steam.
- **[Agreed]** Finite, replayable campaign with potential alternative endings.
- **[Open]** Campaign length, prisoner counts, number of endings.

## 3. Tone and intended experience

**[Agreed]**
- Black humour, grotesque absurdity, dark fantasy, occasional genuine horror.
- The world takes its own ridiculous laws seriously.
- Influences: Terry Pratchett's Discworld (absurd institutions, bureaucratic comedy), Iain M. Banks' Culture novels (strange customs, imaginative grotesquery), Game of Thrones (gritty, politically uncomfortable situations), Papers, Please and Death and Taxes (compact, consequential management decisions).
- Adults only. Fictional gore and graphic executions may be part of the appeal and are not automatically softened.
- All characters, dialogue, setting and writing are original.
- Where the world is brutal about abuse, the humour targets the oppressive institution, not its victims.

## 4. Core daily loop

**[Agreed, in outline]** Management decisions shape what happens on the scaffold; executions shape later management. Gameplay revolves around processing prisoners, making decisions, dealing with authorities, maintaining equipment and experiencing consequences.

**[Provisional]** A day might look like: receive orders and prisoner records, decide how to handle each case, prepare equipment, carry out (or avoid) executions, settle the day's money and consequences, start the next day.

**[Open]** Exact steps, how long a day lasts, how many prisoners per day, what is shown versus abstracted.

## 5. Player actions

The executioner is not the judge. Their official role is to carry out sentences; their position offers opportunities to influence what actually happens. **[Agreed]** as a premise. Everything below is a **[Provisional]** list of possibilities, not mechanics to implement.

**Official**
- Review execution orders and prisoner records.
- Schedule or delay executions.
- Maintain equipment and manage money.
- Request clarification or intervention from judges and officials.
- Seek pardons or challenge administrative errors.
- Manage special sentencing requirements.

**Unofficial**
- Accept bribes.
- Manipulate records or schedules.
- Conceal information.
- Help prisoners escape.
- Fake executions or substitute prisoners.
- Cooperate with or defy political factions.

**[Open]** Which of these become actual actions, and which are only narrative options.

## 6. Management pressures

**[Provisional]** Actions could affect money, royal favour, public sentiment, suspicion, relationships, equipment, and subsequent narrative events.

**[Open]** Which of these need to be formal game systems at all, as opposed to story flags or implicit consequences. No resource list is agreed.

## 7. Execution interaction

- **[Agreed]** Executions are primarily a visual spectacle and a consequence of management decisions, not an elaborate skill-based combat minigame.
- **[Agreed]** Performing an execution might be as simple as a click, tap or button press.
- **[Provisional]** The initial method is an axe and chopping block. Equipment condition, preparation and management decisions could influence success, failure and spectacle.
- **[Provisional]** More elaborate mechanical or magical apparatus may come later.
- **[Open]** Whether there is any contraption-building, physics simulation or execution puzzle at all. Nothing is committed.

## 8. Campaign structure and consequences

- **[Agreed]** Finite campaign, replayable, possibly with alternative endings.
- **[Agreed]** Escalating political instability and potentially a major upheaval.
- **[Provisional]** Saving or executing particular prisoners could influence later events (revolution, assassination, succession crisis, regime change). Officials who were judges or accusers might later become prisoners. The executioner's own name might eventually appear on the list.
- **[Provisional]** Possible endings: surviving in royal service, being executed, becoming a victim of revolution, escaping the kingdom, helping change the regime.
- **[Open]** Actual story events, ending list, victory and failure conditions, number of days.

## 9. Narrative direction

**[Agreed]** Original, episodic encounters with prisoners and officials, carrying a slowly escalating political thread. Specific story content stays undecided. See [IdeaBank.md](IdeaBank.md) for exploratory material.

**[Open]** How much is hand-authored versus assembled from reusable parts; how much prisoner variation is systemic.

## 10. Presentation and technical direction

- **[Agreed]** Unity 6 (6000.6.7f1), URP, initially PC/Steam.
- **[Agreed]** The project uses a 3D setup for flexibility. This does not commit to 3D final artwork.
- **[Agreed]** Placeholder assets initially.
- **[Agreed]** Built on the Notorious P.I.G. framework (services/bootstrap/events/scenes) and, where useful, the separate character package. Neither is used by any game code yet.
- **[Open]** Visual style, animation pipeline, final presentation, audio direction, UI approach.

### Development approach: text first

**[Agreed]** This is a development approach, not a decision that the released game will be text-based. The spectacle described in section 7 remains part of the intended game.

- The initial game is developed inside Unity using simple text and UI controls.
- The initial focus is prisoner management, decisions, resources, consequences, progression and narrative.
- Executions initially resolve through gameplay rules and display textual outcomes.
- A visual scaffold, character animation, execution effects and physical spectacle are introduced in a later phase.
- Gameplay state, decision resolution and execution outcomes should stay independent of their visual presentation, so presentation can be added later without rewriting the rules. No particular architecture is prescribed.

## 11. Confirmed decisions (summary)

- Small management game, simple controls, PC/Steam first, Unity.
- Executioner is not the judge; the role offers influence through official and unofficial routes.
- Executions are spectacle and consequence, not skill minigames.
- Management affects the scaffold; the scaffold affects later management.
- Finite replayable campaign, escalating political instability, undecided events and endings.
- Adult tone, with gore not automatically removed.
- Placeholder assets first; visual style undecided.
- Text-first development in Unity: gameplay is proven with text and UI before the visual scaffold, animation and execution spectacle are added. The released game is not decided to be text-based.

## 12. Open design questions

- What does one day contain, and how long does it take to play?
- Which player actions are real systems? Which resources are real?
- What makes a decision interesting rather than obvious (information, risk, trade-offs)?
- How are consequences communicated: immediately, next day, much later?
- How much information does the player have about guilt or innocence?
- What are the failure and victory conditions? Can a run be lost mid-campaign?
- How many days, prisoners and endings? How much content is hand-written?
- How does equipment condition matter beyond cosmetics?
- How are the kingdom's factions represented, if at all?
- How is the First Night-type material handled so the humour hits the institution, not the victims?
- Visual style and animation approach.
- When does the visual phase begin, and how is the spectacle validated once it exists (see PrototypePlan.md)?
