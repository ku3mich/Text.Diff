#!/usr/bin/env pwsh
dotnet pack --configuration Release -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -o ./bin/pkgs Text.Diff/Text.Diff.csproj 
