using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Xunit;

namespace SeatsioDotNet.Test
{
    public class CustomAssert
    {
        public static void CloseTo(DateTime expected, DateTime actual)
        {
            Assert.True(actual > expected.AddMinutes(-1) && actual < expected.AddMinutes(1));
        }

        public static void ContainsExactly(IEnumerable<object> expected, IEnumerable<object> actual)
        {
            Assert.Equal(expected.ToImmutableSortedSet(), actual.ToImmutableSortedSet());
        }
    }
}