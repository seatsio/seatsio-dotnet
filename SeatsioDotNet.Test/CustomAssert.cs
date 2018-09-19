using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SeatsioDotNet.Test
{
    public class CustomAssert
    {
        public static void CloseTo(DateTime expected, DateTime actual)
        {
            Assert.True(actual > expected.AddMinutes(-2) && actual < expected.AddMinutes(2));
        }

        public static void ContainsOnly(IEnumerable<object> expected, IEnumerable<object> actual)
        {
            var expectedList = expected.ToList();
            expectedList.Sort();
            var actualList = actual.ToList();
            actualList.Sort();
            Assert.Equal(expectedList, actualList);
        }
    }
}