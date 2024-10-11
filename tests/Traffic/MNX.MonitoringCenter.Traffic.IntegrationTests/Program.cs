using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;

namespace MNX.MonitoringCenter.Traffic.Observers.IntegrationTests;

internal class Program
{
    static async Task Main(string[] args)
    {
        await Task.WhenAll(SendIndicators(), StartConnectionAsync());
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

        using var bus = RabbitHutch.CreateBus("host=localhost:5672;username=guest;password=guest;publisherConfirms=true");

        for (int i = 0; i < 10000; i++)
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
                            FlightSheet = new FlightSheetStatistics()
                            {
                                Id = Guid.NewGuid(),
                                MinerId = Guid.NewGuid(),
                                Coins = new()
                                {
                                    new CoinStatistics()
                                    {
                                        CoinId = Guid.Parse("7b1d26a9-2418-46b7-9166-fafb9a7f2b1d"),
                                        HashRate = random.Next(1000),
                                        Shares = new SharesModel()
                                        {
                                            Accepted = random.Next(1000),
                                            Rejected = random.Next(1000),
                                        }
                                    },
                                    new CoinStatistics()
                                    {
                                        CoinId = Guid.Parse("a63b11e1-3763-4d3e-9118-a2b81a92634f"),
                                        HashRate = random.Next(1000),
                                        Shares = new SharesModel()
                                        {
                                            Accepted = random.Next(1000),
                                            Rejected = random.Next(1000),
                                        }
                                    },
                                }
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
                });
            }

            await Task.Delay(2000);
        }

    }

    private static async Task StartConnectionAsync()
    {
        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5172/hubs/monitoring")
            .Build();

        try
        {
            // Подключение к SignalR хабу
            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR hub");

            var stream1 = connection.StreamAsync<int>("Subscribe", SubscriptionType.TotalPower);
            var stream2 = connection.StreamAsync<SharesModel>("Subscribe", SubscriptionType.TotalShares);
            var stream3 = connection.StreamAsync<int>("Subscribe", SubscriptionType.TotalHashRate);

            _ = Task.Run(async () =>
            {
                await foreach (var item in stream1)
                {
                    Console.WriteLine($"Total Power: {item}");
                }
            });

            _ = Task.Run(async () =>
            {
                await foreach (var item in stream2)
                {
                    Console.WriteLine($"accepted: {item.Accepted}, rejected: {item.Rejected}");
                }
            });

            _ = Task.Run(async () =>
            {
                await foreach (var item in stream3)
                {
                    Console.WriteLine($"Total HashRate: {item}");
                }
            });

            await Task.Delay(4000);

            var stream4 = connection.StreamAsync<int>("Subscribe", SubscriptionType.TotalHashRate);

            _ = Task.Run(async () =>
            {
                await foreach (var item in stream4)
                {
                    Console.WriteLine($"Duplicate Total HashRate: {item}");
                }
            });

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
