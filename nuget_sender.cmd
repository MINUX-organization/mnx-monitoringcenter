SET host=77.37.200.24:5554
SET version=1.0.3

cd ./src

cd ./Inventory
cd ./MNX.MonitoringCenter.Inventory.Contracts/bin/Release

dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Inventory.Contracts.%version%.nupkg
dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Inventory.Contracts.%version%.snupkg

cd ../../../../
cd ./Management
cd ./MNX.MonitoringCenter.Management.Agent.Commands/bin/Release

dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Management.Agent.Commands.%version%.nupkg
dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Management.Agent.Commands.%version%.snupkg

cd ../../../../
cd ./Traffic
cd ./MNX.MonitoringCenter.Traffic.Contracts.Bus/bin/Release

dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Traffic.Contracts.Bus.%version%.nupkg
dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Traffic.Contracts.Bus.%version%.snupkg

cd ../../../../

pause