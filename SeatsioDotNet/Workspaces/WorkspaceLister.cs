using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SeatsioDotNet.Events;
using SeatsioDotNet.Util;

namespace SeatsioDotNet.Workspaces;

public class WorkspaceLister
{
    private readonly PageFetcher<Workspace> _pageFetcher;

    public WorkspaceLister(PageFetcher<Workspace> pageFetcher)
    {
        _pageFetcher = pageFetcher;
    }

    public IAsyncEnumerable<Workspace> AllAsync(string filter = null)
    {
        return new PagedEnumerable<Workspace>(_pageFetcher, WorkspaceListParams(filter));
    }

    public async Task<Page<Workspace>> FirstPageAsync(string filter = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchFirstPageAsync(WorkspaceListParams(filter), pageSize, cancellationToken: cancellationToken);
    }

    public async Task<Page<Workspace>> PageAfterAsync(long id, string filter = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchAfterAsync(id, WorkspaceListParams(filter), pageSize, cancellationToken: cancellationToken);
    }

    public async Task<Page<Workspace>> PageBeforeAsync(long id, string filter = null, int? pageSize = null, CancellationToken cancellationToken = default)
    {
        return await _pageFetcher.FetchBeforeAsync(id, WorkspaceListParams(filter), pageSize, cancellationToken: cancellationToken);
    }

    private static Dictionary<string, object> WorkspaceListParams(string filter)
    {
        var workspaceListParams = new Dictionary<string, object>();

        if (filter != null)
        {
            workspaceListParams.Add("filter", filter);
        }

        return workspaceListParams;
    }
}