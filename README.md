# seatsio-dotnet, the official Seats.io .NET API client library

[![Build Status](https://travis-ci.org/seatsio/seatsio-dotnet.svg?branch=master)](https://travis-ci.org/seatsio/seatsio-dotnet)
[![NuGet](https://img.shields.io/nuget/v/SeatsioDotNet.svg)](https://www.nuget.org/packages/SeatsioDotNet/)

The official Seats.io library, supporting .NET Standard 2.0+ and .NET Core 2.0+

## Installing seatsio-dotnet

From the command line:

	nuget install SeatsioDotNet

From Package Manager:

	PM> Install-Package SeatsioDotNet
	
Using the dotnet command:

    dotnet add package SeatsioDotNet
    
## Versioning

seatsio-dotnet follows semver since v70.2.0.
	
## Examples

### Creating a chart and an event

```csharp
var client = new SeatsioClient("<WORKSPACE SECRET KEY>"); // can be found on https://app.seats.io/workspace-settings
var chart = client.Charts.Create();
var evnt = client.Events.Create(chart.Key);
```

### Booking objects

```csharp
var client = new SeatsioClient("<WORKSPACE SECRET KEY>");
client.Events.Book(<EVENT KEY>, new [] { "A-1", "A-2"});
```

### Releasing objects

```csharp
var client = new SeatsioClient("<WORKSPACE SECRET KEY>");
client.Events.Release(<EVENT KEY>, new [] { "A-1", "A-2"});
```

### Booking objects that have been held

```csharp
var client = new SeatsioClient("<WORKSPACE SECRET KEY>");
client.Events.Book(<EVENT KEY>, new [] { "A-1", "A-2"}, <A HOLD TOKEN>);
```

### Changing object status

```csharp
var client = new SeatsioClient("<WORKSPACE SECRET KEY>");
client.Events.ChangeObjectStatus(""<EVENT KEY>"", new [] { "A-1", "A-2"}, "unavailable");
```

### Retrieving the published version of a chart (i.e. the actual drawing, containing the venue type, categories etc.)

```csharp
var client = new SeatsioClient("<WORKSPACE SECRET KEY>");
var drawing = client.Charts.RetrievePublishedVersion(<CHART KEY>);
Console.WriteLine(drawing.VenueType);
```

### Listing all charts

```csharp
var client = new SeatsioClient("<WORKSPACE SECRET KEY>");

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
var client = new SeatsioClient("<COMPANY ADMIN KEY>");
client.Workspaces.Create("a workspace");
```

## Error handling

When an API call results in a 4xx or 5xx error (e.g. when a chart could not be found), a SeatsioException is thrown.

This exception contains a message string describing what went wrong, and also two other properties:

- `Errors`: a list of errors that the server returned. In most cases, this list will contain only one element.
- `RequestId`: the identifier of the request you made. Please mention this to us when you have questions, as it will make debugging easier.
