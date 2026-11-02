using System;
using System.Collections.Generic;
using System.Linq;
using NotoriousPIG.Framework.Events;

namespace TheRoyalExecutioner {
    public class Run {
        private const string readFileFirstReason = "Read the file first";
        private const string auditorReason = "The Auditor is watching";
        private const string notEnoughCoinReason = "Not enough coin";
        private const string axeAlreadySharpReason = "The axe cannot get any sharper";

        private readonly CampaignData campaign;
        private readonly IEvents events;
        private readonly HashSet<StoryFlag> flags = new();
        private readonly List<PrisonerCase> queue = new();
        private readonly List<CaseRecord> dayRecords = new();
        private readonly List<CaseRecord> runRecords = new();
        private readonly List<MorningEvent> appliedMorningEvents = new();
        private int dayIndex;
        private int queueIndex;
        private int dayStartMoney;
        private int dayStartAxeCondition;
        private int dayStartScrutiny;

        public RunPhase phase { get; private set; }
        public int money { get; private set; }
        public int axeCondition { get; private set; }
        public int scrutiny { get; private set; }
        public bool examined { get; private set; }
        public bool auditorPresent { get; private set; }
        public ChoiceResult lastResult { get; private set; }
        public DaySummary summary { get; private set; }
        public RunConclusion conclusion { get; private set; }

        public string conclusionText => campaign.ConclusionTextFor(conclusion);

        public GameRules rules => campaign.rules;
        public int dayNumber => dayIndex + 1;
        public int dayCount => campaign.days.Count;
        public bool isFinalDay => dayIndex == dayCount - 1;
        public DayData currentDay => campaign.days[dayIndex];
        public PrisonerCase currentCase => queue[queueIndex];
        public int caseNumber => queueIndex + 1;
        public int caseCount => queue.Count;
        public bool isLastCaseOfDay => queueIndex == queue.Count - 1;
        public IReadOnlyList<PrisonerCase> todaysCases => queue;
        public IReadOnlyList<MorningEvent> morningEvents => appliedMorningEvents;
        public IReadOnlyList<CaseRecord> records => runRecords;
        public ExecutionQuality currentQuality => rules.QualityFor(axeCondition);
        public bool auditorExpectedTomorrow => !isFinalDay && scrutiny >= rules.auditorWatchesFromScrutiny;

        public Run(CampaignData campaign, IEvents events) {
            this.campaign = campaign;
            this.events = events;
            money = rules.startingMoney;
            axeCondition = rules.startingAxeCondition;
        }

        public void Begin() => StartDay(0);

        public void Continue() {
            switch (phase) {
                case RunPhase.DayBriefing:
                    BeginFirstCase();
                    break;
                case RunPhase.Outcome:
                    AdvanceFromOutcome();
                    break;
                case RunPhase.DaySummary:
                    AdvanceFromSummary();
                    break;
                default:
                    throw new InvalidOperationException($"Cannot continue from phase {phase}.");
            }
        }

        public void Examine() {
            RequirePhase(RunPhase.Deciding);
            examined = true;
            events.Trigger(new PrisonerExamined());
        }

        public string SharpenDisabledReason() {
            if (axeCondition >= rules.maxAxeCondition) {
                return axeAlreadySharpReason;
            }

            return money < rules.sharpenCost ? notEnoughCoinReason : null;
        }

        public void Sharpen() {
            RequirePhase(RunPhase.Deciding);
            if (SharpenDisabledReason() != null) {
                throw new InvalidOperationException("The axe cannot be sharpened now.");
            }

            money -= rules.sharpenCost;
            axeCondition = Math.Min(rules.maxAxeCondition, axeCondition + rules.sharpenRestore);
            events.Trigger(new AxeSharpened());
        }

        public IReadOnlyList<ChoiceOption> GetOptions() {
            RequirePhase(RunPhase.Deciding);
            List<ChoiceOption> options = new() { BuildOption(rules.sentenceChoice) };
            if (currentCase.hasChallenge) {
                options.Add(BuildOption(currentCase.challenge));
            }

            options.AddRange(currentCase.contextualChoices.Where(choice => choice.requirement.IsMet(flags)).Select(BuildOption));
            return options;
        }

        public void Choose(CaseChoice choice) {
            RequirePhase(RunPhase.Deciding);
            string disabledReason = DisabledReasonFor(choice);
            if (disabledReason != null) {
                throw new InvalidOperationException($"Choice '{choice.label}' is unavailable: {disabledReason}.");
            }

            ChoiceEffects effects = ComputeEffects(choice);
            ApplyEffects(effects);
            if (choice.flagToSet != StoryFlag.None) {
                flags.Add(choice.flagToSet);
            }

            CaseRecord record = new(currentCase.prisonerName, choice.label, effects);
            dayRecords.Add(record);
            runRecords.Add(record);
            lastResult = new ChoiceResult(choice, effects, BuildNarrative(choice, effects));
            phase = RunPhase.Outcome;
            events.Trigger(new ChoiceResolved(lastResult));
        }

