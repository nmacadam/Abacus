using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Abacus : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool __shuttingDown = false;
    private static bool _shuttingDown
    {
        get
        {
            return __shuttingDown;
        }
        set
        {
            if (value && !__shuttingDown)
            {
                _instance.Dump();
            }

            __shuttingDown = value;
        }
    }
    private static object _lock = new object();
    private static Abacus _instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static Abacus Instance
    {
        get
        {
            if (__shuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(Abacus) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    //_instance = new Abacus();
                    // Search for existing instance.
                    _instance = (Abacus)FindObjectOfType(typeof(Abacus));

                    // Create new instance if one doesn't already exist.
                    if (_instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<Abacus>();
                        singletonObject.name = typeof(Abacus).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }
    }

    private void OnApplicationQuit()
    {
        _shuttingDown = true;
    }
    private void OnDestroy()
    {
        _shuttingDown = true;
    }

    public void Dump()
    {
        //using (var sw = new StreamWriter("./data.json"))
        //{
        //    foreach (var record in recordables)
        //    {
        //        sw.Write(ConstructJSONMetric(record.GetValueType(), JSONWrapArray(record.Dump())));
        //    }
        //}

        foreach (var record in recordables)
        {
            Debug.Log(ConstructJSONMetric(record.GetValueType(), JSONWrapArray(record.Dump())));
        }
    }

    private string ConstructJSONMetric(Type valueType, string data)
    {
        string output = $"{{\n\"type\":\"{valueType.Name},\"\n\"data\":\n{data}\n}}";
        return output;
    }

    private string JSONWrapArray(string[] values)
    {
        string output = "[\n";
        for (int i = 0; i < values.Length; i++)
        {
            output += values[i];
            if (i != values.Length - 1)
            {
                output += ",";
            }
            output += "\n";
        }
        output += "]";

        return output;
    }

    public void AddRecord(IRecordable value)
    {
        Debug.Log("Adding to recordables");
        recordables.Add(value);
    }

    private List<IRecordable> recordables = new List<IRecordable>();
}
