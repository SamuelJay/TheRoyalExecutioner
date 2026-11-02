namespace TheRoyalExecutioner {
    public readonly struct DayStarted { }

    public readonly struct PrisonerPresented { }

    public readonly struct PrisonerExamined { }

    public readonly struct AxeSharpened { }

    public readonly struct ChoiceResolved {
        public ChoiceResult result { get; }

        public ChoiceResolved(ChoiceResult result) {
            this.result = result;
        }
    }

    public readonly struct DayEnded { }

    public readonly struct RunEnded { }
}
