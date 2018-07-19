@echo off
set cpycmd=/D /I /E /F /Y /H /R 

:: publish DeviceManager project
cd src/DeviceManager/DeviceManager.API/src
dotnet restore
dotnet build -c Release
dotnet publish -r win10-x64 --self-contained --output ../../../../dist/DeviceManager

:: copy configuration 
xcopy "../../../.config" "../../../../dist/DeviceManager/.config" %cpycmd%

:: publish angular project
cd ../../DeviceManager.Web
:: npm install
cmd /c ng build --base-href ./

xcopy "dist" "../../../dist/DeviceManager/wwwroot" %cpycmd%

::publish electron project
cd ../../electron
cmd /c npm install
cmd /c npm run-script build

