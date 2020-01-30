using System;
using System.Collections;
using System.Collections.Generic;
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
            __shuttingDown = value;

            if (value)
            {
                _instance.Dump();
            }
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
        //foreach (var record in _recordings)
        //{
        //    record.Dump();
        //}
    }

    //public void AddRecord(RecordableValue value)
    //{
    //    _recordings.Add(value);
    //}

    //private List<RecordableValue> _recordings = new List<RecordableValue>();
    //private List<RecordableValue> _recordings = new List<RecordableValue>();
}
