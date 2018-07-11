#!/bin/sh

# publish DeviceManager project
cd src/DeviceManager/DeviceManager.API/src
dotnet restore
dotnet build
dotnet publish -r osx.10.11-x64 --self-contained --output ../../../../dist/DeviceManager
cp -Rf "../../../.config" "../../../../dist/DeviceManager/.config"

# publish angular project
cd ../../DeviceManager.Web
# npm install
ng build --base-href ./

# publish electron project
cd ../../electron
# npm install
npm run-script build


 


