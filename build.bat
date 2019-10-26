@echo off
mkdir build\release\
dotnet publish -r win-x64 -c Release
copy bin\Release\netcoreapp3.0\win-x64\publish\wttrwidget.exe build\release\wttrwidget.exe