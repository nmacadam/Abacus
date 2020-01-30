using System.Collections.Generic;
using UnityEngine;

public struct EventTimestamp
{
    public float Time;
    public string Name;
}

public class EventTimestamper : MonoBehaviour
{
    private List<EventTimestamp> _timestamps = new List<EventTimestamp>();
    
    public void Stamp(string eventName)
    {
        EventTimestamp t;
        t.Name = eventName;
        t.Time = Time.time;

        _timestamps.Add(t);
        Debug.Log("Stamped event " + t.Name + " for time " + t.Time);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Stamp("someName");
        }
    }
}
