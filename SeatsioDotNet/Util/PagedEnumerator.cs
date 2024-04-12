using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SeatsioDotNet.Util;

public class PagedEnumerator<T> : IAsyncEnumerator<T>
{
    private Page<T> _currentPage;
    private int _indexInCurrentPage;

    private readonly PageFetcher<T> _pageFetcher;
    private readonly Dictionary<string, object> _listParams;
    private readonly CancellationToken _cancellationToken;

    public PagedEnumerator(PageFetcher<T> pageFetcher, Dictionary<string, object> listParams, CancellationToken cancellationToken = default)
    {
        _pageFetcher = pageFetcher;
        _listParams = listParams;
        _cancellationToken = cancellationToken;
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        if (_currentPage == null)
        {
            _currentPage = await _pageFetcher.FetchFirstPageAsync(_listParams, null, cancellationToken: _cancellationToken);
        }
        else if (NextPageMustBeFetched())
        {
            _currentPage = await _pageFetcher.FetchAfterAsync(_currentPage.NextPageStartsAfter.Value, _listParams, null, cancellationToken: _cancellationToken);
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

    T IAsyncEnumerator<T>.Current => Current;

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}