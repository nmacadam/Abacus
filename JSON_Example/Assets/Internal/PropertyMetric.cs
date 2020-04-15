using System;
using System.Linq.Expressions;
using System.Reflection;
using Abacus.Internal;
using UnityEngine;

namespace Abacus
{
    /// <summary>
    /// Abstract base class for all property-based metrics; Create a new PropertyMetric recorder class by inheriting from this
    /// </summary>
    /// <typeparam name="T">The data type for the property metric to hold</typeparam>
    public abstract class PropertyMetric<T> : Metric<T>
    {
        private Delegate _read;

        protected virtual bool AreEqual(T a, T b)
        {
            if (typeof(IEquatable<>).IsAssignableFrom(typeof(T)))
            {
                return a.Equals(b);
            }

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
            AbacusWriter.Instance.AddRecord(this);

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
}