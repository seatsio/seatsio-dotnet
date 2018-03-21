How to deploy:

1) cd SeatsioDotNet
1) increase version number in SeatsioDotNet.csproj
2) dotnet pack
3) dotnet nuget push bin/Debug/seatsio-dotnet.<version>.nupkg -s nuget.org -k <nuget API key>
