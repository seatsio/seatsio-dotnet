namespace SeatsioDotNet;

public class Region
{
    private readonly string _id;

    private Region(string id)
    {
        _id = id;
    }

    public static Region EU()
    {
        return new Region("eu");
    }

    public static Region NA()
    {
        return new Region("na");
    }

    public static Region SA()
    {
        return new Region("sa");
    }

    public static Region OC()
    {
        return new Region("oc");
    }

    public string Url()
    {
        return "https://api-{region}.seatsio.net".Replace("{region}", _id);
    }
}