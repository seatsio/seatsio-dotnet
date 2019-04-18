using System.Collections.Generic;

namespace SeatsioDotNet.Charts
{
    public class ChartValidationResult
    {
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }
    }
}