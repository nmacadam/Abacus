using System.Collections.Generic;
using Abacus.Internal;
using UnityEngine;

namespace Abacus
{
    /// <summary>
    /// Measures generalized time duration.
    /// Use when you want to measure the of a particular event multiple times
    /// ex: duration of each combat sequence
    /// </summary>
    public class Stopwatch : MonoBehaviour, ITemporal
    {
        [Tooltip("What should the stopwatch be labeled as in the output?")] public string Label;

        public List<TimeDuration> Measures => _measures;
        private List<TimeDuration> _measures = new List<TimeDuration>();
        private TimeDuration _current = null;
        public bool IsRecording => (_current != null);
        public string DisplayType => "stopwatch";

        /// <summary>
        /// Toggle the state of the stopwatch
        /// </summary>
        public void Toggle()
        {
            if (_current == null)
            {
                _current = new TimeDuration();
                _current.StartClock();
                _measures.Add(_current);
            }
            else
            {
                _current.StopClock();
                _current = null;
            }
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
            return _measures.ToArray();
        }

        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);
        }
    }
}