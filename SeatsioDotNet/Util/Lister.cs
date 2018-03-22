using System.Collections.Generic;

namespace SeatsioDotNet.Util
{
    public class Lister<T>
    {
        private readonly PageFetcher<T> _pageFetcher;

        public Lister(PageFetcher<T> pageFetcher)
        {
            _pageFetcher = pageFetcher;
        }

        public IEnumerable<T> All(Dictionary<string, object> listParams = null)
        {
            return new PagedEnumerable<T>(_pageFetcher, listParams);
        }

        public Page<T> FirstPage(int? pageSize = null)
        {
            return _pageFetcher.FetchFirstPage(pageSize: pageSize);
        }

        public Page<T> PageAfter(long id, int? pageSize = null)
        {
            return _pageFetcher.FetchAfter(id, pageSize: pageSize);
        }

        public Page<T> PageBefore(long id, int? pageSize = null)
        {
            return _pageFetcher.FetchBefore(id, pageSize: pageSize);
        }
    }
}