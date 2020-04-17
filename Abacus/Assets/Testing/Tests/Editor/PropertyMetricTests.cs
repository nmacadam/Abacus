using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Abacus.Tests
{
    public class PropertyMetricTests
    {
        private FloatPropertyMetric _recordFrom;
        private FloatPropertyMetric _propertyMetric;

        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var go = new GameObject();
            _recordFrom = go.AddComponent<FloatPropertyMetric>();
            _propertyMetric = go.AddComponent<FloatPropertyMetric>();
        }

        [Test]
        public void RetrievesPropertyOnStart()
        {
            _propertyMetric.AttachVariable(_recordFrom, "TimeStep");
            TestUtilities.InvokeInstanceMethod<PropertyMetric<float>>(_propertyMetric, "Start");
            Assert.IsTrue(_propertyMetric.RetrievedMember);
        }

        [Test]
        public void RetrievesCorrectValue()
        {
            _recordFrom.SetTimeStep(5f);

            _propertyMetric.AttachVariable(_recordFrom, "TimeStep");
            TestUtilities.InvokeInstanceMethod<PropertyMetric<float>>(_propertyMetric, "Start");
            Assert.IsTrue(_propertyMetric.GetValue() == 5f);
        }
    }
}
