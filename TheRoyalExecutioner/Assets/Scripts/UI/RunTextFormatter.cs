using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheRoyalExecutioner {
    public static class RunTextFormatter {
        private const string warningColor = "#E0A030";
        private const string badColor = "#D05050";
        private const string goodColor = "#70B870";
        private const string dimColor = "#9A9A9A";
        private const string detailSeparator = "  |  ";

        public static string Header(Run run) => run.phase == RunPhase.Conclusion
            ? "The Royal Executioner - Judgement"
            : $"Day {run.dayNumber} of {run.dayCount}  -  {run.currentDay.title}";

        public static string Resources(Run run) {
            GameRules rules = run.rules;
            string auditor = run.auditorPresent && run.phase != RunPhase.Conclusion ?$"  <color={warningColor}>[Auditor attending]</color>" : string.Empty;
            return $"Coin <b>{run.money}</b>   Axe <b>{run.axeCondition}/{rules.maxAxeCondition}</b> ({QualityName(run.currentQuality)})   " +
                $"Crown scrutiny <b>{run.scrutiny}</b> (Auditor from {rules.auditorWatchesFromScrutiny}, arrest at {rules.arrestedFromScrutiny}){auditor}";
        }

        public static string Body(Run run) => run.phase switch {
            RunPhase.DayBriefing => Briefing(run),
            RunPhase.Deciding => Deciding(run),
            RunPhase.Outcome => Outcome(run),
            RunPhase.DaySummary => Summary(run),
            _ => Conclusion(run)
        };

        public static string ContinueLabel(Run run) => run.phase switch {
            RunPhase.DayBriefing => "Open the first file",
            RunPhase.Outcome => run.isLastCaseOfDay ? "End the day" : "Next prisoner",
            RunPhase.DaySummary => run.isFinalDay ? "Await the Chancellery's verdict" : "Begin the next day",
            _ => "Start a new run"
        };

        public static string Describe(ChoiceEffects effects) {
            List<string> parts = new();
            if (effects.money != 0) {
                parts.Add($"Coin {Signed(effects.money)}");
            }

            if (effects.method == ExecutionMethod.Axe) {
                parts.Add($"Axe {Signed(effects.axeCondition)}, {QualityName(effects.quality)} execution");
            } else if (effects.method == ExecutionMethod.Alternative) {
                parts.Add("Axe untouched");
            }

            if (effects.scrutiny != 0) {
                parts.Add($"Scrutiny {Signed(effects.scrutiny)}");
            }

            return parts.Count == 0 ? "No cost" : string.Join(detailSeparator, parts);
        }

        public static string SharpenDescription(GameRules rules) => $"Coin -{rules.sharpenCost}{detailSeparator}Axe +{rules.sharpenRestore}";

        public static string QualityName(ExecutionQuality quality) => quality switch {
            ExecutionQuality.Clean => "clean",
            ExecutionQuality.Messy => "messy",
            _ => "botched"
        };

        private static string Briefing(Run run) {
            StringBuilder text = new();
            text.AppendLine(run.currentDay.briefing);
            foreach (MorningEvent morningEvent in run.morningEvents) {
                text.AppendLine().AppendLine($"<color={warningColor}>{morningEvent.text}</color>");
                text.AppendLine(Dim(DescribeMorningEvent(morningEvent)));
            }

            if (run.auditorPresent) {
                text.AppendLine().AppendLine($"<color={badColor}>{run.rules.auditorNoticeText}</color>");
            }

            text.AppendLine().AppendLine("<b>On today's list:</b>");
            foreach (PrisonerCase prisonerCase in run.todaysCases) {
                text.AppendLine($"  - {prisonerCase.prisonerName}");
            }

            return text.ToString();
        }

        private static string Deciding(Run run) {
            PrisonerCase prisonerCase = run.currentCase;
            StringBuilder text = new();
            text.AppendLine($"<b><size=34>{prisonerCase.prisonerName}</size></b>");
            text.AppendLine(Dim($"Prisoner {run.caseNumber} of {run.caseCount}"));
            text.AppendLine().AppendLine($"<b>Charge:</b> {prisonerCase.crime}");
            text.AppendLine($"<b>Order:</b> {prisonerCase.orderText}");
            text.AppendLine();
            text.AppendLine(run.examined ? $"<b>The file:</b>\n{prisonerCase.fileText}" : Dim("The file lies closed on the block. You are, as ever, the only one who reads these."));
            return text.ToString();
        }

        private static string Outcome(Run run) {
            ChoiceResult result = run.lastResult;
            StringBuilder text = new();
            text.AppendLine($"<b>{result.choice.label}</b>").AppendLine();
            text.AppendLine(result.narrative).AppendLine();
            AppendDeltaLines(text, result.effects, run);
            return text.ToString();
        }

        private static string Summary(Run run) {
            DaySummary summary = run.summary;
            StringBuilder text = new();
            text.AppendLine($"<b><size=34>End of day {summary.dayNumber}</size></b>").AppendLine();
            if (summary.records.Count == 0) {
                text.AppendLine("Nobody was processed today. The paperwork was relieved.");
            }

            foreach (CaseRecord record in summary.records) {
                text.AppendLine($"<b>{record.prisonerName}</b>: {DescribeFate(record.effects)}");
                text.AppendLine(Dim($"   {record.choiceLabel}  ({Describe(record.effects)})"));
            }

            text.AppendLine().AppendLine("<b>Ledger</b>");
            text.AppendLine($"Coin {summary.startMoney} -> {summary.endMoney}");
            text.AppendLine($"Axe condition {summary.startAxeCondition} -> {summary.endAxeCondition}");
            text.AppendLine($"Crown scrutiny {summary.startScrutiny} -> {summary.endScrutiny}");
            if (run.auditorExpectedTomorrow) {
                text.AppendLine().AppendLine($"<color={badColor}>The Exchequer has noticed you. An Auditor will attend tomorrow, and unofficial business will be off the table.</color>");
            } else if (!run.isFinalDay) {
                text.AppendLine().AppendLine("What you did today does not end at sundown. Sleep well, or at all.");
            }

            return text.ToString();
        }

        private static string Conclusion(Run run) {
            int executed = run.records.Count(record => record.effects.executed);
            int spared = run.records.Count - executed;
            StringBuilder text = new();
            text.AppendLine($"<b><size=34>{(run.conclusion == RunConclusion.Arrested ? "Arrested" : "Still employed")}</size></b>").AppendLine();
            text.AppendLine(run.conclusionText).AppendLine();
            text.AppendLine($"Executed: {executed}    Spared: {spared}");
            text.AppendLine($"Coin: {run.money}    Crown scrutiny: {run.scrutiny}");
            text.AppendLine().AppendLine(Dim("Start a new run to try different choices."));
            return text.ToString();
        }

        private static void AppendDeltaLines(StringBuilder text, ChoiceEffects effects, Run run) {
            if (effects.money != 0) {
                text.AppendLine(Delta("Coin", effects.money, run.money));
            }

            if (effects.axeCondition != 0) {
                text.AppendLine(Delta("Axe condition", effects.axeCondition, run.axeCondition));
            }

            if (effects.scrutiny != 0) {
                text.AppendLine(Delta("Crown scrutiny", effects.scrutiny, run.scrutiny, higherIsWorse: true));
            }
        }

        private static string Delta(string name, int change, int now, bool higherIsWorse = false) {
            bool good = higherIsWorse ? change < 0 : change > 0;
            return $"<color={(good ? goodColor : badColor)}>{name} {Signed(change)}</color> (now {now})";
        }

        private static string DescribeFate(ChoiceEffects effects) => effects.method switch {
            ExecutionMethod.None => "spared",
            ExecutionMethod.Axe => $"executed ({QualityName(effects.quality)})",
            _ => "executed (by other means)"
        };

        private static string DescribeMorningEvent(MorningEvent morningEvent) {
            List<string> parts = new();
            if (morningEvent.moneyChange != 0) {
                parts.Add($"Coin {Signed(morningEvent.moneyChange)}");
            }

            if (morningEvent.axeConditionChange != 0) {
                parts.Add($"Axe {Signed(morningEvent.axeConditionChange)}");
            }

            if (morningEvent.scrutinyChange != 0) {
                parts.Add($"Scrutiny {Signed(morningEvent.scrutinyChange)}");
            }

            return string.Join(detailSeparator, parts);
        }

        private static string Dim(string text) => $"<color={dimColor}>{text}</color>";

        private static string Signed(int value) => value > 0 ? $"+{value}" : value.ToString();
    }
}
