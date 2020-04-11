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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_current == null)
                {
                    _current = new Split("Split");
                    _current.StartClock();
                    _splits.Add(_current);
                    Debug.Log($"{_current.Name} START: {_current.StartTime}");
                }
                else
                {
                    _current.StopClock();
                    Debug.Log($"{_current.Name} END: {_current.EndTime}");
                    Debug.Log($"{_current.Name} DURATION: {_current.Duration}");
                    _current = null;
                }
            }
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