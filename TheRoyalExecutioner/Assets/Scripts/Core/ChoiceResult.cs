namespace TheRoyalExecutioner {
    public class ChoiceResult {
        public CaseChoice choice { get; }
        public ChoiceEffects effects { get; }
        public string narrative { get; }

        public ChoiceResult(CaseChoice choice, ChoiceEffects effects, string narrative) {
            this.choice = choice;
            this.effects = effects;
            this.narrative = narrative;
        }
    }
}
