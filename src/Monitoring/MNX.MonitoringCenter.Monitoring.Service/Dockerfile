FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

ARG version=0.0.0.1-dev
ENV env_version=$version

ARG nuget_server
ENV env_nuget_server=$nuget_server

WORKDIR /src
COPY . .
RUN dotnet nuget add source ${env_nuget_server} -n Minux
RUN dotnet restore "src/Monitoring/MNX.MonitoringCenter.Monitoring.Service/MNX.MonitoringCenter.Monitoring.Service.csproj"
RUN dotnet publish "src/Monitoring/MNX.MonitoringCenter.Monitoring.Service/MNX.MonitoringCenter.Monitoring.Service.csproj" -c Release -o /publish /property:Version=${env_version}

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /publish
COPY --from=build /publish .
EXPOSE 80 
ENTRYPOINT ["dotnet", "MNX.MonitoringCenter.Monitoring.Service.dll"]
