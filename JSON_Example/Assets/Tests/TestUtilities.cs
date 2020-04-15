using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Abacus.Tests
{
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
        private static object GetInstanceField<T>(T instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = typeof(T).GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }
    }
}