using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SeatsioDotNet.Events;

public class ForSaleAndNotForSale
{
    public ObjectAndQuantity[] ForSale { get; }
    public ObjectAndQuantity[] NotForSale { get; }

    public ForSaleAndNotForSale(ObjectAndQuantity[] forSale, ObjectAndQuantity[] notForSale)
    {
        ForSale = forSale;
        NotForSale = notForSale;
    }

    public Dictionary<string, object> AsDictionary()
    {
        var dictionary = new Dictionary<string, object> { };

        if (ForSale != null)
        {
            dictionary.Add("forSale", ForSale.Select(e => e.AsDictionary()));
        }   
        
        if (NotForSale != null)
        {
            dictionary.Add("notForSale", NotForSale.Select(e => e.AsDictionary()));
        }
        
        return dictionary;
    }
}