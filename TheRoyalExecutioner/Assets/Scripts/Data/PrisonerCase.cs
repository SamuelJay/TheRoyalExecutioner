using System.Collections.Generic;
using UnityEngine;

namespace TheRoyalExecutioner {
    [CreateAssetMenu(fileName = "PrisonerCase", menuName = "Royal Executioner/Prisoner Case")]
    public class PrisonerCase : ScriptableObject {
        [field: SerializeField] public string prisonerName { get; private set; }
        [field: SerializeField] public string crime { get; private set; }
        [field: SerializeField, TextArea(2, 6)] public string orderText { get; private set; }
        [field: SerializeField, TextArea(4, 12)] public string fileText { get; private set; }
        [field: SerializeField] public int fee { get; private set; }
        [field: SerializeField] public FlagRequirement requirement { get; private set; }
        [field: SerializeField, Tooltip("Scrutiny change when the axe execution is clean (or done by an alternative method).")] public int scrutinyIfClean { get; private set; }
        [field: SerializeField, Tooltip("Scrutiny change when the axe execution is messy or botched.")] public int scrutinyIfNotClean { get; private set; }
        [field: SerializeField, TextArea(2, 6)] public string cleanExecutionText { get; private set; }
        [field: SerializeField, TextArea(2, 6)] public string messyExecutionText { get; private set; }
        [field: SerializeField, TextArea(2, 6)] public string botchedExecutionText { get; private set; }
        [field: SerializeField] public bool hasChallenge { get; private set; }
        [field: SerializeField] public CaseChoice challenge { get; private set; }
        [field: SerializeField] public List<CaseChoice> contextualChoices { get; private set; } = new();

        public string ExecutionTextFor(ExecutionQuality quality) => quality switch {
            ExecutionQuality.Clean => cleanExecutionText,
            ExecutionQuality.Messy => messyExecutionText,
            _ => botchedExecutionText
        };

        public int ScrutinyFor(ExecutionQuality quality) => quality == ExecutionQuality.Clean ? scrutinyIfClean : scrutinyIfNotClean;
    }
}
