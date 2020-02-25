using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Defines a generic data recording by pairing a data type with a timestamp
/// </summary>
/// <typeparam name="T">The data type to record</typeparam>
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

/// <summary>
/// Implements methods for recordability, but without any hard data type
/// </summary>
public interface IRecordable
{
    Type GetValueType();
    string[] Dump();
    void Record();
    void SetSource(Component component);
    void Enable();
    void Disable();

    float TimeStep { get; }
    bool IsEnabled { get; }
}

/// <summary>
/// Implements further methods for recording data, with a specified data type
/// </summary>
/// <typeparam name="T">The type of data that the metric is</typeparam>
//public interface IMetric<T> : IRecordable
//{
//    T GetValue();
//    T GetLastRecordedValue();
//    List<DataPoint<T>> History { get; }
//}

public abstract class Metric<T> : MonoBehaviour, IRecordable
{
    [SerializeField] protected bool _recordOnStart = true;
    [SerializeField] protected float _timeStep = 1f;
    public float TimeStep => _timeStep;
    
    protected bool _isEnabled = false;
    public bool IsEnabled => _isEnabled;

    [SerializeField] protected Component recordFrom;
    protected Type componentType;

    [SerializeField] protected string recordName;

    public List<DataPoint<T>> History { get; } = new List<DataPoint<T>>();

    protected float startTime;

    protected T _lastRecordedValue;

    protected T _previousValue;

    /// <summary>
    /// Dumps an array of JSON data 
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// The type that this PropertyMetric records for
    /// </summary>
    /// <returns>The type that this PropertyMetric records for</returns>
    public Type GetValueType()
    {
        return typeof(T);
    }

    public abstract T GetValue();

    public T GetLastRecordedValue()
    {
        return _lastRecordedValue;
    }

    public void Record()
    {
        _lastRecordedValue = GetValue();
        History.Add(new DataPoint<T>(_lastRecordedValue, Time.time));
    }

    public void SetSource(Component component)
    {
        recordFrom = component;
    }

    public void Enable()
    {
        _isEnabled = true;
    }

    public void Disable()
    {
        _isEnabled = false;
    }

    private void Update()
    {
        if (IsEnabled)
        {
            if (Time.time - startTime >= TimeStep)
            {
                Record();
                startTime = Time.time;
            }
        }
    }
}

/// <summary>
/// Abstract base class for all Metric types; Create a new Metric class by inheriting from this
/// </summary>
/// <typeparam name="T">The data type for the metric to hold</typeparam>
public abstract class PropertyMetric<T> : Metric<T>
{
    private Delegate _read;

    protected virtual bool AreEqual(T a, T b)
    {
        if (typeof(IEquatable<>).IsAssignableFrom(typeof(T)))
        {
            return a.Equals(b);
        }
        
        //Debug.LogError($"Type '{typeof(T)}' does not implement IEquatable or override AreEqual method");
        return a.Equals(b);
    }

    /// <summary>
    /// Retrieve the current value from the property
    /// </summary>
    /// <remarks>Uses reflection!  Call only as necessary!</remarks>
    /// <returns>The current value from the property</returns>
    public override T GetValue()
    {
        return (T)_read.DynamicInvoke(recordFrom);
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

        if (_recordOnStart) Enable();

        if (IsEnabled)
        {
            _previousValue = GetValue();
            startTime = Time.time;
            History.Add(new DataPoint<T>(_previousValue, startTime));
        }
    }
}