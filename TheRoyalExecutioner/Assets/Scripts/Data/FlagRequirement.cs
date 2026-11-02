using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheRoyalExecutioner {
    [Serializable]
    public class FlagRequirement {
        [SerializeField] private StoryFlag required;
        [SerializeField] private StoryFlag forbidden;

        public bool IsMet(HashSet<StoryFlag> flags) =>
            (required == StoryFlag.None || flags.Contains(required)) &&
            (forbidden == StoryFlag.None || !flags.Contains(forbidden));
    }
}
