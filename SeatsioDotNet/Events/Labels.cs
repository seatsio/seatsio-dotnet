namespace SeatsioDotNet.Events
{
    public class Labels
    {
        public string Own { get; set; }
        public string Row { get; set; }
        public string Table { get; set; }
        public string Section { get; set; }

        public override bool Equals(object obj)
        {
            Labels labels = (Labels) obj;
            return Own == labels.Own &&
                   Row == labels.Row &&
                   Table == labels.Table &&
                   Section == labels.Section;
        }
    }
}