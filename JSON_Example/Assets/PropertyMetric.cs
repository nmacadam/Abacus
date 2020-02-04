using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public struct DataPoint<T>
{
    public T Value;
    public float Time;

    public DataPoint(T value, float time)
    {
        Value = value;
        Time = time;
    }
}

public interface IRecordable
{
    Type GetValueType();
    string[] Dump();
    void Record();
    void SetSource(Component component);

    float TimeStep { get; }
}

public interface IMetric<T> : IRecordable
{
    T GetValue();
    T GetLastRecordedValue();
    List<DataPoint<T>> History { get; }
}

public abstract class PropertyMetric<T> : MonoBehaviour, IMetric<T>
{
    [SerializeField] private float _timeStep = 1f;
    public float TimeStep => _timeStep;

    public List<DataPoint<T>> History { get; } = new List<DataPoint<T>>();

    [SerializeField] private Component recordFrom;
    private Type componentType;

    [SerializeField] private string recordName;

    private Delegate _read;

    private T _previousValue;

    private float startTime;

    private T _lastRecordedValue;

    protected virtual bool AreEqual(T a, T b)
    {
        if (typeof(IEquatable<>).IsAssignableFrom(typeof(T)))
        {
            return a.Equals(b);
        }
        
        //Debug.LogError($"Type '{typeof(T)}' does not implement IEquatable or override AreEqual method");
        return a.Equals(b);
    }

    public T GetValue()
    {
        return (T)_read.DynamicInvoke(recordFrom);
    }

    public Type GetValueType()
    {
        return typeof(T);
    }

    public string[] Dump()
    {
        Debug.Log("Dumping property " + recordName);
        List<string> output = new List<string>();
        foreach (var value in History)
        {
            output.Add(JsonUtility.ToJson(value));
        }
       
        return output.ToArray();
    }

    private static Delegate CreateDelegate(Type componentType, MethodInfo method)
    {
        return method.CreateDelegate(Expression.GetDelegateType(componentType, typeof(T)));
    }

    private void Start()
    {
        Abacus.Instance.AddRecord(this);

        componentType = recordFrom.GetType().UnderlyingSystemType;


        var propertyInfo = componentType.GetProperty(recordName);
        if (propertyInfo == null)
        {
            Debug.LogError($"Property info (type: {typeof(T)}, name:{recordName}) was not found on component {componentType}");
        }

        _read = CreateDelegate(componentType, propertyInfo.GetGetMethod());

        //else
        //{
        //    // Field testing
        //    var fieldInfo = componentType.GetField(recordName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //    if (fieldInfo == null)
        //    {
        //        Debug.LogError($"Field info (type: {typeof(T)}, name:{recordName}) was not found on component {componentType}");
        //    }
        //    else Debug.Log($"Found Field {recordName}");

        //    //_read = CreateDelegate(componentType, fieldInfo.GetValue(recordFrom))
        //}

        _previousValue = GetValue();
        startTime = Time.time;
        History.Add(new DataPoint<T>(_previousValue, startTime));
    }

    private void Update()
    {
        if (Time.time - startTime >= TimeStep)
        {
            Record();
            startTime = Time.time;
        }
    }

    public void Record()
    {
        //var currentValue = GetValue();
        //if (!AreEqual(_previousValue, currentValue))
        //{
        //    History.Add(new DataPoint<T>(currentValue, Time.time));
        //    _previousValue = currentValue;
        //}
        _lastRecordedValue = GetValue();
        History.Add(new DataPoint<T>(_lastRecordedValue, Time.time));
    }

    public void SetSource(Component component)
    {
        recordFrom = component;
    }

    public T GetLastRecordedValue()
    {
        return _lastRecordedValue;
    }
}