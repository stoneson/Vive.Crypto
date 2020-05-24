cd %~dp0
nuget spec
dotnet pack
nuget push Vive.Crypto.1.1.1.nupkg oy2br7pblxvg2gik355qcxswdzvao53evan3mmyhb3pm4e -Source https://api.nuget.org/v3/index.json

pause