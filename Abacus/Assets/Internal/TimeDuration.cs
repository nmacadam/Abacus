using UnityEngine;

namespace Abacus
{
    /// <summary>
    /// Stores a time duration, with start, end, and duration
    /// </summary>
    public class TimeDuration
    {
        public float StartTime;
        public float EndTime;
        public float Duration
        {
            get
            {
                if (EndTime < 0) return Time.time - StartTime;
                return EndTime - StartTime;
            }
        }

        /// <summary>
        /// Saves the start time for the duration
        /// </summary>
        public void StartClock()
        {
            StartTime = Time.time;
            EndTime = -1f;
        }

        /// <summary>
        /// Saves the end time for the duration
        /// </summary>
        public void StopClock()
        {
            EndTime = Time.time;
        }
    }
}