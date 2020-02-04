using UnityEngine;

public class RecordFromTest : MonoBehaviour
{
    public Vector3 PublicNonProperty;

    [SerializeField] private Vector3 _inner;

    public Vector3 TestValue
    {
        get { return _inner; }
    }

    private Vector3 _testValue
    {
        get { return _inner; }
    }
}
