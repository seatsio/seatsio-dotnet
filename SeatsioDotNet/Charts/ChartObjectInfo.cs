using SeatsioDotNet.Events;

namespace SeatsioDotNet.ChartReports
{
    public class ChartObjectInfo
    {
        public string Label { get; set; }
        public Labels Labels { get; set; }
        public IDs IDs { get; set; }
        public string CategoryLabel { get; set; }
        public int? CategoryKey { get; set; }
        public string ObjectType { get; set; }
        public string Section { get; set; }
        public string Entrance { get; set; }
        public int? Capacity { get; set; }
        public bool? BookAsAWhole { get; set; }
        public string LeftNeighbour { get; set; }
        public string RightNeighbour { get; set; }
        public float? DistanceToFocalPoint { get; set; }
    }
}
