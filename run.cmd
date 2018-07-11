@echo off

:: publish netcore project
cd src/netcore
dotnet restore
dotnet build
dotnet publish -r win10-x64 --self-contained --output ../../dist/netcore

:: publish angular project
cd ../angular
:: npm install

cmd /c ng build --base-href ./


:: publish electron project
cd ../electron
::npm install

cmd /c npm start
