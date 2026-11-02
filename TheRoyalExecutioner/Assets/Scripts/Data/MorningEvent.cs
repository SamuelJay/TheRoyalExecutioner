using System;
using UnityEngine;

namespace TheRoyalExecutioner {
    [Serializable]
    public class MorningEvent {
        [field: SerializeField] public FlagRequirement requirement { get; private set; }
        [field: SerializeField, TextArea(2, 6)] public string text { get; private set; }
        [field: SerializeField] public int moneyChange { get; private set; }
        [field: SerializeField] public int axeConditionChange { get; private set; }
        [field: SerializeField] public int scrutinyChange { get; private set; }
    }
}
