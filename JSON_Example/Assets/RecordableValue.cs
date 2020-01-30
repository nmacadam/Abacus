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

public interface IRecordable<T>
{
    T GetValue();
    Type GetValueType();
    void Dump();

    List<DataPoint<T>> History { get; }
}

public abstract class RecordableValue<T> : MonoBehaviour, IRecordable<T>
{
    public List<DataPoint<T>> History { get; } = new List<DataPoint<T>>();

    public Component recordFrom;
    public Type componentType;
    public string propertyNamePlaceholder;

    private Delegate _read;

    private T _previousValue;

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

    public void Dump()
    {
        string output = "";
        foreach (var value in History)
        {
            output += $"[value;{value.Value}; time:{value.Time}]  ";
        }
        Debug.Log(output);
    }

    private static Delegate CreateDelegate(Type componentType, MethodInfo method)
    {
        return method.CreateDelegate(Expression.GetDelegateType(componentType, typeof(T)));
    }

    private void Start()
    {
        componentType = recordFrom.GetType().UnderlyingSystemType;
        var propertyInfo = componentType.GetProperty(propertyNamePlaceholder);
        if (propertyInfo == null)
        {
            Debug.LogError($"Property info (type: {typeof(T)}, name:{propertyNamePlaceholder}) was not found on component {componentType}");
        }

        _read = CreateDelegate(componentType, propertyInfo.GetGetMethod());

        _previousValue = GetValue();
        History.Add(new DataPoint<T>(_previousValue, Time.time));
    }

    private void Update()
    {
        var currentValue = GetValue();
        if (!AreEqual(_previousValue, currentValue))
        {
            History.Add(new DataPoint<T>(currentValue, Time.time));
            _previousValue = currentValue;
        }
    }

    private void OnDisable()
    {
        Dump();
    }
}