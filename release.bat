@echo off
dotnet build src/Limbo.Integrations.BorgerDk --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget