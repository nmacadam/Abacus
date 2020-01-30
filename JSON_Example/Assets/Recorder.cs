using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


// could be an interface      
public abstract class RecordableValue : MonoBehaviour
{
    public abstract Type GetValueType();
    public abstract void Dump();
}

//[System.Serializable]
public class RecordableValue<T> : RecordableValue
{
    [SerializeField] private T _value;
    public T Value
    {
        get { return _value; }
        set
        {
            //if (_value == value) return;
            _value = value;
            OnVariableChange?.Invoke(_value);
        }
    }

    public List<T> History { get; } = new List<T>();

    private delegate void OnVariableChangeDelegate(T newValue);
    private event OnVariableChangeDelegate OnVariableChange;

    //public RecordableValue()
    //{
    //    OnVariableChange += HandleChange;
    //    Abacus.Instance.AddRecord(this);
    //}
    private void Awake()
    {
        OnVariableChange += HandleChange;
        Abacus.Instance.AddRecord(this);
    }

    private void HandleChange(T newValue)
    {
        History.Add(newValue);
    }

    public sealed override Type GetValueType()
    {
        return typeof(T);
    }

    public override void Dump()
    {
        string test = "";
        foreach (var item in History)
        {
            test += item + ", ";
        }
        Debug.Log("Values: " + test);
    }
}

//[System.Serializable]

//public class Abacus
//{
//    public static Abacus Instance = new Abacus();

//    public void AddValue(RecordableValue value)
//    {
//        _recordings.Add(value);
//        Debug.Log("Added value set of type " + value.GetValueType());
//    }

//    private List<RecordableValue> _recordings = new List<RecordableValue>();
//}

public class Recorder : MonoBehaviour
{
    public RecordableVector3 someInteger;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            someInteger.Value++;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Abacus.Instance.Dump();
        }
    }

}
