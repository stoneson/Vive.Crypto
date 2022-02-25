cd %~dp0
nuget spec
dotnet pack
nuget push Vive.Crypto.1.2.3.nupkg oy2n6654j43jw6moja5qexnagjsezuuurgx3hrzskbdhy4 -Source https://api.nuget.org/v3/index.json

pause