namespace TheRoyalExecutioner {
    public class ChoiceOption {
        public CaseChoice choice { get; }
        public ChoiceEffects effects { get; }
        public string disabledReason { get; }

        public ChoiceOption(CaseChoice choice, ChoiceEffects effects, string disabledReason) {
            this.choice = choice;
            this.effects = effects;
            this.disabledReason = disabledReason;
        }

        public bool enabled => disabledReason == null;
    }
}
