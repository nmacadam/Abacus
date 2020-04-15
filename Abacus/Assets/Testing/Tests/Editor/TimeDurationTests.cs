using NUnit.Framework;
using UnityEngine;

namespace Abacus.Tests
{
    public class TimeDurationTests
    {
        private TimeDuration _duration;

        [SetUp]
        public void Setup()
        {
            _duration = new TimeDuration();
        }

        [Test]
        public void TimeDurationStartTimeIsAccurate()
        {
            double maxCallTimeDelta = 0.01;

            _duration.StartClock();
            Assert.IsTrue(Time.time - _duration.StartTime < maxCallTimeDelta);
        }

        [Test]
        public void TimeDurationEndTimeIsAccurate()
        {
            double maxCallTimeDelta = 0.01;

            _duration.StartClock();
            _duration.StopClock();
            Assert.IsTrue(Time.time - _duration.EndTime < maxCallTimeDelta);
        }

        [Test]
        public void TimeDurationDurationIsAccurate()
        {
            _duration.StartClock();
            _duration.StopClock();
            Assert.AreEqual(_duration.EndTime - _duration.StartTime, _duration.Duration);
        }
    }
}
