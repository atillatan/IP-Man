#!/bin/sh

# publish netcore project
cd src/netcore
dotnet restore
dotnet build
dotnet publish -r osx.10.11-x64 --self-contained --output ../../dist/netcore

# publish angular project
cd ../angular
npm install

ng build --base-href=./ 

# publish electron project
cd ../electron
npm install

npm start
