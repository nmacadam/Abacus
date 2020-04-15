using NSubstitute;
using NUnit.Framework;
using UnityEditor.SceneManagement;

namespace Abacus.Tests
{
    public class AbacusWriterTests
    {
        [SetUp]
        public void ResetScene()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }

        [Test]
        public void LazyInstantiationIsNotNull()
        {
            Assert.IsNotNull(AbacusWriter.Instance);
        }

        [Test]
        public void TemporalRecordersAreRegistered()
        {
            ITemporal temporalRecorder = Substitute.For<ITemporal>();
            AbacusWriter.Instance.AddRecord(temporalRecorder);
            Assert.IsNotEmpty(AbacusWriter.Instance.RegisteredTemporals);
        }

        [Test]
        public void RecordableRecordersAreRegistered()
        {
            IRecordable recordable = Substitute.For<IRecordable>();
            AbacusWriter.Instance.AddRecord(recordable);
            Assert.IsNotEmpty(AbacusWriter.Instance.RegisteredRecordables);
        }
    }
}