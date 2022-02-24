using System;

namespace SeatsioDotNet.Charts
{
    public abstract class CategoryKey
    {
        public abstract object ToJson();

        public static LongCategoryKey Of(long key)
        {
            return new LongCategoryKey(key);
        }

    }

    public class LongCategoryKey : CategoryKey
    {
        private long Value { get; set; }

        public LongCategoryKey(long value)
        {
            Value = value;
        }

        public override object ToJson() => Value;
    }

    public class StringCategoryKey : CategoryKey
    {
        private string Value { get; set; }

        public StringCategoryKey(string value)
        {
            Value = value;
        }

        public override object ToJson() => Value;
    }
}
