FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

ARG nuget_server
ENV env_nuget_server=$nuget_server

WORKDIR /src
COPY . .
RUN dotnet nuget add source ${env_nuget_server} -n Minux
RUN dotnet restore "src/Management/MNX.MonitoringCenter.Management.Service/MNX.MonitoringCenter.Management.Service.csproj"
RUN dotnet publish "src/Management/MNX.MonitoringCenter.Management.Service/MNX.MonitoringCenter.Management.Service.csproj" -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /publish
COPY --from=build /publish .
EXPOSE 80 
ENTRYPOINT ["dotnet", "MNX.MonitoringCenter.Management.Service.dll"]	