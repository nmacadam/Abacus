using UnityEngine;
using UnityEngine.UI;

namespace Abacus.Examples
{
    public class SplitFromInputField : MonoBehaviour
    {
        public Splitwatch Splitwatch;
        public Button Button;
        public InputField InputField;

        private void Start()
        {
            Button.onClick.AddListener(() => Splitwatch.ToggleSplit(InputField.text));
        }
    }
}