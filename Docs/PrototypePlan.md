# The Royal Executioner: Prototype Plan

Plan for the smallest meaningful playable prototype. Everything is a candidate, not a requirement. No implementation exists yet.

## Development approach

The first playable is a **text-based management loop** built in Unity with simple text and UI controls. This is a development approach, not a decision that the released game is text-based. Art is not held back until the whole campaign is complete; the visual phase (below) begins once the text loop has shown the gameplay works.

## Hypothesis

**Can processing prisoners, making management decisions, resolving executions and experiencing consequences form an enjoyable repeatable gameplay loop?**

The first playable must show that choices matter and that consequences influence later decisions. Whether the visual execution spectacle works is a separate question, validated later (see Later phase).

## Candidate scope

One complete working day, followed by the start of another day where consequences are visible.

Priorities, in order:
- Prisoner records.
- Available actions.
- Decision resolution.
- Persistent state carried between days.
- End-of-day consequences.
- The following day's events.

Possible elements:
- A small queue of prisoners.
- A few distinct decisions.
- Basic money and equipment condition.
- Executions resolved by gameplay rules, with a textual outcome.
- Several different execution outcomes.
- End-of-day consequences.
- At least one decision that influences a later day.

Gameplay state, decision resolution and execution outcomes stay independent of presentation, so visuals can be added later. Placeholder everything: text and UI only, no final art, no audio pipeline, no save system unless play-testing demands it.

## Design questions to answer before implementing

1. What decisions does the player make per prisoner, and what information do they have when making them?
2. What are the two or three resources that matter in a day (candidates: money, equipment condition, one "danger" measure such as suspicion or favour)?
3. What are the possible outcomes of an execution, and what inputs decide between them?
4. How does equipment condition change outcome or spectacle in a way the player can see and predict?
5. Which one decision carries over into the next day, and how is that shown?
6. How many prisoners and how much time make a day feel like a day?
7. What does a "bad" day look like? Is there a failure state in the prototype?
8. What is the minimum prisoner data (name, crime, order, hidden truth, quirks) needed to make the cases feel distinct?

The next session is collaborative design work on these, specifically the management actions, rules, resources and consequences.

## Implementation milestones (provisional, small)

Text-first phase:

1. **Prisoner queue and records.** A handful of hand-written prisoners shown as text with their order, and the actions available for each, as agreed in design.
2. **Decision resolution.** Chosen actions change persistent state; executions resolve through rules and display a textual outcome (several variants chosen from simple state).
3. **Money and equipment.** Earn money, equipment wears, a way to maintain it.
4. **Day end.** Summary text showing outcomes and what carries forward.
5. **Day two.** Start the next day with at least one visible consequence of a day-one decision, affecting what the player can or must decide.
6. **Playtest pass.** Run through with the observation list below; adjust.

Use the Notorious P.I.G. framework services/bootstrap/events where they fit naturally. Do not build speculative architecture or use the character package until a concrete need appears.

## Later phase: visual presentation

Begins after the text loop is judged promising; the full campaign need not be finished first.

- Visual scaffold scene (block, axe, executioner and prisoner stand-ins, camera).
- Character animation and execution effects.
- Execution interaction (the simple input from the GDD) driving the existing rule-resolved outcomes.

**Separate validation requirement:** once built, the visual execution spectacle must be validated on its own: is it fun to watch and trigger, or skippable by the third? A promising text loop does not prove this.

## What to observe in playtests

Text-first phase:
- Do players want to start another day, unprompted?
- Do they pause to think about decisions, or click through? Are the decisions meaningfully different?
- Do they notice and remember the consequence on day two? Do they say "because I did X"?
- Does equipment maintenance feel like a decision or a chore?
- Where does the player laugh, wince, or feel uncomfortable, and is it on purpose?
- Do they try to do things the prototype does not support (the unofficial actions)? That indicates what to build next.
- How long does a day actually take?

Visual phase:
- Are the executions fun to watch and trigger, or do they become skippable by the third?

## Decision rule

If players can describe their own story of the day and want another, the loop is promising: grow content and systems. If not, change the decisions before adding scope.

Separately, the visual spectacle is judged by the visual-phase observation above, not by the text loop's result.
