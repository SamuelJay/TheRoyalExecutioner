using System;
using UnityEngine;

namespace TheRoyalExecutioner {
    [Serializable]
    public class CaseChoice {
        [field: SerializeField] public string label { get; private set; }
        [field: SerializeField, TextArea(2, 6)] public string resultText { get; private set; }
        [field: SerializeField] public ExecutionMethod method { get; private set; }
        [field: SerializeField, Tooltip("Coin gained (bribes) or spent (costs, negative) on top of any execution fee.")] public int moneyChange { get; private set; }
        [field: SerializeField] public int scrutinyChange { get; private set; }
        [field: SerializeField] public StoryFlag flagToSet { get; private set; }
        [field: SerializeField, Tooltip("Unavailable until the player has examined the file.")] public bool requiresExamination { get; private set; }
        [field: SerializeField, Tooltip("Unavailable while an Auditor is attending.")] public bool isUnofficial { get; private set; }
        [field: SerializeField] public FlagRequirement requirement { get; private set; }
    }
}
