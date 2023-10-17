using System.Collections.Generic;
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

    public IEnumerable<Workspace> All(string filter = null)
    {
        return new PagedEnumerable<Workspace>(_pageFetcher, WorkspaceListParams(filter));
    }

    public Page<Workspace> FirstPage(string filter = null, int? pageSize = null)
    {
        return _pageFetcher.FetchFirstPage(WorkspaceListParams(filter), pageSize);
    }

    public Page<Workspace> PageAfter(long id, string filter = null, int? pageSize = null)
    {
        return _pageFetcher.FetchAfter(id, WorkspaceListParams(filter), pageSize);
    }

    public Page<Workspace> PageBefore(long id, string filter = null, int? pageSize = null)
    {
        return _pageFetcher.FetchBefore(id, WorkspaceListParams(filter), pageSize);
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