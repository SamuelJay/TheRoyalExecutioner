using System;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace TheRoyalExecutioner.Tests {
    public class RunTests {
        private const string campaignPath = "Assets/Content/Campaign/Campaign.asset";
        private const string sentenceLabel = "Carry out the sentence";
        private const string appealLabel = "Appeal";
        private const string cushionLabel = "Cushion";
        private const string challengeLabel = "Challenge the warrant";
        private const string envelopeLabel = "envelope";
        private const string standingOfferLabel = "standing offer";
        private const string ruleSevenLabel = "Rule 7";
        private const string startingMoneyProperty = "<startingMoney>k__BackingField";
        private const string rulesProperty = "<rules>k__BackingField";

        private CampaignData campaign;
        private RecordingEvents events;
        private UnityEngine.Object[] createdObjects = Array.Empty<UnityEngine.Object>();

        [SetUp]
        public void SetUp() {
            campaign = AssetDatabase.LoadAssetAtPath<CampaignData>(campaignPath);
            events = new RecordingEvents();
            Assert.IsNotNull(campaign, "Campaign asset is missing.");
        }

        [TearDown]
        public void TearDown() {
            foreach (UnityEngine.Object created in createdObjects) {
                UnityEngine.Object.DestroyImmediate(created);
            }

            createdObjects = Array.Empty<UnityEngine.Object>();
        }

        [Test]
        public void NewRun_StartsDayOneWithStartingResources() {
            Run run = NewRun();

            Assert.AreEqual(RunPhase.DayBriefing, run.phase);
            Assert.AreEqual(1, run.dayNumber);
            Assert.AreEqual(campaign.rules.startingMoney, run.money);
            Assert.AreEqual(campaign.rules.startingAxeCondition, run.axeCondition);
            Assert.AreEqual(0, run.scrutiny);
            Assert.IsFalse(run.auditorPresent);
            Assert.AreEqual(3, run.todaysCases.Count);
        }

        [Test]
        public void Examine_IsRequiredBeforeFileDependentChoices() {
            Run run = AtCase(NewRun(), 2);

            Assert.IsFalse(OptionFor(run, challengeLabel).enabled);
            run.Examine();
            Assert.IsTrue(OptionFor(run, challengeLabel).enabled);
        }

        [Test]
        public void CleanExecution_PaysFullFeeAndWearsAxe() {
            Run run = AtCase(NewRun(), 0);

            Sentence(run);

            Assert.AreEqual(RunPhase.Outcome, run.phase);
            Assert.AreEqual(ExecutionQuality.Clean, run.lastResult.effects.quality);
            Assert.AreEqual(campaign.rules.startingMoney + run.currentCase.fee, run.money);
            Assert.AreEqual(campaign.rules.startingAxeCondition - campaign.rules.wearPerAxeExecution, run.axeCondition);
            Assert.AreEqual(run.currentCase.cleanExecutionText, run.lastResult.narrative);
        }

        [Test]
        public void WornAxe_GivesMessyExecutionWithReducedFeeAndScrutiny() {
            Run run = AtCase(NewRun(), 1);
            int moneyBefore = run.money;

            Sentence(run);

            int expectedFee = campaign.rules.FeeFor(run.currentCase.fee, ExecutionQuality.Messy);
            Assert.AreEqual(ExecutionQuality.Messy, run.lastResult.effects.quality);
            Assert.AreEqual(moneyBefore + expectedFee, run.money);
            Assert.Less(expectedFee, run.currentCase.fee);
            Assert.AreEqual(1, run.scrutiny);
        }

        [Test]
        public void Sharpening_CostsCoinAndTurnsMessyIntoClean() {
            Run run = AtCase(NewRun(), 1);
            int moneyBefore = run.money;

            run.Sharpen();
            Sentence(run);

            Assert.AreEqual(ExecutionQuality.Clean, run.lastResult.effects.quality);
            Assert.AreEqual(moneyBefore - campaign.rules.sharpenCost + run.currentCase.fee, run.money);
            Assert.AreEqual(0, run.scrutiny);
        }

        [Test]
        public void Sharpening_IsRefusedWhenAxeIsAlreadyAtMaximum() {
            Run run = AtCase(NewRun(), 0);
            run.Sharpen();

            Assert.AreEqual(campaign.rules.maxAxeCondition, run.axeCondition);
            Assert.IsNotNull(run.SharpenDisabledReason());
            Assert.Throws<InvalidOperationException>(run.Sharpen);
        }

        [Test]
        public void PennilessExecutioner_CannotSharpenOrPayForAppeal() {
            Run run = AtCase(NewRunWithStartingMoney(0), 0);

            Assert.IsNotNull(run.SharpenDisabledReason());
            Assert.IsFalse(OptionFor(run, appealLabel).enabled);
            Assert.IsTrue(OptionFor(run, sentenceLabel).enabled);
        }

        [Test]
        public void BribingTheReeve_ChangesDayTwoCasesAndPaysOutNextMorning() {
            Run run = PlayDayOneBribingReeve(sharpenFirst: true);
            int moneyAtSummary = run.money;

            run.Continue();

            Assert.AreEqual(2, run.dayNumber);
            CollectionAssert.AreEqual(new[] { "Wenna Lowe", "Pip Mumbles, Court Jester" }, run.todaysCases.Select(item => item.prisonerName).ToArray());
            Assert.AreEqual(1, run.morningEvents.Count);
            Assert.AreEqual(moneyAtSummary + run.morningEvents[0].moneyChange, run.money);
        }

        [Test]
        public void ChallengingTheWarrant_ChangesDayTwoCasesAndRepairsTheAxe() {
            Run run = PlayDayOneChallengingWarrant();
            int axeBeforeMorning = run.axeCondition;

            run.Continue();

            CollectionAssert.AreEqual(new[] { "Tax Reeve Haldane", "Pip Mumbles, Court Jester" }, run.todaysCases.Select(item => item.prisonerName).ToArray());
            Assert.AreEqual(1, run.morningEvents.Count);
            Assert.AreEqual(axeBeforeMorning + run.morningEvents[0].axeConditionChange, run.axeCondition);
        }

        [Test]
        public void AuditorAttendingAfterHighScrutiny_LocksUnofficialChoices() {
            Run run = PlayDayOneBribingReeve(sharpenFirst: false);
            run.Continue();
            run.Continue();

            Assert.IsTrue(run.auditorPresent);
            Assert.GreaterOrEqual(run.scrutiny, campaign.rules.auditorWatchesFromScrutiny);
            Assert.AreEqual("Wenna Lowe", run.currentCase.prisonerName);
            Assert.IsFalse(OptionFor(run, standingOfferLabel).enabled);
            Assert.IsTrue(OptionFor(run, sentenceLabel).enabled);
        }

        [Test]
        public void SameBribeWithAWellKeptAxe_LeavesUnofficialChoicesOpenOnDayTwo() {
            Run run = PlayDayOneBribingReeve(sharpenFirst: true);
            run.Continue();
            run.Continue();

            Assert.IsFalse(run.auditorPresent);
            Assert.IsTrue(OptionFor(run, standingOfferLabel).enabled);
        }

        [Test]
        public void IgnoringTheAxe_EndsInArrest() {
            Run run = AtCase(NewRun(), 0);

            PlayThroughByTheBook(run, maintainAxe: false);

            Assert.AreEqual(RunConclusion.Arrested, run.conclusion);
            Assert.IsNotEmpty(run.conclusionText);
        }

        [Test]
        public void MaintainingTheAxe_EndsWithEmploymentRetained() {
            Run run = AtCase(NewRun(), 0);

            PlayThroughByTheBook(run, maintainAxe: true);

            Assert.AreEqual(RunConclusion.Retained, run.conclusion);
            Assert.AreEqual(5, run.records.Count);
        }

        [Test]
        public void NewRun_AfterAFinishedRun_StartsFromScratch() {
            Run finished = AtCase(NewRun(), 0);
            PlayThroughByTheBook(finished, maintainAxe: false);

            Run fresh = NewRun();

            Assert.AreEqual(RunPhase.DayBriefing, fresh.phase);
            Assert.AreEqual(1, fresh.dayNumber);
            Assert.AreEqual(campaign.rules.startingMoney, fresh.money);
            Assert.AreEqual(campaign.rules.startingAxeCondition, fresh.axeCondition);
            Assert.AreEqual(0, fresh.scrutiny);
            Assert.AreEqual(0, fresh.records.Count);
        }

        [Test]
        public void RoyalPleasure_LowersScrutinyOnCleanExecution() {
            Run run = AtPip(PlayDayOneChallengingWarrant());
            run.Sharpen();
            int scrutinyBefore = run.scrutiny;

            Sentence(run);

            Assert.AreEqual(ExecutionQuality.Clean, run.lastResult.effects.quality);
            Assert.AreEqual(scrutinyBefore - 1, run.scrutiny);
        }

        [Test]
        public void ChoiceThatSparesPrisoner_LeavesAxeUntouched() {
            Run run = AtPip(PlayDayOneChallengingWarrant());
            run.Examine();
            int axeBefore = run.axeCondition;

            run.Choose(OptionFor(run, ruleSevenLabel).choice);

            Assert.IsFalse(run.lastResult.effects.executed);
            Assert.AreEqual(axeBefore, run.axeCondition);
        }

        [Test]
        public void Run_TriggersEventsForEachStepOfPlay() {
            Run run = AtCase(NewRun(), 0);
            run.Examine();
            run.Sharpen();

            PlayThroughByTheBook(run, maintainAxe: false);

            CollectionAssert.IsSubsetOf(
                new[] { typeof(DayStarted), typeof(PrisonerPresented), typeof(PrisonerExamined), typeof(AxeSharpened), typeof(ChoiceResolved), typeof(DayEnded), typeof(RunEnded) },
                events.triggered);
            Assert.IsNotNull(events.lastResolvedResult);
        }

        [Test]
        public void Content_EveryCaseAndChoiceIsFullyAuthored() {
            Assert.IsNotNull(campaign.rules);
            Assert.IsNotEmpty(campaign.rules.sentenceChoice.label);
            foreach (DayData day in campaign.days) {
                Assert.IsNotEmpty(day.cases);
                foreach (PrisonerCase prisonerCase in day.cases) {
                    Assert.IsNotNull(prisonerCase);
                    Assert.IsNotEmpty(prisonerCase.prisonerName);
                    Assert.IsNotEmpty(prisonerCase.fileText);
                    Assert.IsNotEmpty(prisonerCase.cleanExecutionText);
                    Assert.IsNotEmpty(prisonerCase.messyExecutionText);
                    Assert.IsNotEmpty(prisonerCase.botchedExecutionText);
                    Assert.Greater(prisonerCase.fee, 0);
                    Assert.IsTrue(prisonerCase.contextualChoices.All(choice => !string.IsNullOrEmpty(choice.label)));
                    Assert.IsTrue(!prisonerCase.hasChallenge || !string.IsNullOrEmpty(prisonerCase.challenge.label));
                }
            }
        }

        private Run NewRun() => StartRun(campaign);

        private Run StartRun(CampaignData source) {
            Run run = new(source, events);
            run.Begin();
            return run;
        }

        private Run NewRunWithStartingMoney(int startingMoney) {
            CampaignData campaignCopy = UnityEngine.Object.Instantiate(campaign);
            GameRules rulesCopy = UnityEngine.Object.Instantiate(campaign.rules);
            createdObjects = new UnityEngine.Object[] { campaignCopy, rulesCopy };

            SerializedObject rulesObject = new(rulesCopy);
            rulesObject.FindProperty(startingMoneyProperty).intValue = startingMoney;
            rulesObject.ApplyModifiedPropertiesWithoutUndo();

            SerializedObject campaignObject = new(campaignCopy);
            campaignObject.FindProperty(rulesProperty).objectReferenceValue = rulesCopy;
            campaignObject.ApplyModifiedPropertiesWithoutUndo();
            return StartRun(campaignCopy);
        }

        private static Run AtCase(Run run, int caseIndex) {
            run.Continue();
            for (int index = 0; index < caseIndex; index++) {
                Sentence(run);
                run.Continue();
            }

            return run;
        }

        private static void Sentence(Run run) => run.Choose(OptionFor(run, sentenceLabel).choice);

        private static ChoiceOption OptionFor(Run run, string labelFragment) =>
            run.GetOptions().Single(option => option.choice.label.Contains(labelFragment));

        private static void PlayThroughByTheBook(Run run, bool maintainAxe) {
            while (run.phase != RunPhase.Conclusion) {
                if (run.phase != RunPhase.Deciding) {
                    run.Continue();
                    continue;
                }

                if (maintainAxe && run.currentQuality != ExecutionQuality.Clean) {
                    run.Sharpen();
                }

                Sentence(run);
            }
        }

        private Run PlayDayOneBribingReeve(bool sharpenFirst) {
            Run run = AtCase(NewRun(), 1);
            if (sharpenFirst) {
                run.Sharpen();
            }

            Sentence(run);
            run.Continue();
            run.Choose(OptionFor(run, envelopeLabel).choice);
            run.Continue();
            return run;
        }

        private Run PlayDayOneChallengingWarrant() {
            Run run = AtCase(NewRun(), 1);
            run.Examine();
            run.Choose(OptionFor(run, cushionLabel).choice);
            run.Continue();
            run.Examine();
            run.Choose(OptionFor(run, challengeLabel).choice);
            run.Continue();
            return run;
        }

        private static Run AtPip(Run run) {
            run.Continue();
            run.Continue();
            Sentence(run);
            run.Continue();
            return run;
        }
    }
}
