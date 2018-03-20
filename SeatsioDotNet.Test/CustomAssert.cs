using System;
using Xunit;

namespace SeatsioDotNet.Test
{
    public class CustomAssert
    {
        public static void CloseTo(DateTime expected, DateTime actual)
        {
            Assert.True(actual > expected.AddMinutes(-1) && actual < expected.AddMinutes(1));
        }
    }
}