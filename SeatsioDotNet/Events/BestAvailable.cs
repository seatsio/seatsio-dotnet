﻿using System.Collections.Generic;

namespace SeatsioDotNet.Events;

public class BestAvailable
{
    public int Number { get; }
    public IEnumerable<string> Categories { get; }
    public string Zone { get; }
    public IEnumerable<Dictionary<string, object>> ExtraData { get; }
    public string[] TicketTypes { get; }
    public bool? TryToPreventOrphanSeats;

    public BestAvailable(int number, IEnumerable<string> categories = null, IEnumerable<Dictionary<string, object>> extraData = null, string[] ticketTypes = null, bool? tryToPreventOrphanSeats = null, string zone = null)
    {
        Categories = categories;
        Number = number;
        ExtraData = extraData;
        TicketTypes = ticketTypes;
        TryToPreventOrphanSeats = tryToPreventOrphanSeats;
        Zone = zone;
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

        if (Zone != null)
        {
            dictionary.Add("zone", Zone);
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