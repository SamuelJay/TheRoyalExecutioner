using System.Collections.Generic;

namespace TheRoyalExecutioner {
    public class DaySummary {
        public int dayNumber { get; }
        public string title { get; }
        public IReadOnlyList<CaseRecord> records { get; }
        public int startMoney { get; }
        public int endMoney { get; }
        public int startAxeCondition { get; }
        public int endAxeCondition { get; }
        public int startScrutiny { get; }
        public int endScrutiny { get; }

        public DaySummary(int dayNumber, string title, IReadOnlyList<CaseRecord> records, int startMoney, int endMoney, int startAxeCondition, int endAxeCondition, int startScrutiny, int endScrutiny) {
            this.dayNumber = dayNumber;
            this.title = title;
            this.records = records;
            this.startMoney = startMoney;
            this.endMoney = endMoney;
            this.startAxeCondition = startAxeCondition;
            this.endAxeCondition = endAxeCondition;
            this.startScrutiny = startScrutiny;
            this.endScrutiny = endScrutiny;
        }
    }
}
