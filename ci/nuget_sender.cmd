SET host=77.37.200.24:5554

cd ../
cd ./src

cd ./Inventory
cd ./MNX.MonitoringCenter.Inventory.Contracts/bin/Release

dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Inventory.Contracts.1.0.5.nupkg

cd ../../../../
cd ./Management
cd ./MNX.MonitoringCenter.Management.Agent.Commands/bin/Release

dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Management.Agent.Commands.1.0.5.nupkg

cd ../../../../
cd ./Traffic
cd ./MNX.MonitoringCenter.Traffic.Contracts.Bus/bin/Release

dotnet nuget push -s http://%host%/v3/index.json -k NUGET-SERVER-API-KEY MNX.MonitoringCenter.Traffic.Contracts.Bus.1.0.3.nupkg

cd ../../../../

pause