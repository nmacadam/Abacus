using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Abacus.Internal
{
    /// <summary>
    /// Writes the collected data to file
    /// </summary>
    public class AbacusWriter : MonoBehaviour
    {
        private static AbacusSettings _settings;

        // There's multiple layers of 'shut down' here, because the writer needs to write out the json before being destroyed

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
                            singletonObject.name = "Abacus Writer";

                            // Make instance persistent.
                            if (Application.isPlaying)
                            {
                                DontDestroyOnLoad(singletonObject);
                            }

                            if (_settings == null)
                            {
                                _settings = AbacusSettings.Instance;
                            }
                            else if (_settings == null && AbacusSettings.Instance == null)
                            {
                                Debug.Log("Couldn't retrieve settings");
                            }
                        }
                    }

                    return _instance;
                }
            }
        }

        private void Awake()
        {
            _start = DateTime.Now;
            SceneManager.activeSceneChanged += OnBeforeSceneChange;
        }

        private void OnBeforeSceneChange(Scene current, Scene next)
        {
            StoreIntermediateData();
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }
        private void OnDestroy()
        {
            _shuttingDown = true;
        }

        private List<OutputData> _intermediateRecords = new List<OutputData>();

        /// <summary>
        /// Stores a temporary OutputData object for export at the end of the application or on a Dump call
        /// </summary>
        public void StoreIntermediateData()
        {
            foreach (var record in recordables)
            {
                OutputData data;
                data.DisplayType = "set";
                data.VariableName = record.GetVariableName();
                data.VariableType = record.GetValueType().ToString();
                data.Data = record.Dump();
                _intermediateRecords.Add(data);
            }

            foreach (var record in temporals)
            {
                OutputData data;
                data.DisplayType = record.DisplayType;
                data.VariableName = record.GetVariableName();
                data.VariableType = typeof(float).ToString();
                data.Data = record.Dump();
                _intermediateRecords.Add(data);
            }

            recordables.Clear();
            temporals.Clear();
        }

        /*
        private List<string> _intermediateData = new List<string>();

        public void DumpIntermediateAsTempFile()
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

            var intermediateContent = outputRecords.ToArray();

            var path = $"{AbacusSettings.Instance.WritePath}/abacusTemp_{_intermediateData.Count}.json";
            _intermediateData.Add(path);
            using (var sw = new StreamWriter(path))
            {
                sw.Write(JsonConvert.SerializeObject(intermediateContent, AbacusSettings.Instance.FormatOutput ? Formatting.Indented : Formatting.None));
            }

            recordables.Clear();
            temporals.Clear();
        }

        private string RetrieveIntermediateTempFileData()
        {
            string intermediateJSON = string.Empty;

            for (var i = 0; i < _intermediateData.Count; i++)
            {
                string text = System.IO.File.ReadAllText(_intermediateData[i]);
                text = text.TrimStart('[');
                text = text.TrimEnd(']');

                if (i > _intermediateData.Count - 1)
                {
                    text += ",";
                }

                intermediateJSON += text;
            }

            _intermediateData.Clear();

            return intermediateJSON;
        }
        */

        /// <summary>
        /// Converts all registered recorder's histories to a json output
        /// </summary>
        public void Dump()
        {
            List<OutputData> outputRecords = _intermediateRecords.Count > 0 ? _intermediateRecords : new List<OutputData>();

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

            //Debug.Log(JsonConvert.SerializeObject(outputContent, AbacusSettings.Instance.FormatOutput ? Formatting.Indented : Formatting.None));

            var path = $"{AbacusSettings.Instance.WritePath}/abacus_data_{DateTime.Now:MM-dd-yyyy_hh-mm-ss-tt}.json";
            using (var sw = new StreamWriter(path))
            {
                sw.Write(JsonConvert.SerializeObject(outputContent, AbacusSettings.Instance.FormatOutput ? Formatting.Indented : Formatting.None));
            }

            recordables.Clear();
            temporals.Clear();
        }

        /// <summary>
        /// Registers a temporal recorder
        /// </summary>
        /// <param name="value">The temporal recorder</param>
        public void AddRecord(ITemporal value)
        {
            temporals.Add(value);
        }

        /// <summary>
        /// Registers a recordable
        /// </summary>
        /// <param name="value">The recordable</param>
        public void AddRecord(IRecordable value)
        {
            recordables.Add(value);
        }

        private List<IRecordable> recordables = new List<IRecordable>();
        private List<ITemporal> temporals = new List<ITemporal>();

        public List<IRecordable> RegisteredRecordables => recordables;
        public List<ITemporal> RegisteredTemporals => temporals;
    }
}