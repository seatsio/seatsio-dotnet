using System.Collections.Generic;

namespace SeatsioDotNet.Events;

public class ObjectAndQuantity
{
    public string ObjectLabel { get; }
    public int? Quantity { get; }

    public ObjectAndQuantity(string objectLabel)
    {
        ObjectLabel = objectLabel;
    }

    public ObjectAndQuantity(string objectLabel, int quantity)
    {
        ObjectLabel = objectLabel;
        Quantity = quantity;
    }

    public Dictionary<string, object> AsDictionary()
    {
        var dictionary = new Dictionary<string, object> {{"object", ObjectLabel}};

        if (Quantity != null)
        {
            dictionary.Add("quantity", Quantity);
        }

        return dictionary;
    }
}