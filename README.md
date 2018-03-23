# seatsio-dotnet, the official Seats.io .NET API client library

[![Build Status](https://travis-ci.org/seatsio/seatsio-dotnet.svg?branch=master)](https://travis-ci.org/seatsio/seatsio-dotnet)

The official Seats.io library, supporting .NET Standard 2.0+, .NET Core 2.0+ and .NET Framework 4.6.1+

## Installing seatsio-dotnet

From the command line:

	nuget install SeatsioDotNet

From Package Manager:

	PM> Install-Package SeatsioDotNet
	
Using the dotnet command:

    dotnet add package SeatsioDotNet
	
## Examples

### Creating a chart and an event

```csharp
var client = new SeatsioClient("<SECRET KEY>"); // can be found on https://app.seats.io/settings
var chart = client.Charts.Create();
var evnt = client.Events.Create(chart.Key);
```

### Booking objects

```csharp
var client = new SeatsioClient("<SECRET KEY>");
client.Events.Book(<EVENT KEY>, new [] { "A-1", "A-2"});
```

### Releasing objects

```csharp
var client = new SeatsioClient("<SECRET KEY>");
client.Events.Release(<EVENT KEY>, new [] { "A-1", "A-2"});
```

### Booking objects that have been held

```csharp
var client = new SeatsioClient("<SECRET KEY>");
client.Events.Book(<EVENT KEY>, new [] { "A-1", "A-2"}, <A HOLD TOKEN>);
```

### Changing object status

```csharp
var client = new SeatsioClient("<SECRET KEY>");
client.Events.ChangeObjectStatus(""<EVENT KEY>"", new [] { "A-1", "A-2"}, "unavailable");
```

### Listing all charts

```csharp
var client = new SeatsioClient("<SECRET KEY>");
var charts = client.Charts.ListAll(); // returns an IEnumerable<Chart>
```

### Retrieving the published version of a chart (i.e. the actual drawing, containing the venue type, categories etc.)

```csharp
var client = new SeatsioClient("<SECRET KEY>");
var drawing = client.Charts.RetrievePublishedVersion(<CHART KEY>);
Console.WriteLine(drawing.VenueType);
```

### Listing the first page of charts (default page size is 20)

```csharp
var client = new SeatsioClient("<SECRET KEY>");
var charts = client.Charts.ListFirstPage(); // returns a Page<Chart>
```

## Error handling

When an API call results in a 4xx or 5xx error (e.g. when a chart could not be found), a SeatsioException is thrown.

This exception contains a message string describing what went wrong, and also two other properties:

- `Messages`: a list of error messages that the server returned. In most cases, this list will contain only one element.
- `RequestId`: the identifier of the request you made. Please mention this to us when you have questions, as it will make debugging easier.
