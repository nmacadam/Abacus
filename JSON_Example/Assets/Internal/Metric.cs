using System;
using System.Collections.Generic;
using UnityEngine;

namespace Abacus
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
        //public string[] Dump()
        //{
        //    Debug.Log("Dumping property " + recordName);
        //    List<string> output = new List<string>();
        //    foreach (var value in History)
        //    {
        //        output.Add(JsonUtility.ToJson(value));
        //    }

        //    return output.ToArray();
        //}
        public object Dump()
        {
            Debug.Log("Dumping metric " + recordName);
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
}