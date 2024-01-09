@echo off
dotnet build src/Limbo.Integrations.BorgerDk --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:\nuget\