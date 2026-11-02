using UnityEngine;

namespace TheRoyalExecutioner {
    [CreateAssetMenu(fileName = "GameRules", menuName = "Royal Executioner/Game Rules")]
    public class GameRules : ScriptableObject {
        private const int percentDivisor = 100;

        [field: SerializeField] public int startingMoney { get; private set; }
        [field: SerializeField] public int startingAxeCondition { get; private set; }
        [field: SerializeField] public int maxAxeCondition { get; private set; }
        [field: SerializeField, Tooltip("Axe condition at or above this gives a clean execution.")] public int cleanMinCondition { get; private set; }
        [field: SerializeField, Tooltip("Axe condition at or above this (but below clean) gives a messy execution; below it, a botched one.")] public int messyMinCondition { get; private set; }
        [field: SerializeField] public int wearPerAxeExecution { get; private set; }
        [field: SerializeField] public int messyFeePercent { get; private set; }
        [field: SerializeField] public int botchedFeePercent { get; private set; }
        [field: SerializeField] public int botchedExtraScrutiny { get; private set; }
        [field: SerializeField] public int sharpenCost { get; private set; }
        [field: SerializeField] public int sharpenRestore { get; private set; }
        [field: SerializeField, Tooltip("At the start of a day, an Auditor attends if scrutiny is at or above this.")] public int auditorWatchesFromScrutiny { get; private set; }
        [field: SerializeField, Tooltip("If scrutiny is at or above this when the last day ends, the executioner is arrested.")] public int arrestedFromScrutiny { get; private set; }
        [field: SerializeField, TextArea(2, 4)] public string auditorNoticeText { get; private set; }
        [field: SerializeField] public CaseChoice sentenceChoice { get; private set; }

        public ExecutionQuality QualityFor(int axeCondition) {
            if (axeCondition >= cleanMinCondition) {
                return ExecutionQuality.Clean;
            }

            return axeCondition >= messyMinCondition ? ExecutionQuality.Messy : ExecutionQuality.Botched;
        }

        public int FeeFor(int baseFee, ExecutionQuality quality) => quality switch {
            ExecutionQuality.Clean => baseFee,
            ExecutionQuality.Messy => baseFee * messyFeePercent / percentDivisor,
            _ => baseFee * botchedFeePercent / percentDivisor
        };

        public int ExtraScrutinyFor(ExecutionQuality quality) => quality == ExecutionQuality.Botched ? botchedExtraScrutiny : 0;
    }
}
