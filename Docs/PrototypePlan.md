# The Royal Executioner: Prototype Plan

Plan for the smallest meaningful playable prototype. Everything is a candidate, not a requirement. No implementation exists yet.

## Hypothesis

**Can processing prisoners, making management decisions, watching executions and experiencing consequences form an enjoyable repeatable gameplay loop?**

## Candidate scope

One complete working day, followed by the start of another day where consequences are visible.

Possible elements:
- A small queue of prisoners.
- A few distinct decisions.
- Basic money and equipment condition.
- One execution method (axe and block) with a simple input.
- Several different execution outcomes.
- End-of-day consequences.
- At least one decision that influences a later day.

Placeholder everything: primitives, UI text, no final art, no audio pipeline, no save system unless play-testing demands it.

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

1. **Greybox scaffold scene.** Block, axe, executioner and prisoner stand-ins, a camera. Nothing interactive.
2. **Execution button.** One input produces a basic execution, with outcome variants chosen from simple state. Uses placeholder visuals.
3. **Prisoner queue and records.** A handful of hand-written prisoners shown with their order; basic accept/decline-style decisions as agreed in design.
4. **Money and equipment.** Earn money, equipment wears, a way to maintain it.
5. **Day end.** Summary screen showing outcomes and what carries forward.
6. **Day two.** Start the next day with at least one visible consequence of a day-one decision.
7. **Playtest pass.** Run through with the observation list below; adjust.

Use the Notorious P.I.G. framework services/bootstrap/events where they fit naturally. Do not build speculative architecture or use the character package until a concrete need appears.

## What to observe in playtests

- Do players want to start another day, unprompted?
- Do they pause to think about decisions, or click through? Are the decisions meaningfully different?
- Do they notice and remember the consequence on day two? Do they say "because I did X"?
- Are the executions fun to watch and trigger, or do they become skippable by the third?
- Does equipment maintenance feel like a decision or a chore?
- Where does the player laugh, wince, or feel uncomfortable, and is it on purpose?
- Do they try to do things the prototype does not support (the unofficial actions)? That indicates what to build next.
- How long does a day actually take?

## Decision rule

If players can describe their own story of the day and want another, the loop is promising: grow content and systems. If not, change the decisions before adding scope.
