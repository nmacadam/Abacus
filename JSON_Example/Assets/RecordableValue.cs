using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public interface IRecordable<T>
{
    T GetValue();
    Type GetValueType();
    void Dump();

    List<T> History { get; }
}

public abstract class RecordableValue<T> : MonoBehaviour, IRecordable<T>
{
    public List<T> History { get; } = new List<T>();

    public Component recordFrom;
    public Type componentType;
    public string propertyNamePlaceholder;
    //public Type propertyType;

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
            output += value + " ";
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
        History.Add(_previousValue);
    }

    private void Update()
    {
        var currentValue = GetValue();
        if (!AreEqual(_previousValue, currentValue))
        {
            History.Add(currentValue);
            _previousValue = currentValue;
        }
    }

    private void OnDisable()
    {
        Dump();
    }
}