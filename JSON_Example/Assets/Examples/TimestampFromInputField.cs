using UnityEngine;
using UnityEngine.UI;

namespace Abacus.Examples
{
    public class TimestampFromInputField : MonoBehaviour
    {
        public EventTimestamper Stamp;
        public Button Button;
        public InputField InputField;

        private void Start()
        {
            Button.onClick.AddListener(() => Stamp.Stamp(InputField.text));
        }
    }
}