namespace SeatsioDotNet.Reports.Usage
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

        public bool Before(int year, int month)
        {
            if (Year > year)
            {
                return false;
            }

            if (Year == year)
            {
                return Month < month;
            }

            return true;
        }
    }
}