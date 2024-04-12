using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using SeatsioDotNet.Util;
using static SeatsioDotNet.Util.RestUtil;

namespace SeatsioDotNet.Charts;

public class Charts
{
    public Lister<Chart> Archive { get; }

    private readonly RestClient _restClient;

    public Charts(RestClient restClient)
    {
        _restClient = restClient;
        Archive = new Lister<Chart>(new PageFetcher<Chart>(_restClient, "/charts/archive"));
    }

    public async Task<Chart> CreateAsync(string name = null, string venueType = null, IEnumerable<Category> categories = null, CancellationToken cancellationToken = default)
    {
        var requestBody = new Dictionary<string, object>();

        if (name != null)
        {
            requestBody.Add("name", name);
        }

        if (venueType != null)
        {
            requestBody.Add("venueType", venueType);
        }

        if (categories != null)
        {
            requestBody.Add("categories", categories.Select(c => c.AsDictionary()));
        }

        var restRequest = new RestRequest("/charts", Method.Post)
            .AddJsonBody(requestBody);
        return AssertOk(await _restClient.ExecuteAsync<Chart>(restRequest, cancellationToken));
    }

    public async Task UpdateAsync(string chartKey, string name = null, IEnumerable<Category> categories = null, CancellationToken cancellationToken = default)
    {
        var requestBody = new Dictionary<string, object>();

        if (name != null)
        {
            requestBody.Add("name", name);
        }

        if (categories != null)
        {
            requestBody.Add("categories", categories.Select(c => c.AsDictionary()));
        }

        var restRequest = new RestRequest("/charts/{key}", Method.Post)
            .AddUrlSegment("key", chartKey)
            .AddJsonBody(requestBody);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task AddCategoryAsync(string chartKey, Category category, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{chartKey}/categories", Method.Post)
            .AddUrlSegment("chartKey", chartKey)
            .AddJsonBody(category.AsDictionary());
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task RemoveCategoryAsync(string chartKey, object categoryKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{chartKey}/categories/{categoryKey}", Method.Delete)
            .AddUrlSegment("chartKey", chartKey)
            .AddUrlSegment("categoryKey", categoryKey.ToString());
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task<IEnumerable<Category>> ListCategoriesAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest($"/charts/{chartKey}/categories", Method.Get);
        return (await _restClient.ExecuteAsync<CategoryList>(restRequest, cancellationToken)).Data.List;
    }

    private class CategoryList
    {
        [JsonPropertyName("categories")] public IEnumerable<Category> List { get; set; }
    }

    public async Task<Chart> CopyAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/published/actions/copy", Method.Post)
            .AddUrlSegment("key", chartKey);
        return AssertOk(await _restClient.ExecuteAsync<Chart>(restRequest, cancellationToken));
    }

    public async Task<Chart> CopyToWorkspaceAsync(string chartKey, string toWorkspaceKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/published/actions/copy-to-workspace/{toWorkspaceKey}", Method.Post)
            .AddUrlSegment("key", chartKey)
            .AddUrlSegment("toWorkspaceKey", toWorkspaceKey);
        return AssertOk(await _restClient.ExecuteAsync<Chart>(restRequest, cancellationToken));
    }

    public async Task<Chart> CopyToWorkspaceAsync(string chartKey, string fromWorkspaceKey, string toWorkspaceKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest($"/charts/{chartKey}/version/published/actions/copy/from/{fromWorkspaceKey}/to/{toWorkspaceKey}", Method.Post)
            .AddUrlSegment("chartKey", chartKey)
            .AddUrlSegment("fromWorkspaceKey", fromWorkspaceKey)
            .AddUrlSegment("toWorkspaceKey", toWorkspaceKey);
        return AssertOk(await _restClient.ExecuteAsync<Chart>(restRequest, cancellationToken));
    }

    public async Task<Chart> CopyDraftVersionAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/draft/actions/copy", Method.Post)
            .AddUrlSegment("key", chartKey);
        return AssertOk(await _restClient.ExecuteAsync<Chart>(restRequest, cancellationToken));
    }

