using System.Collections.Generic;

namespace SeatsioDotNet.Events;

public class ObjectProperties
{
    public string ObjectLabel { get; }
    public int? Quantity { get; }
    public string TicketType { get; }
    public Dictionary<string, object> ExtraData { get; }

    public ObjectProperties(string objectLabel)
    {
        ObjectLabel = objectLabel;
    }

    public ObjectProperties(string objectLabel, string ticketType)
    {
        ObjectLabel = objectLabel;
        TicketType = ticketType;
    }

    public ObjectProperties(string objectLabel, string ticketType, Dictionary<string, object> extraData)
    {
        ObjectLabel = objectLabel;
        TicketType = ticketType;
        ExtraData = extraData;
    }

    public ObjectProperties(string objectLabel, Dictionary<string, object> extraData)
    {
        ObjectLabel = objectLabel;
        ExtraData = extraData;
    }

    public ObjectProperties(string objectLabel, int quantity)
    {
        ObjectLabel = objectLabel;
        Quantity = quantity;
    }

    public Dictionary<string, object> AsDictionary()
    {
        var dictionary = new Dictionary<string, object> {{"objectId", ObjectLabel}};
        if (TicketType != null)
        {
            dictionary.Add("ticketType", TicketType);
        }

        if (Quantity != null)
        {
            dictionary.Add("quantity", Quantity);
        }

        if (ExtraData != null)
        {
            dictionary.Add("extraData", ExtraData);
        }

        return dictionary;
    }
}