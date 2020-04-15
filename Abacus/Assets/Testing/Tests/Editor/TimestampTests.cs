using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Abacus.Tests
{
    public class TimestampTests
    {
        private EventTimestamper _timestamper;

        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var go = new GameObject();
            _timestamper = go.AddComponent<EventTimestamper>();
        }

        [Test]
        public void StopwatchReturnsCorrectLabel()
        {
            var name = "StopwatchName";
            _timestamper.Label = name;
            Assert.AreEqual(name, _timestamper.GetVariableName());
        }

        [Test]
        public void TimestampStampIsWritten()
        {
            _timestamper.Stamp("NewStamp");

            Assert.IsNotEmpty(_timestamper.Timestamps);
        }

        [Test]
        public void StopwatchDumpIsEquivalentToStoredValues()
        {
            _timestamper.Stamp("NewStamp");

            var dump = _timestamper.Dump();

            Assert.AreEqual(_timestamper.Timestamps.ToArray(), dump);
        }

        [Test]
        public void TimestampRegistersSelfToWriter()
        {
            Assert.Contains(_timestamper, AbacusWriter.Instance.RegisteredTemporals);
        }
    }
}
