# seatsio-dotnet, the official Seats.io .NET API client library

[![Build](https://github.com/seatsio/seatsio-dotnet/workflows/Build/badge.svg)](https://github.com/seatsio/seatsio-dotnet/actions/workflows/build.yml)
[![NuGet version](https://badge.fury.io/nu/seatsiodotnet.svg)](https://badge.fury.io/nu/seatsiodotnet)

The official Seats.io library, supporting .NET 6 and newer.

## Installing seatsio-dotnet

From the command line:

	nuget install SeatsioDotNet

From Package Manager:

	PM> Install-Package SeatsioDotNet
	
Using the dotnet command:

    dotnet add package SeatsioDotNet

From within Visual Studio:

- Open the Solution Explorer.
- Right-click on a project within your solution.
- Click on Manage NuGet Packages...
- Click on the Browse tab and search for "SeatsioDotNet". 
- Install the package.

This is the homepage of our NuGet package:

https://www.nuget.org/packages/SeatsioDotNet/
    
## Versioning

seatsio-dotnet follows semver since v70.2.0.
	
## Usage

### General instructions

To use this library, you'll need to create a `SeatsioClient`:

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
...
```

You can find your _workspace secret key_ in the [settings section of the workspace](https://app.seats.io/workspace-settings).

The region should correspond to the region of your account:

- `Region.EU`: Europe
- `Region.NA`: North-America
- `Region.SA`: South-America
- `Region.OC`: Oceania

If you're unsure about your region, have a look at your [company settings page](https://app.seats.io/company-settings).

### Creating a chart and an event

```csharp
using SeatsioDotNet;
using SeatsioDotNet.Charts;
using SeatsioDotNet.Events;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
var chart = client.Charts.Create();
var evnt = client.Events.Create(chart.Key);
```

### Booking objects

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
client.Events.Book(<EVENT KEY>, new [] { "A-1", "A-2"});
```

### Releasing objects

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
client.Events.Release(<EVENT KEY>, new [] { "A-1", "A-2"});
```

### Booking objects that have been held

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
client.Events.Book(<EVENT KEY>, new [] { "A-1", "A-2"}, <A HOLD TOKEN>);
```

### Changing object status

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
client.Events.ChangeObjectStatus(""<EVENT KEY>"", new [] { "A-1", "A-2"}, "unavailable");
```

### Retrieving the published version of a chart (i.e. the actual drawing, containing the venue type, categories etc.)

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
var drawing = client.Charts.RetrievePublishedVersion(<CHART KEY>);
Console.WriteLine(drawing.VenueType);
```

### Retrieving object category and status (and other information)

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
var objectInfos = Client.Events.RetrieveObjectInfos(evnt.Key, new string[] {"A-1", "A-2"});

Console.WriteLine(objectInfos["A-1"].CategoryKey);
Console.WriteLine(objectInfos["A-1"].CategoryLabel);
Console.WriteLine(objectInfos["A-1"].Status);

Console.WriteLine(objectInfos["A-2"].CategoryKey);
Console.WriteLine(objectInfos["A-2"].CategoryLabel);
Console.WriteLine(objectInfos["A-2"].Status);
```

### Listing a chart's categories

```csharp
var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");
IEnumerable<Category> categoryList = client.Charts.ListCategories(<chart key>);
foreach (var category in categoryList)
{
    Console.Write(category.Label);
}
```

### Listing all charts

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>");

var charts = client.Charts.ListAll();
foreach (var chart in charts)
{
  Console.WriteLine("Chart " + chart.Key);
}
```

Note: `listAll()` returns an IEnumerable`, which under the hood calls the seats.io API to fetch charts page by page. So multiple API calls may be done underneath to fetch all charts.

### Listing charts page by page

E.g. to show charts in a paginated list on a dashboard.

```csharp
// ... user initially opens the screen ...

var firstPage = client.Charts.ListFirstPage();
foreach (var chart in firstPage.Items)
{
  Console.WriteLine("Chart " + chart.Key);
}
```

```csharp
// ... user clicks on 'next page' button ...

var nextPage = client.Charts.ListPageAfter(firstPage.NextPageStartsAfter);
foreach (var chart in nextPage.Items)
{
  Console.WriteLine("Chart " + chart.Key);
}
```

```csharp
// ... user clicks on 'previous page' button ...

var previousPage = client.Charts.ListPageBefore(nextPage.PreviousPageEndsBefore);
foreach (var chart in previousPage.Items)
{
  Console.WriteLine("Chart " + chart.Key);
}
```

### Creating a workspace

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<COMPANY ADMIN KEY>");
client.Workspaces.Create("a workspace");
```

### Creating a chart and an event with the company admin key

```csharp
using SeatsioDotNet;

var client = new SeatsioClient(Region.EU, "<COMPANY ADMIN KEY>", "<WORKSPACE PUBLIC KEY>"); // workspace public key can be found on https://app.seats.io/workspace-settings
var chart = client.Charts.Create();
var evnt = client.Events.Create(chart.Key);
```

## Error handling

When an API call results in a 4xx or 5xx error (e.g. when a chart could not be found), a SeatsioException is thrown.

This exception contains a message string describing what went wrong, and also two other properties:

- `Errors`: a list of errors that the server returned. In most cases, this list will contain only one element.
- `RequestId`: the identifier of the request you made. Please mention this to us when you have questions, as it will make debugging easier.


## Rate limiting - exponential backoff

This library supports [exponential backoff](https://en.wikipedia.org/wiki/Exponential_backoff).

When you send too many concurrent requests, the server returns an error `429 - Too Many Requests`. The client reacts to this by waiting for a while, and then retrying the request.
If the request still fails with an error `429`, it waits a little longer, and try again. By default this happens 5 times, before giving up (after approximately 15 seconds).

We throw a `RateLimitExceededException` (which is a subclass of `SeatsioException`) when exponential backoff eventually fails.

To change the maximum number of retries, create the `SeatsioClient` as follows:

```csharp
var client = new SeatsioClient(Region.EU, "<WORKSPACE SECRET KEY>").SetMaxRetries(3);
```

Passing in 0 disables exponential backoff completely. In that case, the client will never retry a failed request.
