using System.Reflection;
using UnityEngine;

namespace Abacus.Tests
{
    /// <summary>
    /// Utilities for Unit Testing in Unity
    /// </summary>
    public static class TestUtilities
    {
        /// <summary>
        /// Uses reflection to get a field value from an object instance
        /// </summary>
        /// <remarks>
        /// This is almost always a testing anti-pattern.  Use at your own risk.
        /// </remarks>
        /// <typeparam name="T">Instance Type</typeparam>
        /// <param name="instance">Instance</param>
        /// <param name="fieldName">Field name for the value to retrieve</param>
        /// <returns>Value of given field</returns>
        public static object GetInstanceField<T>(T instance, string fieldName)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = typeof(T).GetField(fieldName, bindingFlags);
            return field.GetValue(instance);
        }

        /// <summary>
        /// Uses reflection to invoke a method from an object instance
        /// </summary>
        /// <remarks>
        /// - This can easily be a testing anti-pattern.  Use at your own risk.
        /// - Good for running a component's Start method in a Editor unit test
        /// - Specify the type parameter as a parent class to get access to methods inherited from them
        /// </remarks>
        /// <typeparam name="T">The class type to retrieve the method from</typeparam>
        /// <param name="instance">An object instance of the class to call the method from</param>
        /// <param name="methodName">The name of the method</param>
        /// <param name="parameters">Any parameters to use in the method call</param>
        /// <returns>The return value of the method called (returned as type object)</returns>
        public static object InvokeInstanceMethod<T>(T instance, string methodName, params object[] parameters)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;
            MethodInfo dynamicMethod = typeof(T).GetMethod(methodName, bindingFlags);

            if (dynamicMethod == null) Debug.LogError("Method could not be invoked");

            return dynamicMethod.Invoke(instance, parameters);
        }
    }
}