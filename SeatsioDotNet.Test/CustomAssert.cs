using System;
using Xunit;

namespace SeatsioDotNet.Test
{
    public class CustomAssert
    {
        public static void CloseTo(DateTime date, DateTime otherDate)
        {
            Assert.True(date > otherDate.AddMinutes(-1) && date < otherDate.AddMinutes(1));
        }
    }
}