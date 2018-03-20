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

        public IEnumerable<T> All()
        {
            return All(new ListParams());
        }

        public IEnumerable<T> All(ListParams listParams)
        {
            return new PagedEnumerable<T>(_pageFetcher, listParams);
        }

        public Page<T> FirstPage()
        {
            return FirstPage(new ListParams());
        }

        public Page<T> FirstPage(ListParams listParams)
        {
            return _pageFetcher.FetchFirstPage(listParams);
        }

        public Page<T> PageAfter(long id)
        {
            return PageAfter(id, new ListParams());
        }

        public Page<T> PageAfter(long id, ListParams listParams)
        {
            return _pageFetcher.FetchAfter(id, listParams);
        }

        public Page<T> PageBefore(long id)
        {
            return PageBefore(id, new ListParams());
        }

        public Page<T> PageBefore(long id, ListParams listParams)
        {
            return _pageFetcher.FetchBefore(id, listParams);
        }
    }
}