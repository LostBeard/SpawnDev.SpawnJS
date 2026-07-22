@echo off
@REM Finds the latest nupkg for SpawnDev.SpawnJS, confirms, asks for the apikey (secret), then publishes to nuget.org
@REM All paths are relative to this repo root.
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

setlocal
:PROMPT
SET /P AREYOUSURE=Publish this package to nuget.org? y/[N]?

IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

set /p apikey=Enter api key:
@IF NOT [%apikey%] == [] (
    dotnet nuget push "%releaseFolder%\%NewestFile%" --api-key %apikey% --source https://api.nuget.org/v3/index.json
) else (
    @echo Cancelled
)
:END
endlocal
