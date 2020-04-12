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
        }

        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);
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