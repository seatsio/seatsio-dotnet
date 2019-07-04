namespace SeatsioDotNet.UsageReports
{
    public class UsageMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }

        public UsageMonth()
        {
        }

        public UsageMonth(int year, int month)
        {
            Year = year;
            Month = month;
        }

        public string Serialize()
        {
            return Year + "-" + Month.ToString().PadLeft(2, '0');
        }
    }
}