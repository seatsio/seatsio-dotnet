using RestSharp;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.Charts
{
    public class ChartLister : Lister<Chart, ChartListParams>
    {
        public ChartLister(RestClient restClient) : base(CreatePageFetcher(restClient))
        {
        }

        private static PageFetcher<Chart> CreatePageFetcher(RestClient restClient)
        {
            return new PageFetcher<Chart>(restClient, "/charts");
        }
    }
}