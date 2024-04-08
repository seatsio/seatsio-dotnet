using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.Events;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Workspaces;

public class Workspaces
{
    public WorkspaceLister Active { get; }
    public WorkspaceLister Inactive { get; }

    private readonly RestClient _restClient;

    public Workspaces(RestClient restClient)
    {
        _restClient = restClient;
        Active = new WorkspaceLister(new PageFetcher<Workspace>(_restClient, "/workspaces/active"));
        Inactive = new WorkspaceLister(new PageFetcher<Workspace>(_restClient, "/workspaces/inactive"));
    }

    public async Task<Workspace> CreateAsync(string name, bool? isTest = null, CancellationToken cancellationToken = default)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        requestBody.Add("name", name);

        if (isTest != null)
        {
            requestBody.Add("isTest", isTest);
        }

        var restRequest = new RestRequest("/workspaces", Method.Post).AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<Workspace>(restRequest, cancellationToken));
    }

    public async Task<Workspace> RetrieveAsync(string key, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/workspaces/{key}", Method.Get)
            .AddUrlSegment("key", key);
        return AssertOk(await _restClient.ExecuteAsync<Workspace>(restRequest, cancellationToken));
    }

    public async Task UpdateAsync(string key, string name, CancellationToken cancellationToken = default)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        requestBody.Add("name", name);

        var restRequest = new RestRequest("/workspaces/{key}", Method.Post).AddJsonBody(requestBody)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task<string> RegenerateSecretKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/workspaces/{key}/actions/regenerate-secret-key", Method.Post)
            .AddUrlSegment("key", key);
        var response = AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest, cancellationToken));
        return response["secretKey"];
    }  
        
    public async Task ActivateAsync(string key, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/workspaces/{key}/actions/activate", Method.Post)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest, cancellationToken));
    }   
        
    public async Task DeactivateAsync(string key, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/workspaces/{key}/actions/deactivate", Method.Post)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest, cancellationToken));
    }  
        
    public async Task SetDefaultAsync(string key, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/workspaces/actions/set-default/{key}", Method.Post)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest, cancellationToken));
    }

    public IAsyncEnumerable<Workspace> ListAllAsync(string filter = null)
    {
        return ParametrizedList().AllAsync(filter);
    }

    public async Task<Page<Workspace>> ListFirstPageAsync(int? pageSize = null, string filter = null, CancellationToken cancellationToken = default)
    {
        return await ParametrizedList().FirstPageAsync(filter, pageSize, cancellationToken:cancellationToken);
    }

    public async Task<Page<Workspace>> ListPageAfterAsync(long id, int? pageSize = null, string filter = null, CancellationToken cancellationToken = default)
    {
        return await ParametrizedList().PageAfterAsync(id, filter, pageSize, cancellationToken:cancellationToken);
    }

    public async Task<Page<Workspace>> ListPageBeforeAsync(long id, int? pageSize = null, string filter = null, CancellationToken cancellationToken = default)
    {
        return await ParametrizedList().PageBeforeAsync(id, filter, pageSize, cancellationToken:cancellationToken);
    }

    private WorkspaceLister ParametrizedList()
    {
        return new WorkspaceLister(new PageFetcher<Workspace>(_restClient, "/workspaces"));
    }
}