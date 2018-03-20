using System.Collections.Generic;

namespace SeatsioDotNet.Util
{
    public class Lister<T, TListParams> where TListParams : ListParams
    {
        private readonly PageFetcher<T> _pageFetcher;

        public Lister(PageFetcher<T> pageFetcher)
        {
            _pageFetcher = pageFetcher;
        }

        public IEnumerable<T> All(TListParams listParams = null)
        {
            return new PagedEnumerable<T>(_pageFetcher, listParams);
        }

        public Page<T> FirstPage(TListParams listParams = null)
        {
            return _pageFetcher.FetchFirstPage(listParams);
        }

        public Page<T> PageAfter(long id, TListParams listParams = null)
        {
            return _pageFetcher.FetchAfter(id, listParams);
        }

        public Page<T> PageBefore(long id, TListParams listParams = null)
        {
            return _pageFetcher.FetchBefore(id, listParams);
        }
    }
}