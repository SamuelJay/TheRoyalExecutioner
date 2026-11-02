using NotoriousPIG.Framework;
using NotoriousPIG.Framework.Events;
using UnityEngine;

namespace TheRoyalExecutioner {
    public class RunManager : MonoBehaviour, IRuns {
        [SerializeField] private CampaignData campaign;

        public Run current { get; private set; }

        private void OnDestroy() => Services.UnregisterApp<IRuns>(this);

        public void StartNewRun() {
            current = new Run(campaign, Services.Get<IEvents>());
            current.Begin();
        }
    }
}
