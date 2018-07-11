echo %%PUBLISH_PATH%%
echo %%VERSION%%
dotnet pack src --include-source --output %PUBLISH_PATH% --verbosity n /p:PackageVersion=%BUILD_VERSION% -c relase