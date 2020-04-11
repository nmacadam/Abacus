using System;
using System.Reflection;
using UnityEngine;

namespace Abacus
{
    /// <summary>
    /// Abstract base class for all field-based metrics; Create a new FieldMetric recorder class by inheriting from this
    /// </summary>
    /// <typeparam name="T">The data type for the field metric to hold</typeparam>
    public abstract class FieldMetric<T> : Metric<T>
    { 
        private FieldInfo fieldInfo;

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
            return (T)fieldInfo.GetValue(recordFrom);
        }

        private void Start()
        {
            AbacusWriter.Instance.AddRecord(this);

            componentType = recordFrom.GetType().UnderlyingSystemType;

            fieldInfo = componentType.GetField(recordName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
            {
                Debug.LogError($"Field info (type: {typeof(T)}, name:{recordName}) was not found on component {componentType}");
            }
      
            if (_recordOnStart) Enable();

            if (IsEnabled)
            {
                _previousValue = GetValue();
                startTime = Time.time;
                History.Add(new DataPoint<T>(_previousValue, startTime));
            }
        }
    }
}