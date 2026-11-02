using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheRoyalExecutioner {
    public class ChoiceButton : MonoBehaviour {
        [SerializeField] private Button button;
        [SerializeField] private Text labelText;
        [SerializeField] private Text detailText;

        public void Bind(string label, string detail, bool interactable, UnityAction onClick) {
            labelText.text = label;
            detailText.text = detail;
            button.interactable = interactable;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClick);
        }
    }
}
