@echo off

dotnet build src/Limbo.Umbraco.Search --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget