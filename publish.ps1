Remove-Item ./example/bin -Recurse -Force

dotnet publish -c Release -o ./example/bin/win-x64 ./src/NSwag/NSwag.csproj /p:PublishProfile=./src/NSwag/Properties/PublishProfiles/win.pubxml
dotnet publish -c Release -o ./example/bin/linux-x64 ./src/NSwag/NSwag.csproj /p:PublishProfile=./src/NSwag/Properties/PublishProfiles/linux.pubxml
dotnet publish -c Release -o ./example/bin/osx-x64 ./src/NSwag/NSwag.csproj /p:PublishProfile=./src/NSwag/Properties/PublishProfiles/osx.pubxml
