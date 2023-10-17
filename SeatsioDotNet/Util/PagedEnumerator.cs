using System.Collections;
using System.Collections.Generic;

namespace SeatsioDotNet.Util;

public class PagedEnumerator<T> : IEnumerator<T>
{
    private Page<T> _currentPage;
    private int _indexInCurrentPage;
        
    private readonly PageFetcher<T> _pageFetcher;
    private readonly Dictionary<string, object> _listParams;

    public PagedEnumerator(PageFetcher<T> pageFetcher, Dictionary<string, object> listParams)
    {
        _pageFetcher = pageFetcher;
        _listParams = listParams;
    }

    public bool MoveNext()
    {
        if (_currentPage == null)
        {
            _currentPage = _pageFetcher.FetchFirstPage(_listParams, null);
        }
        else if (NextPageMustBeFetched())
        {
            _currentPage = _pageFetcher.FetchAfter(_currentPage.NextPageStartsAfter.Value, _listParams, null);
            _indexInCurrentPage = 0;
        }
        else
        {
            _indexInCurrentPage += 1;
        }

        return _indexInCurrentPage < _currentPage.Items.Count;
    }

    private bool NextPageMustBeFetched()
    {
        return _indexInCurrentPage + 1 >= _currentPage.Items.Count &&
               _currentPage.NextPageStartsAfter != null;
    }

    public void Reset()
    {
        _currentPage = null;
        _indexInCurrentPage = 0;
    }

    public T Current => _currentPage.Items[_indexInCurrentPage];

    object IEnumerator.Current => Current;

    public void Dispose()
    {
    }
}