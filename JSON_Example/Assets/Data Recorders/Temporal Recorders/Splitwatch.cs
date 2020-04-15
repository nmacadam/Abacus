using System.Collections.Generic;
using Abacus.Internal;
using UnityEngine;

namespace Abacus
{
    /// <summary>
    /// Measures splits (discrete, sequential durations).
    /// Use when you want to measure the time each event in a sequence takes to complete
    /// </summary>
    public class Splitwatch : MonoBehaviour, ITemporal
    {
        [Tooltip("What should the splitwatch be labeled as in the output?")] public string Label;

        private List<Split> _splits = new List<Split>();
        public List<Split> Splits => _splits;
        private Split _current = null;
        public bool IsRecording => (_current != null);
        public string DisplayType => "splitwatch";

        /// <summary>
        /// Toggle the current split's state
        /// </summary>
        /// <param name="name">The name of the split</param>
        public void ToggleSplit(string name)
        {
            if (_current == null)
            {
                StartSplit(name);
            }
            else
            {
                StopSplit();
            }
        }

        /// <summary>
        /// Start a new split
        /// </summary>
        /// <param name="name">The name of the split</param>
        public void StartSplit(string name)
        {
            _current = new Split(name);
            _current.StartClock();
            _splits.Add(_current);
        }

        /// <summary>
        /// Stop the active split
        /// </summary>
        public void StopSplit()
        {
            if (_current == null) return;

            _current.StopClock();
            _current = null;
        }

        /// <summary>
        /// Retrieve the splitwatch's name/label
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
            return _splits.ToArray();
        }
        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);
        }
    }
}