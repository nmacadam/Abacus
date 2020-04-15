using System.Collections.Generic;
using Abacus.Internal;
using UnityEngine;

namespace Abacus
{
    /// <summary>
    /// Timestamps mark a time with a string
    /// </summary>
    public class EventTimestamper : MonoBehaviour, ITemporal
    {
        [Tooltip("What should the timestamp be labeled as in the output?")] public string Label;

        private List<EventTimestamp> _timestamps = new List<EventTimestamp>();
        public List<EventTimestamp> Timestamps => _timestamps;
        public string DisplayType => "timestamp";

        /// <summary>
        /// Create a timestamp for the current time
        /// </summary>
        /// <param name="eventName">Name of the event being stamped</param>
        public void Stamp(string eventName)
        {
            EventTimestamp t;
            t.Name = eventName;
            t.Time = Time.time;

            _timestamps.Add(t);
        }

        /// <summary>
        /// Retrieve the recorder's name/label
        /// </summary>
        /// <returns>The name of the recorder</returns>
        public string GetVariableName()
        {
            return Label;
        }

        /// <summary>
        /// Convert the recorder data to an object for serialization
        /// </summary>
        /// <returns>The recorder's data as an object</returns>
        public object Dump()
        {
            return _timestamps.ToArray();
        }

        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);
        }
    }
}