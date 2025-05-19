using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;
using MNX.Application.Serializers;
using MNX.Application.Serializers.MsgPack;

namespace MNX.MonitoringCenter.Traffic.Contracts.SerializationTests;

public class MsgPackSerializersTests
{
    private readonly IMessageSerializer _serializer = new MsgPackSerializer();

    [TestCaseSource(typeof(SerializeTestCases), nameof(SerializeTestCases.PolymorphModels))]
    public void SerializePolymorphismTest(IDeviceDynamicIndicators model)
    {
        RunTest(model);
    }

    [TestCaseSource(typeof(SerializeTestCases), nameof(SerializeTestCases.RigDynamicIndicators))]
    public void SerializeRigDynamicIndicatorsTest(RigDynamicIndicators model)
    {
        RunTest(model);
    }

    private void RunTest(object model)
    {
        try
        {
            var body = _serializer.SerializeToBytes(model.GetType(), model);

            var typeName = $"{model.GetType().FullName}, {model.GetType().Assembly.FullName}";
            var type = Type.GetType(typeName);

            var newModel = _serializer.Deserialize(type!, body);

            Assert.That(newModel?.GetType(), Is.EqualTo(model.GetType()));
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
        }
    }

    private class SerializeTestCases
    {
        public static IEnumerable<IDeviceDynamicIndicators> PolymorphModels
        {
            get
            {
                yield return new CpuDynamicIndicators()
                {
                    Temperature = 100,
                    FanSpeed = 100
                };

                yield return new GpuDynamicIndicators()
                {
                    MemoryTemperature = 100,
                    CoreTemperature = 100,
                    FanSpeed = 100
                };

                yield return new NetworkAdapterDynamicIndicators()
                {
                    InternetSpeed = 100
                };
            }
        }

        public static IEnumerable<RigDynamicIndicators> RigDynamicIndicators
        {
            get
            {
                yield return new RigDynamicIndicators()
                {
                    RigId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    BootedUpTimeInSeconds = 1_000_000,
                    Devices = new List<IDeviceDynamicIndicators>()
                    {
                        new CpuDynamicIndicators()
                        {
                            DeviceId = Guid.NewGuid(),
                            MiningState = MiningState.Active,
                            Coins = new(),
                            MiningUpTimeInSeconds = 0
                        },

                        new GpuDynamicIndicators()
                        {
                            DeviceId = Guid.NewGuid(),
                            MiningState = MiningState.Inactive,
                            FanSpeed = 29,
                            CoreTemperature = 30,
                            MinerName = "rigel",
                            Coins = new(),
                            MiningUpTimeInSeconds = 0,
                            Power = 12
                        },

                        new GpuDynamicIndicators()
                        {
                            DeviceId = Guid.NewGuid(),
                            MiningState = MiningState.Inactive,
                            FanSpeed = 0,
                            CoreTemperature = 49,
                            Coins = new(),
                            MiningUpTimeInSeconds = 0,
                            Power = 4
                        },

                        new GpuDynamicIndicators()
                        {
                            DeviceId = Guid.NewGuid(),
                            MiningState = MiningState.Inactive,
                            FanSpeed = 0,
                            CoreTemperature = 51,
                            Coins = new(),
                            MiningUpTimeInSeconds = 0,
                            Power = 9
                        },

                        new NetworkAdapterDynamicIndicators()
                        {
                            DeviceId = Guid.NewGuid(),
                            IsUse = true,
                            OnlineState = OnlineState.Zero,
                            InternetSpeed = 4,
                            Power = 0
                        }
                    }
                };
            }
        }
    }
}