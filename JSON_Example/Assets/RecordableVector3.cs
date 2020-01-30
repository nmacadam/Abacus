using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public class RecordableVector3 : RecordableValue<int>
{
    public Component recordFrom;
    public Type componentType;
    public string propertyNamePlaceholder;
    public Type propertyType;
    //public PropertyInfo propertyInfo;

    //private Action<Transform, Vector3> setter;
    private Func<Transform, Vector3> getter;
    //private Func<object, Vector3> getter;

    private delegate Vector3 Retriever(Transform o);

    private Retriever r;

    Delegate d;

    public Vector3 val;

    private void Start()
    {
        componentType = recordFrom.GetType().UnderlyingSystemType;
        PropertyInfo p = componentType.GetProperty(propertyNamePlaceholder);
        propertyType = p.PropertyType;

        //setter = (Action<Transform, Vector3>)Delegate.CreateDelegate(
        //    typeof(Action<Transform, Vector3>),
        //    null,
        //    typeof(Vector3).GetProperty("position").GetSetMethod()
        //    );

        // 
        //getter = (Func<Transform, Vector3>)p.GetGetMethod().CreateDelegate(typeof(Func<Transform, Vector3>));
        //getter = (Func<Transform, Vector3>)CreateDelegate(componentType, propertyType, p.GetGetMethod());

        d = CreateDelegate(componentType, propertyType, p.GetGetMethod());
        //r = new Retriever();
        //r = (Retriever)CreateDelegate(componentType, propertyType, p.GetGetMethod());


        //var value = d.DynamicInvoke(recordFrom);
        //var value = r((Transform)recordFrom);
        //Debug.Log(value);

        //Vector3 i = GetFunc(p.GetGetMethod()).Invoke(recordFrom);
        //Debug.Log(i);
        //getter = GetFunc(p.GetGetMethod());

        //getter = GetFunc(componentType, typeof(Vector3), p.GetGetMethod());

        //getter = (Func<Transform, Vector3>)Delegate.CreateDelegate(
        //    typeof(Func<Transform, Vector3>),
        //    null,
        //    p.GetGetMethod()
        //    );


        if (getter == null) Debug.Log("Getter is null");
    }

    static Delegate CreateDelegate(Type a, Type b, MethodInfo method)
    {
        return method.CreateDelegate(Expression.GetDelegateType(a, b));
    }

    //public static Delegate CreateAction(Type type, MethodInfo info)
    //{
    //    var methodInfo = info.MakeGenericMethod(type);
    //    var actionT = typeof(Action<>).MakeGenericType(type);
    //    return Delegate.CreateDelegate(actionT, methodInfo);
    //}

    //public Func<object, Vector3> GetFunc(Type componentType, Type propType, MethodInfo info)
    //{
    //    var body = Expression.Constant(info.Invoke(recordFrom, null));
    //    var parameter = Expression.Parameter(componentType);
    //    var delegateType = typeof(Func<,>).MakeGenericType(componentType, propType);
    //    /*dynamic*/
    //    var lambda = Expression.Lambda(delegateType, body, parameter);

    //    return (Func<object, Vector3>)lambda.Compile();
    //}

    public Func<object, Vector3> GetFunc(Type componentType, Type propType, MethodInfo methodInfo)
    {
        //var body = Expression.Call()
        var obj = Expression.Parameter(componentType);
        //var convert = Expression.Convert(obj, methodInfo.GetParameters().First().ParameterType);
        var call = Expression.Call(methodInfo);
        var delegateType = typeof(Func<,>).MakeGenericType(componentType, propType);
        var lambda = Expression.Lambda(delegateType, obj);

        return (Func<object, Vector3>)lambda.Compile();
    }

    private void Update()
    {
        //var value = getter((Transform)recordFrom);
        var value = d.DynamicInvoke(recordFrom);
        var v3 = (Vector3)value;
        val = v3;
        //Debug.Log((Vector3)value);
    }
}