    public async Task AddTagAsync(string chartKey, string tag, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/tags/{tag}", Method.Post)
            .AddUrlSegment("key", chartKey)
            .AddUrlSegment("tag", tag);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task RemoveTagAsync(string chartKey, string tag, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/tags/{tag}", Method.Delete)
            .AddUrlSegment("key", chartKey)
            .AddUrlSegment("tag", tag);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task<Chart> RetrieveAsync(string chartKey, bool? expandEvents = null, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}", Method.Get)
            .AddUrlSegment("key", chartKey);

        if (expandEvents != null && expandEvents.Value)
        {
            restRequest.AddQueryParameter("expand", "events");
        }

        return AssertOk(await _restClient.ExecuteAsync<Chart>(restRequest, cancellationToken));
    }

    public async Task<IEnumerable<string>> ListAllTagsAsync(CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/tags", Method.Get);
        return AssertOk(await _restClient.ExecuteAsync<Tags>(restRequest, cancellationToken)).List;
    }

    private class Tags
    {
        [JsonPropertyName("tags")] public IEnumerable<string> List { get; set; }
    }

    public async Task MoveToArchiveAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/actions/move-to-archive", Method.Post)
            .AddUrlSegment("key", chartKey);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task MoveOutOfArchiveAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/actions/move-out-of-archive", Method.Post)
            .AddUrlSegment("key", chartKey);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task<Drawing> RetrievePublishedVersionAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/published", Method.Get)
            .AddUrlSegment("key", chartKey);
        return AssertOk(await _restClient.ExecuteAsync<Drawing>(restRequest, cancellationToken));
    }

    public async Task<byte[]> RetrievePublishedVersionThumbnailAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/published/thumbnail", Method.Get)
            .AddUrlSegment("key", chartKey);
        var restResponse = await _restClient.ExecuteAsync<object>(restRequest, cancellationToken);
        AssertOk(restResponse);
        return restResponse.RawBytes;
    }

    public async Task<Drawing> RetrieveDraftVersionAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/draft", Method.Get)
            .AddUrlSegment("key", chartKey);
        return AssertOk(await _restClient.ExecuteAsync<Drawing>(restRequest, cancellationToken));
    }

    public async Task<byte[]> RetrieveDraftVersionThumbnailAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/draft/thumbnail", Method.Get)
            .AddUrlSegment("key", chartKey);
        var restResponse = await _restClient.ExecuteAsync<object>(restRequest, cancellationToken);
        AssertOk(restResponse);
        return restResponse.RawBytes;
    }

    public async Task PublishDraftVersionAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/draft/actions/publish", Method.Post)
            .AddUrlSegment("key", chartKey);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task DiscardDraftVersionAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/draft/actions/discard", Method.Post)
            .AddUrlSegment("key", chartKey);
        AssertOk(await _restClient.ExecuteAsync<object>(restRequest, cancellationToken));
    }

    public async Task<ChartValidationResult> ValidatePublishedVersionAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/published/actions/validate", Method.Post)
            .AddUrlSegment("key", chartKey);
        return AssertOk(await _restClient.ExecuteAsync<ChartValidationResult>(restRequest, cancellationToken));
    }

    public async Task<ChartValidationResult> ValidateDraftVersionAsync(string chartKey, CancellationToken cancellationToken = default)
    {
        var restRequest = new RestRequest("/charts/{key}/version/draft/actions/validate", Method.Post)
            .AddUrlSegment("key", chartKey);
        return AssertOk(await _restClient.ExecuteAsync<ChartValidationResult>(restRequest, cancellationToken));
    }

    public IAsyncEnumerable<Chart> ListAllAsync(string filter = null, string tag = null, bool? expandEvents = null, bool? withValidation = false)
    {
        return List().AllAsync(ChartListParams(filter, tag, expandEvents, withValidation));
    }

    public async Task<Page<Chart>> ListFirstPageAsync(string filter = null, string tag = null, bool? expandEvents = false, int? pageSize = null, bool? withValidation = false, CancellationToken cancellationToken = default)
    {
        return await List().FirstPageAsync(ChartListParams(filter, tag, expandEvents, withValidation), pageSize, cancellationToken);
    }

    public async Task<Page<Chart>> ListPageAfterAsync(long id, string filter = null, string tag = null, bool? expandEvents = false, int? pageSize = null, bool? withValidation = false, CancellationToken cancellationToken = default)
    {
        return await List().PageAfterAsync(id, ChartListParams(filter, tag, expandEvents, withValidation), pageSize, cancellationToken);
    }

    public async Task<Page<Chart>> ListPageBeforeAsync(long id, string filter = null, string tag = null, bool? expandEvents = false, int? pageSize = null, bool? withValidation = false, CancellationToken cancellationToken = default)
    {
        return await List().PageBeforeAsync(id, ChartListParams(filter, tag, expandEvents, withValidation), pageSize, cancellationToken);
    }

    private Dictionary<string, object> ChartListParams(string filter, string tag, bool? expandEvents, bool? withValidation = false)
    {
        var chartListParams = new Dictionary<string, object>();

        if (filter != null)
        {
            chartListParams.Add("filter", filter);
        }

        if (tag != null)
        {
            chartListParams.Add("tag", tag);
        }

        if (expandEvents != null && expandEvents.Value)
        {
            chartListParams.Add("expand", "events");
        }

        if (withValidation == true)
        {
            chartListParams.Add("validation", true);
        }

        return chartListParams;
    }

    private ParametrizedLister<Chart> List()
    {
        return new ParametrizedLister<Chart>(new PageFetcher<Chart>(_restClient, "/charts"));
    }
}