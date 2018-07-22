#!/bin/sh
export ASPNETCORE_ENVIRONMENT="production"
# publish DeviceManager project
cd src/DeviceManager/DeviceManager.API/src
dotnet restore
dotnet build -c Release
dotnet publish -r osx.10.11-x64 --self-contained --output ../../../../dist/DeviceManager

# copy configuration 
cp -Rf "../../../.config" "../../../../dist/DeviceManager/.config"

# publish angular project
cd ../../DeviceManager.Web
npm install
ng build --base-href ./
cp -Rf "./dist/" "../../../dist/DeviceManager/wwwroot"

# publish electron project

cd ../../electron
npm install
npm run-script build





