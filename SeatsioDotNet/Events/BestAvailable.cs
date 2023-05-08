using System;
using System.Collections.Generic;

namespace SeatsioDotNet.Events
{
    public class BestAvailable
    {
        public int Number { get; }
        public IEnumerable<string> Categories { get; }
        public IEnumerable<Dictionary<string, object>> ExtraData { get; }
        public string[] TicketTypes { get; }
        public bool? TryToPreventOrphanSeats;

        public BestAvailable(int number, IEnumerable<string> categories = null, IEnumerable<Dictionary<string, object>> extraData = null, string[] ticketTypes = null, bool? tryToPreventOrphanSeats = null)
        {
            Categories = categories;
            Number = number;
            ExtraData = extraData;
            TicketTypes = ticketTypes;
            TryToPreventOrphanSeats = tryToPreventOrphanSeats;
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
            if (TicketTypes != null)
            {
                dictionary.Add("ticketTypes", TicketTypes);
            }
            if (TryToPreventOrphanSeats != null)
            {
                dictionary.Add("tryToPreventOrphanSeats", TryToPreventOrphanSeats);
            }

            return dictionary;
        }
    }
}