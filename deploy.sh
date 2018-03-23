#!/bin/bash
set -e

if [ "$#" -ne 1 ]; then
  echo "Error: please pass in your nuget.org API key"
  exit 1
fi

fileName=(`ls -t SeatsioDotNet/bin/Debug/*.nupkg | head -1`)
dotnet nuget push $fileName -s nuget.org -k $1
