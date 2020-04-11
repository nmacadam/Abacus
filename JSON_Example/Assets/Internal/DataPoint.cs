namespace Abacus
{
    /// <summary>
    /// Defines a generic data recording by pairing a data type with a timestamp
    /// </summary>
    /// <typeparam name="T">The data type to record</typeparam>
    public struct DataPoint<T>
    {
        public T Value;
        public float Time;

        public DataPoint(T value, float time)
        {
            Value = value;
            Time = time;
        }
    }
}