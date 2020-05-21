cd %~dp0
nuget spec
dotnet pack
nuget push HCenter.Encryption.1.0.1.nupkg oy2noesfx54uio6c5fqafwdwqfpq7pxubnh5gjq4ajfl6e -Source https://api.nuget.org/v3/index.json

pause