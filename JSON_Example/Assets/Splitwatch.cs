using System.Collections.Generic;
using UnityEngine;

public class TimeDuration
{
    public float StartTime;
    public float EndTime;
    public float Duration
    {
        get
        {
            if (EndTime < 0) return Time.time - StartTime;
            return EndTime - StartTime;
        }
    }

    public void StartClock()
    {
        StartTime = Time.time;
        EndTime = -1f;
    }

    public void StopClock()
    {
        EndTime = Time.time;
    }
}

/// <summary>
/// Splits are for measuring the duration of discrete, sequential events
/// </summary>
public class Split : TimeDuration
{
    public string Name;

    public Split(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Measures splits (discrete, sequential durations).
/// Use when you want to measure the time each event in a sequence takes to complete
/// </summary>
public class Splitwatch : MonoBehaviour
{
    private List<Split> _splits = new List<Split>();

    private Split _current = null;

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
}
