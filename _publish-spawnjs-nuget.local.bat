@echo off
@REM Finds the latest Release nupkg for SpawnDev.SpawnJS and publishes it to the local feed
set projectPath=%~dp0SpawnDev.SpawnJS
set releaseFolder=%projectPath%\bin\Release
@REM Finding latest nupkg

@echo:

FOR /F "eol=| delims=" %%I IN ('DIR "%releaseFolder%\*.nupkg" /A-D /B /O-D /TW 2^>nul') DO SET "NewestFile=%%I" & GOTO FoundFile
ECHO No *.nupkg file found
GOTO :EOF

:FoundFile
ECHO Latest *.nupkg file is:
ECHO %NewestFile%

nuget add "%releaseFolder%\%NewestFile%" -source "D:\users\SpawnDevPackages"
