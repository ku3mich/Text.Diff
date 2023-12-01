#!/usr/bin/env pwsh

dotnet build --configuration Release TextDiff/TextDiff.csproj 
./run-pack.ps1

function Push-Package {
  param($pkg)
	nuget push $pkg -source local
}

Get-ChildItem bin/pkgs -Filter '*.nupkg' | % { Push-Package $_.FullName	}
