using UnityEngine;

namespace Abacus.Examples
{
    /// <summary>
    /// Simple component with a public field, a private field, a public property, and a private property
    /// </summary>
    public class RecordFromTest : MonoBehaviour
    {
        public float PublicFloatField;

        public Vector3 PublicNonProperty;

        [SerializeField] private Vector3 _inner = default;

        public Vector3 TestValue
        {
            get { return _inner; }
        }

        private Vector3 _testValue
        {
            get { return _inner; }
        }
    }
}