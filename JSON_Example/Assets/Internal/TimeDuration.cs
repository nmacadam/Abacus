using UnityEngine;

namespace Abacus
{
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

        public void StartClock()
        {
            StartTime = Time.time;
            EndTime = -1f;
        }

        public void StopClock()
        {
            EndTime = Time.time;
        }
    }
}