using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheRoyalExecutioner {
    [Serializable]
    public class DayData {
        [field: SerializeField] public string title { get; private set; }
        [field: SerializeField, TextArea(3, 8)] public string briefing { get; private set; }
        [field: SerializeField] public List<MorningEvent> morningEvents { get; private set; } = new();
        [field: SerializeField] public List<PrisonerCase> cases { get; private set; } = new();
    }
}
