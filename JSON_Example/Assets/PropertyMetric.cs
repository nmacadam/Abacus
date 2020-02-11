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
public interface IMetric<T> : IRecordable
{
    T GetValue();
    T GetLastRecordedValue();
    List<DataPoint<T>> History { get; }
}

/// <summary>
/// Abstract base class for all Metric types; Create a new Metric class by inheriting from this
/// </summary>
/// <typeparam name="T">The data type for the metric to hold</typeparam>
public abstract class PropertyMetric<T> : MonoBehaviour, IMetric<T>
{
    [SerializeField] private bool _recordOnStart = true;
    [SerializeField] private float _timeStep = 1f;
    public float TimeStep => _timeStep;

    public List<DataPoint<T>> History { get; } = new List<DataPoint<T>>();

    private bool _isEnabled = false;
    public bool IsEnabled => _isEnabled;

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

    /// <summary>
    /// Retrieve the current value from the property
    /// </summary>
    /// <remarks>Uses reflection!  Call only as necessary!</remarks>
    /// <returns>The current value from the property</returns>
    public T GetValue()
    {
        return (T)_read.DynamicInvoke(recordFrom);
    }

    /// <summary>
    /// The type that this PropertyMetric records for
    /// </summary>
    /// <returns>The type that this PropertyMetric records for</returns>
    public Type GetValueType()
    {
        return typeof(T);
    }

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

        if (_recordOnStart) Enable();

        if (IsEnabled)
        {
            _previousValue = GetValue();
            startTime = Time.time;
            History.Add(new DataPoint<T>(_previousValue, startTime));
        }
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

    public void Enable()
    {
        _isEnabled = true;
    }

    public void Disable()
    {
        _isEnabled = false;
    }
}