using System.Collections.Generic;

namespace SeatsioDotNet.Util
{
    public class ParametrizedLister<T>
    {
        private readonly PageFetcher<T> _pageFetcher;

        public ParametrizedLister(PageFetcher<T> pageFetcher)
        {
            _pageFetcher = pageFetcher;
        }

        public IEnumerable<T> All(Dictionary<string, object> listParams = null)
        {
            return new PagedEnumerable<T>(_pageFetcher, listParams);
        }

        public Page<T> FirstPage(Dictionary<string, object> listParams = null, int? pageSize = null)
        {
            return _pageFetcher.FetchFirstPage(listParams, pageSize);
        }

        public Page<T> PageAfter(long id, Dictionary<string, object> listParams = null, int? pageSize = null)
        {
            return _pageFetcher.FetchAfter(id, listParams, pageSize);
        }

        public Page<T> PageBefore(long id, Dictionary<string, object> listParams = null, int? pageSize = null)
        {
            return _pageFetcher.FetchBefore(id, listParams, pageSize);
        }
    }
}