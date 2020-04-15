using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abacus.Internal
{
    /// <summary>
    /// Abstract base class for all Metric types; Create a new Metric class by inheriting from this
    /// </summary>
    /// <typeparam name="T">The data type for the metric to hold</typeparam>
    public abstract class Metric<T> : MonoBehaviour, IRecordable
    {
        [SerializeField] protected bool _recordOnStart = true;
        [SerializeField] protected float _timeStep = 1f;
        public float TimeStep => _timeStep;

        protected bool _isEnabled = false;
        public bool IsEnabled => _isEnabled;

        // The fields are serialized to be managed by the custom inspector
        [SerializeField] protected GameObject recordGameObject;
        [SerializeField] protected Component recordFrom;
        [SerializeField] protected int recordFromIndex;
        [SerializeField] protected string recordName;
        [SerializeField] protected int recordNameIndex;

        protected Type componentType;

        public List<DataPoint<T>> History { get; } = new List<DataPoint<T>>();

        protected float startTime;

        protected T _lastRecordedValue;

        protected T _previousValue;

        /// <summary>
        /// Returns the metric's data as an object to be serialized
        /// </summary>
        /// <returns>The metric's data as an object</returns>
        public object Dump()
        {
            return History.ToArray();
        }

        /// <summary>
        /// The name of the variable that this Metric records
        /// </summary>
        /// <returns>The type that this Metric records for</returns>
        public string GetVariableName()
        {
            return recordName;
        }

        /// <summary>
        /// The type that this Metric records
        /// </summary>
        /// <returns>The type that this Metric records for</returns>
        public Type GetValueType()
        {
            return typeof(T);
        }

        public abstract T GetValue();

        /// <summary>
        /// The last value recorded by this metric
        /// </summary>
        /// <returns>The last value recorded by this metric</returns>
        public T GetLastRecordedValue()
        {
            return _lastRecordedValue;
        }

        /// <summary>
        /// Records the current value of the attached variable
        /// </summary>
        public void Record()
        {
            _lastRecordedValue = GetValue();
            History.Add(new DataPoint<T>(_lastRecordedValue, Time.time));
        }

        /// <summary>
        /// Sets the component that the metric retrieves a field or property from
        /// </summary>
        /// <param name="component"></param>
        public void SetSource(Component component)
        {
            recordFrom = component;
        }

        /// <summary>
        /// Enables the metric recorder
        /// </summary>
        public void Enable()
        {
            _isEnabled = true;
        }

        /// <summary>
        /// Disables the metric recorder
        /// </summary>
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
}