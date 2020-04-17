using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Abacus.Tests
{
    public class FieldMetricTests
    {
        private FloatFieldMetric _recordFrom;
        private FloatFieldMetric _fieldMetric;

        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var go = new GameObject();
            _recordFrom = go.AddComponent<FloatFieldMetric>();
            _fieldMetric = go.AddComponent<FloatFieldMetric>();
        }

        [Test]
        public void RetrievesFieldOnStart()
        {
            _fieldMetric.AttachVariable(_recordFrom, "_timeStep");
            TestUtilities.InvokeInstanceMethod<FieldMetric<float>>(_fieldMetric, "Start");
            Assert.IsTrue(_fieldMetric.RetrievedMember);
        }

        [Test]
        public void RetrievesCorrectValue()
        {
            _recordFrom.SetTimeStep(5f);

            _fieldMetric.AttachVariable(_recordFrom, "_timeStep");
            TestUtilities.InvokeInstanceMethod<FieldMetric<float>>(_fieldMetric, "Start");
            Assert.IsTrue(_fieldMetric.GetValue() == 5f);
        }
    }
}
