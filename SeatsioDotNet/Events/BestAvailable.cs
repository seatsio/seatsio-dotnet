using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class BestAvailable
    {
        public int Number { get; }
        public IEnumerable<string> Categories { get; }
        public IEnumerable<Dictionary<string, object>> ExtraData { get; }

        public BestAvailable(int number)
        {
            Number = number;
        }

        public BestAvailable(int number, IEnumerable<string> categories)
        {
            Categories = categories;
            Number = number;
        }

        public BestAvailable(int number, IEnumerable<string> categories, IEnumerable<Dictionary<string, object>> extraData)
        {
            Categories = categories;
            Number = number;
            ExtraData = extraData;
        }

        public Dictionary<string, object> AsDictionary()
        {
            var dictionary = new Dictionary<string, object>
            {
                {"number", Number}
            };
            if (Categories != null)
            {
                dictionary.Add("categories", Categories);
            }

            if (ExtraData != null)
            {
                dictionary.Add("extraData", ExtraData);
            }

            return dictionary;
        }
    }
}