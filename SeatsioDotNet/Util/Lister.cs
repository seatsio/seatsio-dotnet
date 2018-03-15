using System.Collections.Generic;
using RestSharp;
using SeatsioDotNet.Subaccounts;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Util
{
    public class Lister<T>
    {
        private readonly PageFetcher<T> _pageFetcher;

        public Lister(PageFetcher<T> pageFetcher)
        {
            _pageFetcher = pageFetcher;
        }

        public IEnumerable<T> All()
        {
            return All(new ListParams());
        }

        public IEnumerable<T> All(ListParams listParams)
        {
            return new PagedEnumerable<T>(_pageFetcher, listParams);
        }
    }
}