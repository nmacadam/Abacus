using System.Collections.Generic;
using UnityEngine;

namespace Abacus
{
    /// <summary>
    /// Measures splits (discrete, sequential durations).
    /// Use when you want to measure the time each event in a sequence takes to complete
    /// </summary>
    public class Splitwatch : MonoBehaviour, ITemporal
    {
        public string Label;
        private List<Split> _splits = new List<Split>();

        private Split _current = null;

        public string DisplayType => "splitwatch";

        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);
        }

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

        public void StartSplit(string name)
        {
            _current = new Split(name);
            _current.StartClock();
            _splits.Add(_current);
        }

        public void StopSplit()
        {
            if (_current == null) return;

            _current.StopClock();
            _current = null;
        }

        public string GetVariableName()
        {
            return Label;
        }

        public object Dump()
        {
            return _splits.ToArray();
        }
    }
}