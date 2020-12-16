namespace SeatsioDotNet
{
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

        public static Region US()
        {
            return new Region("us");
        }

        public string Url()
        {
            return "https://api-{region}.seatsio.net".Replace("{region}", _id);
        }
    }
}