using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Abacus.Tests
{
    public class SplitwatchTests
    {
        private Splitwatch _splitwatch;

        [SetUp]
        public void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var go = new GameObject();
            _splitwatch = go.AddComponent<Splitwatch>();
        }

        [Test]
        public void SplitwatchReturnsCorrectLabel()
        {
            var name = "SplitwatchName";
            _splitwatch.Label = name;
            Assert.AreEqual(name, _splitwatch.GetVariableName());
        }

        [Test]
        public void SplitwatchFirstToggleStartsWatch()
        {
            _splitwatch.ToggleSplit("NewSplit");
            Assert.IsTrue(_splitwatch.IsRecording);
        }

        [Test]
        public void SplitwatchSecondToggleStopsWatch()
        {
            _splitwatch.ToggleSplit("NewSplit");
            _splitwatch.ToggleSplit("NewSplit");
            Assert.IsFalse(_splitwatch.IsRecording);
        }

        [Test]
        public void SplitwatchMeasureIsWritten()
        {
            _splitwatch.ToggleSplit("NewSplit");
            _splitwatch.ToggleSplit("NewSplit");

            Assert.IsNotEmpty(_splitwatch.Splits);
        }

        [Test]
        public void SplitwatchDumpIsEquivalentToStoredValues()
        {
            _splitwatch.ToggleSplit("NewSplit");
            _splitwatch.ToggleSplit("NewSplit");
            var dump = _splitwatch.Dump();

            Assert.AreEqual(_splitwatch.Splits.ToArray(), dump);
        }

        [Test]
        public void SplitwatchRegistersSelfToWriter()
        {
            Assert.Contains(_splitwatch, AbacusWriter.Instance.RegisteredTemporals);
        }
    }
}
