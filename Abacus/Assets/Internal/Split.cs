namespace Abacus
{
    /// <summary>
    /// Splits are for measuring the duration of discrete, sequential events
    /// </summary>
    public class Split : TimeDuration
    {
        public string Name;

        public Split(string name)
        {
            Name = name;
        }
    }
}