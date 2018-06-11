namespace SeatsioDotNet.Events
{
    public class LabelAndType
    {
        public string Label { get; set; }
        public string Type { get; set; }

        public LabelAndType()
        {
        }
        
        public LabelAndType(string label = null, string type = null)
        {
            Label = label;
            Type = type;
        }

    }
}