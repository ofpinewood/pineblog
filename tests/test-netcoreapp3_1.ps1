dotnet build ../PineBlog.sln --configuration Release
dotnet test ../PineBlog.sln --configuration Release --framework netcoreapp3.1 --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Include="[Opw.*]*" /p:Exclude="[*.Tests]*%2c[*.*Tests]*"
PAUSE