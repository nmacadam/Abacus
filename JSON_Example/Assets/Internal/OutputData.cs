using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abacus
{
    [Serializable]
    public struct AbacusContent
    {
        public ProjectData ProjectData;
        public OutputData[] Records;
    }

    [Serializable]
    public struct ProjectData
    {
        public string Name;
        public string Version;
        public string Platform;
        public string Author;
        public string UnityVersion;
        public string StartTime;
        public string EndTime;
        public string Date;
        public string Duration;

        public ProjectData(string name, string version, string platform, string author, string unityVersion, string start, string end, string date, string duration)
        {
            Name = name;
            Version = version;
            Platform = platform;
            Author = author;
            UnityVersion = unityVersion;
            StartTime = start;
            EndTime = end;
            Date = date;
            Duration = duration;
        }
    }

    [Serializable]
    public struct OutputData
    {
        public string DisplayType;
        public string VariableName;
        public string VariableType;
        //public string[] Data;
        public object Data;
    }
}