        private void StartDay(int index) {
            dayIndex = index;
            queue.Clear();
            dayRecords.Clear();
            appliedMorningEvents.Clear();
            ApplyMorningEvents();
            auditorPresent = scrutiny >= rules.auditorWatchesFromScrutiny;
            queue.AddRange(currentDay.cases.Where(prisonerCase => prisonerCase.requirement.IsMet(flags)));
            queueIndex = 0;
            dayStartMoney = money;
            dayStartAxeCondition = axeCondition;
            dayStartScrutiny = scrutiny;
            phase = RunPhase.DayBriefing;
            events.Trigger(new DayStarted());
        }

        private void ApplyMorningEvents() {
            foreach (MorningEvent morningEvent in currentDay.morningEvents) {
                if (!morningEvent.requirement.IsMet(flags)) {
                    continue;
                }

                money = Math.Max(0, money + morningEvent.moneyChange);
                axeCondition = Math.Clamp(axeCondition + morningEvent.axeConditionChange, 0, rules.maxAxeCondition);
                scrutiny = Math.Max(0, scrutiny + morningEvent.scrutinyChange);
                appliedMorningEvents.Add(morningEvent);
            }
        }

        private void BeginFirstCase() {
            queueIndex = 0;
            if (queue.Count == 0) {
                EndDay();
                return;
            }

            PresentCase();
        }

        private void AdvanceFromOutcome() {
            queueIndex++;
            if (queueIndex < queue.Count) {
                PresentCase();
                return;
            }

            queueIndex = queue.Count - 1;
            EndDay();
        }

        private void AdvanceFromSummary() {
            if (!isFinalDay) {
                StartDay(dayIndex + 1);
                return;
            }

            conclusion = scrutiny >= rules.arrestedFromScrutiny ? RunConclusion.Arrested : RunConclusion.Retained;
            phase = RunPhase.Conclusion;
            events.Trigger(new RunEnded());
        }

        private void PresentCase() {
            examined = false;
            phase = RunPhase.Deciding;
            events.Trigger(new PrisonerPresented());
        }

        private void EndDay() {
            summary = new DaySummary(dayNumber, currentDay.title, dayRecords.ToList(), dayStartMoney, money, dayStartAxeCondition, axeCondition, dayStartScrutiny, scrutiny);
            phase = RunPhase.DaySummary;
            events.Trigger(new DayEnded());
        }

        private ChoiceOption BuildOption(CaseChoice choice) => new(choice, ComputeEffects(choice), DisabledReasonFor(choice));

        private string DisabledReasonFor(CaseChoice choice) {
            if (choice.requiresExamination && !examined) {
                return readFileFirstReason;
            }

            if (choice.isUnofficial && auditorPresent) {
                return auditorReason;
            }

            return money + Math.Min(0, choice.moneyChange) < 0 ? notEnoughCoinReason : null;
        }

        private ChoiceEffects ComputeEffects(CaseChoice choice) {
            int moneyDelta = choice.moneyChange;
            int scrutinyDelta = choice.scrutinyChange;
            int axeDelta = 0;
            ExecutionQuality quality = ExecutionQuality.Clean;

            if (choice.method == ExecutionMethod.Axe) {
                quality = currentQuality;
                moneyDelta += rules.FeeFor(currentCase.fee, quality);
                scrutinyDelta += currentCase.ScrutinyFor(quality) + rules.ExtraScrutinyFor(quality);
                axeDelta = -Math.Min(rules.wearPerAxeExecution, axeCondition);
            } else if (choice.method == ExecutionMethod.Alternative) {
                moneyDelta += currentCase.fee;
                scrutinyDelta += currentCase.scrutinyIfClean;
            }

            return new ChoiceEffects(moneyDelta, Math.Max(-scrutiny, scrutinyDelta), axeDelta, choice.method, quality);
        }

        private void ApplyEffects(ChoiceEffects effects) {
            money += effects.money;
            scrutiny += effects.scrutiny;
            axeCondition += effects.axeCondition;
        }

        private string BuildNarrative(CaseChoice choice, ChoiceEffects effects) {
            string executionText = effects.method == ExecutionMethod.Axe ? currentCase.ExecutionTextFor(effects.quality) : string.Empty;
            return string.Join("\n\n", new[] { choice.resultText, executionText }.Where(text => !string.IsNullOrEmpty(text)));
        }

        private void RequirePhase(RunPhase required) {
            if (phase != required) {
                throw new InvalidOperationException($"Expected phase {required} but run is in {phase}.");
            }
        }
    }
}
