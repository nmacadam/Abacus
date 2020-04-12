using System;
using System.Collections;
using System.Collections.Generic;
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
        public string Label;

        private List<TimeDuration> _measures = new List<TimeDuration>();

        private TimeDuration _current = null;

        public string DisplayType => "stopwatch";

        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);
        }

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

        public string GetVariableName()
        {
            return Label;
        }

        public object Dump()
        {
            return _measures.ToArray();
        }
    }
}