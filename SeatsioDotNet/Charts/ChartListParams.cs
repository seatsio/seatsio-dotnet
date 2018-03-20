using SeatsioDotNet.Util;

namespace SeatsioDotNet.Charts
{
    public class ChartListParams : ListParams
    {
        public ChartListParams SetFilter(string filter)
        {
            _params.Add("filter", filter);
            return this;
        }

        public ChartListParams SetTag(string tag)
        {
            _params.Add("tag", tag);
            return this;
        }

        public ChartListParams SetExpandEvents()
        {
            _params.Add("expand", "events");
            return this;
        }
    }
}