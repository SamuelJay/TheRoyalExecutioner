namespace TheRoyalExecutioner {
    public readonly struct ChoiceEffects {
        public int money { get; }
        public int scrutiny { get; }
        public int axeCondition { get; }
        public ExecutionMethod method { get; }
        public ExecutionQuality quality { get; }

        public ChoiceEffects(int money, int scrutiny, int axeCondition, ExecutionMethod method, ExecutionQuality quality) {
            this.money = money;
            this.scrutiny = scrutiny;
            this.axeCondition = axeCondition;
            this.method = method;
            this.quality = quality;
        }

        public bool executed => method != ExecutionMethod.None;
    }
}
