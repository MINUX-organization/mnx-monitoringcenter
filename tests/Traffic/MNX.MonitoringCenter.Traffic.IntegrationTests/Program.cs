using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;

namespace MNX.MonitoringCenter.Traffic.Observers.IntegrationTests;

public enum SubStream
{
    Monitoring,
    Devices,
    Rigs
}

internal class Program
{
    static async Task Main(string[] args)
    {
        await StartConnectionAsync();
        //await Task.WhenAll(SendIndicators(), StartConnectionAsync());
    }

    private static async Task SendIndicators()
    {
        List<Guid> rigsIds = new()
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

        var random = new Random();

        using var bus = RabbitHutch.CreateBus("host=77.37.200.24:5672;username=guest;password=guest;publisherConfirms=true");

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                await bus.PubSub.PublishAsync(new RigDynamicIndicators()
                {
                    RigId = rigsIds[j],
                    UserId = Guid.Parse("b80f8d4c-12e6-4430-8fe2-5d5339bbb4db"),
                    Devices = new()
                    {
                        new CpuDynamicIndicators()
                        {
                            DeviceId = Guid.NewGuid(),
                            Power = 1,
                            FanSpeed = random.Next(100),
                            Temperature = random.Next(100),
                            MiningState = MiningState.Active,
                            MinerName = "Miner",
                            MiningUpTimeInSeconds = 1000,
                            Coins = new()
                                {
                                    new MiningMetrics()
                                    {
                                        HashRate = random.Next(1000),
                                        Shares = new SharesModel()
                                        {
                                            Accepted = random.Next(1000),
                                            Rejected = random.Next(1000),
                                        }
                                    },
                                    new MiningMetrics()
                                    {
                                        HashRate = random.Next(1000),
                                        Shares = new SharesModel()
                                        {
                                            Accepted = random.Next(1000),
                                            Rejected = random.Next(1000),
                                        }
                                    },
                                }
                        },
                        new GpuDynamicIndicators()
                        {
                            DeviceId = Guid.NewGuid(),
                            Power = 1,
                            FanSpeed = random.Next(100),
                            MemoryTemperature = random.Next(100),
                            CoreTemperature = random.Next(100),
                            MiningState = MiningState.Inactive,
                        },
                        new Traffic.Contracts.Bus.Devices.Network.NetworkAdapterDynamicIndicators()
                        {
                            DeviceId= Guid.NewGuid(),
                            IsUse = true,
                            Power = 1,
                            OnlineState = OnlineState.Three,
                            InternetSpeed = random.Next(0, 10000),
                        }
                    }
                });;
            }

            await Task.Delay(2000);
        }

    }

    private static async Task StartConnectionAsync()
    {
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImJmZGQ4YWNhLWViNmQtNGQwZC1hMTlhLTljYTE2MWFlNDQ2MyIsIkNsaWVudFR5cGUiOiJVc2VyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6IlRlc3RfMSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRlZmF1bHRVc2VyIiwiZXhwIjoxNzQ1MDc3MTQyLCJpc3MiOiJzZWN1cml0eSIsImF1ZCI6IlVzZXIifQ.LzZZJlplBhkP4CoMLmJ2RTb-AnJsGc29IeKSeD-ONNg";
        var connection = new HubConnectionBuilder()
            .WithUrl($"http://localhost:9000/hubs/monitoring?access_token={token}")
            .Build();

        try
        {
            // Подключение к SignalR хабу
            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR hub");

            var stream1 = connection.StreamAsync<object>("Subscribe", SubStream.Devices);

            await foreach (var item in stream1)
            {
                Console.WriteLine($"Total Power: {item}");
                Console.WriteLine(DateTime.Now);
            }

            await Task.Delay(4000);

            await connection.SendAsync("Unsubscribe");

            await Task.Delay(4000);

            var stream2 = connection.StreamAsync<object>("Subscribe", SubStream.Devices);

            _ = Task.Run(async () =>
            {
                await foreach (var item in stream2)
                {
                    Console.WriteLine($"Total Power: {item}");
                }
            });

            await Task.Delay(4000);

            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR hub: {ex.Message}");
        }
        finally
        {
            await connection.DisposeAsync();
        }
    }
}
