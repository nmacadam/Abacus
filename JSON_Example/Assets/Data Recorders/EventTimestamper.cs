using System.Collections.Generic;
using UnityEngine;

namespace Abacus
{
    public class EventTimestamper : MonoBehaviour, ITemporal
    {
        public string Label;
        private List<EventTimestamp> _timestamps = new List<EventTimestamp>();

        public string DisplayType => "timestamp";

        public void Stamp(string eventName)
        {
            EventTimestamp t;
            t.Name = eventName;
            t.Time = Time.time;

            _timestamps.Add(t);
            Debug.Log("Stamped event " + t.Name + " for time " + t.Time);
        }

        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Stamp("someName");
            }
        }

        public string GetVariableName()
        {
            return Label;
        }

        public object Dump()
        {
            return _timestamps.ToArray();
        }
    }
}