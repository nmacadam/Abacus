using Abacus.Internal;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Abacus.Tests
{
    public class StopwatchTests
    {
        private Stopwatch _stopwatch;

        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var go = new GameObject();
            _stopwatch = go.AddComponent<Stopwatch>();
        }

        [Test]
        public void StopwatchReturnsCorrectLabel()
        {
            var name = "StopwatchName";
            _stopwatch.Label = name;
            Assert.AreEqual(name, _stopwatch.GetVariableName());
        }

        [Test]
        public void StopwatchFirstToggleStartsWatch()
        {
            _stopwatch.Toggle();
            Assert.IsTrue(_stopwatch.IsRecording);
        }

        [Test]
        public void StopwatchSecondToggleStopsWatch()
        {
            _stopwatch.Toggle();
            _stopwatch.Toggle();
            Assert.IsFalse(_stopwatch.IsRecording);
        }

        [Test]
        public void StopwatchMeasureIsWritten()
        {
            _stopwatch.Toggle();
            _stopwatch.Toggle();

            Assert.IsNotEmpty(_stopwatch.Measures);
        }

        [Test]
        public void StopwatchTimeDurationIsAccurate()
        {
            _stopwatch.Toggle();
            _stopwatch.Toggle();

            var measure = _stopwatch.Measures[0];
            Assert.AreEqual(measure.Duration, measure.EndTime - measure.StartTime);
        }

        [Test]
        public void StopwatchDumpIsEquivalentToStoredValues()
        {
            _stopwatch.Toggle();
            _stopwatch.Toggle();
            var dump = _stopwatch.Dump();

            Assert.AreEqual(_stopwatch.Measures.ToArray(), dump);
        }

        [Test]
        public void StopwatchRegistersSelfToWriter()
        {
            TestUtilities.InvokeInstanceMethod(_stopwatch, "Start");
            Assert.Contains(_stopwatch, AbacusWriter.Instance.RegisteredTemporals);
        }
    }
}