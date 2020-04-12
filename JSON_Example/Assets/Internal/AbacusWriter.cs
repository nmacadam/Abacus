﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Abacus.Internal;
using Newtonsoft.Json;
using UnityEngine;

namespace Abacus
{
    public class AbacusWriter : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        private static bool __shuttingDown = false;
        private static bool _shuttingDown
        {
            get
            {
                return __shuttingDown;
            }
            set
            {
                if (value && !__shuttingDown)
                {
                    _instance.Dump();
                }

                __shuttingDown = value;
            }
        }
        private static object _lock = new object();
        private static AbacusWriter _instance;
        private static DateTime _start;
        private static DateTime _finish;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static AbacusWriter Instance
        {
            get
            {
                if (__shuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(AbacusWriter) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        //_instance = new AbacusWriter();
                        // Search for existing instance.
                        _instance = (AbacusWriter)FindObjectOfType(typeof(AbacusWriter));

                        // Create new instance if one doesn't already exist.
                        if (_instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<AbacusWriter>();
                            singletonObject.name = typeof(AbacusWriter).ToString() + " (Singleton)";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return _instance;
                }
            }
        }

        private void Awake()
        {
            _start = DateTime.Now;
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }
        private void OnDestroy()
        {
            _shuttingDown = true;
        }

        public void Dump()
        {
            List<OutputData> outputRecords = new List<OutputData>();
            foreach (var record in recordables)
            {
                OutputData data;
                data.DisplayType = "set";
                data.VariableName = record.GetVariableName();
                data.VariableType = record.GetValueType().ToString();
                data.Data = record.Dump();
                outputRecords.Add(data);
            }

            foreach (var record in temporals)
            {
                OutputData data;
                data.DisplayType = record.DisplayType;
                data.VariableName = record.GetVariableName();
                data.VariableType = typeof(float).ToString();
                data.Data = record.Dump();
                outputRecords.Add(data);
            }

            _finish = DateTime.Now;
            AbacusContent outputContent;
            outputContent.ProjectData = new ProjectData(
                Application.productName, 
                Application.version,
                Application.platform.ToString(), 
                Application.companyName, 
                Application.unityVersion,
                _start.ToLongTimeString(),
                _finish.ToLongTimeString(),
                _start.ToShortDateString(),
                (_finish - _start).ToString()
            );
            outputContent.Records = outputRecords.ToArray();

            Debug.Log(JsonConvert.SerializeObject(outputContent, AbacusSettings.Instance.FormatOutput ? Formatting.Indented : Formatting.None));

            using (var sw = new StreamWriter("./data.json"))
            {
                sw.Write(JsonConvert.SerializeObject(outputContent, AbacusSettings.Instance.FormatOutput ? Formatting.Indented : Formatting.None));
            }
        }

        public void AddRecord(ITemporal value)
        {
            temporals.Add(value);
        }

        public void AddRecord(IRecordable value)
        {
            recordables.Add(value);
        }

        private List<IRecordable> recordables = new List<IRecordable>();
        private List<ITemporal> temporals = new List<ITemporal>();
    }
}