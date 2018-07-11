echo %PUBLISH_PATH%
echo %VERSION%
dotnet pack src --include-symbols --include-source --output %1 --verbosity n /p:PackageVersion=%2 -c relase 