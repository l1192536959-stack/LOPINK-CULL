@echo off
setlocal
cd /d "%~dp0"
where dotnet >nul 2>nul || (echo .NET 8 SDK is required. Download: https://dotnet.microsoft.com/download/dotnet/8.0 & pause & exit /b 1)
dotnet publish LOPINK_CULL.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
if errorlevel 1 (echo Publish failed.&pause&exit /b 1)
echo EXE output is in bin\Release\net8.0-windows\win-x64\publish\
pause