@echo off

:: Path to CurrentVersion.txt
set VERSION_FILE=%CD%\..\src\MNX.MonitoringCenter.RigsApi.Service\CurrentVersion.txt

:: To check for the presence of a version.txt file
if not exist "%VERSION_FILE%" (
    echo File %VERSION_FILE% not found. Please ensure it exists.
    pause
    exit /b
)

:: Read the first, second, and third numbers from the version.txt file
setlocal enabledelayedexpansion
set /a line=0
for /f "tokens=*" %%a in (%VERSION_FILE%) do (
    set /a line+=1
    if !line! == 1 set MAJOR=%%a
    if !line! == 2 set MINOR=%%a
    if !line! == 3 set PATCH=%%a
)
set /a PATCH=%PATCH%+1
set NEW_VERSION=%MAJOR%.%MINOR%.%PATCH%
echo %NEW_VERSION%

:: Verify that a version has been successfully retrieved
if "%NEW_VERSION%"=="" (
    echo Version extraction or increment failed.
    pause
    exit /b
)

:: 1. Start Docker Compose...
echo Starting Docker Compose...
docker compose build --no-cacheа
docker compose up -d

:: 2. Tag Docker image MODULE
echo Tagging Docker image with version %NEW_VERSION%...
docker tag 77.37.200.24:5000/rigs-api 77.37.200.24:5000/rigs-api:%NEW_VERSION%-dev

:: 3. Tag Docker latest DB
echo Tagging Docker image with version latest..
docker tag 77.37.200.24:5000/monitoring-center-db 77.37.200.24:5000/monitoring-center-db:%NEW_VERSION%-dev

:: 4. Push Docker image MODULE
echo Pushing Docker image with version %NEW_VERSION%...
docker push 77.37.200.24:5000/rigs-api:%NEW_VERSION%-dev

:: 5. Push Docker latest MODULE
echo Pushing Docker image with version latest
docker push 77.37.200.24:5000/rigs-api:latest

:: 6. Push Docker image DB
echo Pushing Docker image with version %NEW_VERSION%...
docker push 77.37.200.24:5000/monitoring-center-db:%NEW_VERSION%-dev

:: 7. Push Docker latest DB
echo Pushing Docker image with version latest
docker push 77.37.200.24:5000/monitoring-center-db:latest

(
    echo %MAJOR%
    echo %MINOR%
    echo %PATCH%
) > %VERSION_FILE%

echo Script completed!

pause