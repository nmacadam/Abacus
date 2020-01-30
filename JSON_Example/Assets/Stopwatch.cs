using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Measures generalized time duration.
/// Use when you want to measure the of a particular event multiple times
/// ex: duration of each combat sequence
/// </summary>
public class Stopwatch : MonoBehaviour
{
    private List<TimeDuration> _measures = new List<TimeDuration>();

    private TimeDuration _current = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_current == null)
            {
                _current = new TimeDuration();
                _current.StartClock();
                _measures.Add(_current);
                Debug.Log($"START: {_current.StartTime}");
            }
            else
            {
                _current.StopClock();
                Debug.Log($"END: {_current.EndTime}");
                Debug.Log($"DURATION: {_current.Duration}");
                _current = null;
            }
        }
    }

    private void OnDisable()
    {
        if (_measures.Count == 0) return;

        float average = 0f;
        foreach (var item in _measures)
        {
            if (item.EndTime < 0) continue;
            average += item.Duration;
        }

        average /= _measures.Count;

        Debug.Log($"AVERAGE DURATION: {average}");
    }
}
