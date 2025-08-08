@echo off
SET host=77.37.200.24:5554
SET NUGET-SERVER-API-KEY=x8tldAV12zf2yqfJyiNHocWU4c89JC9A

cd /d %~dp0\..

call :PushPackage "src\Inventory\MNX.MonitoringCenter.Inventory.Contracts" "MNX.MonitoringCenter.Inventory.Contracts.csproj"

call :PushPackage "src\Management\MNX.MonitoringCenter.Management.Agent.Commands" "MNX.MonitoringCenter.Management.Agent.Commands.csproj"

call :PushPackage "src\Traffic\MNX.MonitoringCenter.Traffic.Contracts.Bus" "MNX.MonitoringCenter.Traffic.Contracts.Bus.csproj"

pause
exit /b

:: Функция для извлечения версии и публикации пакета
:PushPackage
setlocal
set projectPath=%1
set csprojFile=%2
set version=

for /f "delims=" %%i in ('powershell -Command "[xml]$csproj = Get-Content \"%projectPath%\\%csprojFile%\"; $csproj.Project.PropertyGroup.Version"') do set version=%%i

set version=%version: =%

echo Version extracted: %version%

if exist %projectPath%\bin\Release (
    cd %projectPath%\bin\Release
) else (
    echo Error: Folder %projectPath%\bin\Release does not exists!
    pause
)

dotnet nuget push -s http://%host%/v3/index.json -k %NUGET-SERVER-API-KEY% --skip-duplicate %csprojFile:~0,-8%.%version%.nupkg
endlocal
goto :eof