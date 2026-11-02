namespace TheRoyalExecutioner {
    public readonly struct CaseRecord {
        public string prisonerName { get; }
        public string choiceLabel { get; }
        public ChoiceEffects effects { get; }

        public CaseRecord(string prisonerName, string choiceLabel, ChoiceEffects effects) {
            this.prisonerName = prisonerName;
            this.choiceLabel = choiceLabel;
            this.effects = effects;
        }
    }
}
