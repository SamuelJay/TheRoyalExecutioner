using System.Collections.Generic;
using UnityEngine;

namespace TheRoyalExecutioner {
    [CreateAssetMenu(fileName = "Campaign", menuName = "Royal Executioner/Campaign")]
    public class CampaignData : ScriptableObject {
        [field: SerializeField] public GameRules rules { get; private set; }
        [field: SerializeField] public List<DayData> days { get; private set; } = new();
        [field: SerializeField, TextArea(3, 8)] public string arrestedText { get; private set; }
        [field: SerializeField, TextArea(3, 8)] public string retainedText { get; private set; }

        public string ConclusionTextFor(RunConclusion conclusion) => conclusion == RunConclusion.Arrested ? arrestedText : retainedText;
    }
}
