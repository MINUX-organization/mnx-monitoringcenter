using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using MNX.MonitoringCenter.Monitoring.Service.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

namespace MonitoringClient;

internal class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRlZmF1bHRSb2xlIiwiZXhwIjoxNzEzMDEwNTIyfQ.kB5bbqG3TWTGEv2ez5Dpw4iSI7dM6i1PEIqtoL3uLak";

            var connection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:8002/hubs/monitoring", options =>
                {
                    options.Transports = HttpTransportType.WebSockets;
                    options.AccessTokenProvider = () => Task.FromResult(token)!;
                })
                .Build();

            connection.On<IEnumerable<RigInformationMessage>>("ReceivedRigsInformation", x => Console.WriteLine("Пришла информация о ригах."));

            connection.On<TotalDataChangeMessage>("ReceivedTotalData", x => Console.WriteLine($"Получены обобщённые данные: {x.Type}"));

            await connection.StartAsync();

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
