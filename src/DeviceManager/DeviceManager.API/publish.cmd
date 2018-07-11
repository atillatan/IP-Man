set cpycmd=/D /I /E /F /-Y /H /R /EXCLUDE:excludes.txt
dotnet build src
dotnet publish src --runtime win-x64 --self-contained
xcopy "../../.config/devicemanager/*" "src/bin/Debug/netcoreapp2.1/win-x64/publish/.config/devicemanager"  %cpycmd%
