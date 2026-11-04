@echo off
setlocal
cd /d "%~dp0"
title Smash House POS - GUI

where dotnet >nul 2>nul
if errorlevel 1 goto nodotnet

echo Building...
dotnet build FastFoodPOS.csproj -c Debug -v quiet -nologo
if errorlevel 1 goto fail

echo Starting...
start "" "bin\Debug\net8.0-windows\FastFoodPOS.exe"
exit /b 0

:nodotnet
echo .NET 8 SDK was not found.
echo Download it from https://dotnet.microsoft.com/download/dotnet/8.0
pause
exit /b 1

:fail
echo.
echo Build failed. Copy the error text above and send it to get it fixed.
pause
exit /b 1
