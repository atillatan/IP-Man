@echo off
set cpycmd=/D /I /E /F /-Y /H /R 

:: publish netcore project
cd src/DeviceManager/DeviceManager.API/src
dotnet build
dotnet publish -r win10-x64 --self-contained --output ../../../../dist/DeviceManager
xcopy "../../../.config/devicemanager" "../../../../dist/DeviceManager/.config/devicemanager"  %cpycmd%

:: publish angular project
cd ../../DeviceManager.Web
cmd /c ng build --base-href ./

:: publish electron project
cd ../../electron
npm run-script build

cmd /c npm start

