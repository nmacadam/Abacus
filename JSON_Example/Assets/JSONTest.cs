using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AbacusMain.Output
{
    public class Data
    {
        public string test = "what";
        public int number = 12;
    }

    [System.Serializable]
    public class Chart
    {
        public string type;
        public ChartData data;
    }

    [System.Serializable]
    public class ChartData
    {
        public string[] labels;
        public ChartDataSet[] dataSets;
    }

    [System.Serializable]
    public class ChartDataSet
    {
        public string label;
        public int[] data;
        public Color[] backgroundColor;
        public Color[] borderColor;
    }


    public class JSONTest : MonoBehaviour
    {
        struct SomeData
        {
            public Vector2 v2;
            public Vector3 v3;

            public SomeData(Vector2 vec2, Vector3 vec3)
            {
                v2 = vec2;
                v3 = vec3;
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            ChartDataSet set = new ChartDataSet();
            set.label = "Some data";
            set.data = new int[] { 12, 19, 3, 5, 2, 3 };
            set.backgroundColor = new Color[] { new Color(255, 99, 132, 0.2f) };
            set.borderColor = new Color[] { new Color(255, 99, 132, 1) };

            ChartData data = new ChartData();
            data.dataSets = new ChartDataSet[] { set };

            Chart chart = new Chart();
            chart.type = "line";
            chart.data = data;


            using (var sw = new StreamWriter("example.json"))
            {
                //SomeData data = new SomeData(new Vector2(0,1), new Vector3(0,1,2));

                sw.Write(JsonUtility.ToJson(chart));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}