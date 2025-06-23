FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src

COPY ./nuget.config .
COPY ./src ./src
COPY ./tests ./tests
COPY MNX.MonitoringCenter.sln .

RUN dotnet restore MNX.MonitoringCenter.sln
RUN dotnet publish MNX.MonitoringCenter.sln -c Release -o /publish



FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime

WORKDIR /publish

COPY --from=build /publish .

EXPOSE 80 
ENTRYPOINT ["dotnet", "MNX.MonitoringCenter.RigsApi.Service.dll"]	