using System.Collections.Generic;
using NotoriousPIG.Framework;
using NotoriousPIG.Framework.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheRoyalExecutioner {
    public class RunScreen : MonoBehaviour {
        private const string examineLabel = "Examine the file";
        private const string examineDetail = "Read the paperwork before deciding";
        private const string sharpenLabel = "Sharpen the axe";

        [SerializeField] private Text headerText;
        [SerializeField] private Text resourcesText;
        [SerializeField] private Text bodyText;
        [SerializeField] private ScrollRect bodyScroll;
        [SerializeField] private ChoiceButton choiceButtonPrefab;
        [SerializeField] private RectTransform choiceContainer;
        [SerializeField] private Button restartButton;

        private readonly List<ChoiceButton> choiceButtons = new();
        private IEvents events;
        private IRuns runs;

        private void OnEnable() {
            events = Services.Get<IEvents>();
            runs = Services.Get<IRuns>();
            events.StartListening<DayStarted>(OnRunEvent);
            events.StartListening<PrisonerPresented>(OnRunEvent);
            events.StartListening<PrisonerExamined>(OnRunEvent);
            events.StartListening<AxeSharpened>(OnRunEvent);
            events.StartListening<ChoiceResolved>(OnRunEvent);
            events.StartListening<DayEnded>(OnRunEvent);
            events.StartListening<RunEnded>(OnRunEvent);
            restartButton.onClick.AddListener(runs.StartNewRun);
        }

        private void Start() => runs.StartNewRun();

        private void OnDisable() {
            restartButton.onClick.RemoveListener(runs.StartNewRun);
            if (!Services.TryGet(out IEvents eventService)) {
                return;
            }

            eventService.StopListening<DayStarted>(OnRunEvent);
            eventService.StopListening<PrisonerPresented>(OnRunEvent);
            eventService.StopListening<PrisonerExamined>(OnRunEvent);
            eventService.StopListening<AxeSharpened>(OnRunEvent);
            eventService.StopListening<ChoiceResolved>(OnRunEvent);
            eventService.StopListening<DayEnded>(OnRunEvent);
            eventService.StopListening<RunEnded>(OnRunEvent);
        }

        private void OnRunEvent<TEvent>(TEvent runEvent) => Refresh();

        private void Refresh() {
            Run run = runs.current;
            headerText.text = RunTextFormatter.Header(run);
            resourcesText.text = RunTextFormatter.Resources(run);
            bodyText.text = RunTextFormatter.Body(run);
            Canvas.ForceUpdateCanvases();
            bodyScroll.verticalNormalizedPosition = 1f;
            ShowChoices(run);
        }

        private void ShowChoices(Run run) {
            int used = 0;
            if (run.phase == RunPhase.Deciding) {
                used = ShowDecisionButtons(run);
            } else {
                BindButton(used++, RunTextFormatter.ContinueLabel(run), string.Empty, true, ContinueOrRestart);
            }

            for (int index = used; index < choiceButtons.Count; index++) {
                choiceButtons[index].gameObject.SetActive(false);
            }
        }

        private int ShowDecisionButtons(Run run) {
            int used = 0;
            if (!run.examined) {
                BindButton(used++, examineLabel, examineDetail, true, run.Examine);
            }

            string sharpenReason = run.SharpenDisabledReason();
            BindButton(used++, sharpenLabel, sharpenReason ?? RunTextFormatter.SharpenDescription(run.rules), sharpenReason == null, run.Sharpen);
            foreach (ChoiceOption option in run.GetOptions()) {
                CaseChoice choice = option.choice;
                string label = choice.isUnofficial ? $"[Unofficial] {choice.label}" : choice.label;
                string detail = option.enabled ? RunTextFormatter.Describe(option.effects) : option.disabledReason;
                BindButton(used++, label, detail, option.enabled, () => run.Choose(choice));
            }

            return used;
        }

        private void ContinueOrRestart() {
            if (runs.current.phase == RunPhase.Conclusion) {
                runs.StartNewRun();
                return;
            }

            runs.current.Continue();
        }

        private void BindButton(int index, string label, string detail, bool interactable, UnityAction onClick) {
            while (choiceButtons.Count <= index) {
                choiceButtons.Add(Instantiate(choiceButtonPrefab, choiceContainer));
            }

            ChoiceButton choiceButton = choiceButtons[index];
            choiceButton.gameObject.SetActive(true);
            choiceButton.Bind(label, detail, interactable, onClick);
        }
    }
}
