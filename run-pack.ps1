dotnet pack --configuration Release -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg -o ./bin/pkgs TextDiff/TextDiff.csproj 