using System.Collections.Generic;
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

    public async Task<Workspace> CreateAsync(string name, bool? isTest = null)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        requestBody.Add("name", name);

        if (isTest != null)
        {
            requestBody.Add("isTest", isTest);
        }

        var restRequest = new RestRequest("/workspaces", Method.Post).AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<Workspace>(restRequest));
    }

    public async Task<Workspace> RetrieveAsync(string key)
    {
        var restRequest = new RestRequest("/workspaces/{key}", Method.Get)
            .AddUrlSegment("key", key);
        return AssertOk(await _restClient.ExecuteAsync<Workspace>(restRequest));
    }

    public async Task UpdateAsync(string key, string name)
    {
        Dictionary<string, object> requestBody = new Dictionary<string, object>();
        requestBody.Add("name", name);

        var restRequest = new RestRequest("/workspaces/{key}", Method.Post).AddJsonBody(requestBody)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest));
    }

    public async Task<string> RegenerateSecretKeyAsync(string key)
    {
        var restRequest = new RestRequest("/workspaces/{key}/actions/regenerate-secret-key", Method.Post)
            .AddUrlSegment("key", key);
        var response = AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest));
        return response["secretKey"];
    }  
        
    public async Task ActivateAsync(string key)
    {
        var restRequest = new RestRequest("/workspaces/{key}/actions/activate", Method.Post)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest));
    }   
        
    public async Task DeactivateAsync(string key)
    {
        var restRequest = new RestRequest("/workspaces/{key}/actions/deactivate", Method.Post)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest));
    }  
        
    public async Task SetDefaultAsync(string key)
    {
        var restRequest = new RestRequest("/workspaces/actions/set-default/{key}", Method.Post)
            .AddUrlSegment("key", key);
        AssertOk(await _restClient.ExecuteAsync<Dictionary<string, string>>(restRequest));
    }

    public IAsyncEnumerable<Workspace> ListAllAsync(string filter = null)
    {
        return ParametrizedList().All(filter);
    }

    public async Task<Page<Workspace>> ListFirstPageAsync(int? pageSize = null, string filter = null)
    {
        return await ParametrizedList().FirstPage(filter, pageSize);
    }

    public async Task<Page<Workspace>> ListPageAfterAsync(long id, int? pageSize = null, string filter = null)
    {
        return await ParametrizedList().PageAfter(id, filter, pageSize);
    }

    public async Task<Page<Workspace>> ListPageBeforeAsync(long id, int? pageSize = null, string filter = null)
    {
        return await ParametrizedList().PageBefore(id, filter, pageSize);
    }

    private WorkspaceLister ParametrizedList()
    {
        return new WorkspaceLister(new PageFetcher<Workspace>(_restClient, "/workspaces"));
    }
}