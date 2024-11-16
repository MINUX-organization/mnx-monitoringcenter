using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices;

/// <summary>
/// Запрос на получение списка устройств.
/// </summary>
public class GetDevicesQuery : IStreamRequest<Device>
{
    /// <summary>
    /// Типы устройств.
    /// </summary>
    public DeviceType? DeviceTypes { get; }

    /// <summary>
    /// Спецификация устройств.
    /// </summary>
    public InventorySpecification Specification { get; }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="GetDevicesQuery"/>.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigsIds"> Идентификаторы запрашиваемых ригов. Если нет, то все доступные риги. </param>
    /// <param name="deviceTypes"> Типы устройств. </param>
    public GetDevicesQuery(Guid userId, Guid[]? rigsIds = null, DeviceType? deviceTypes = null)
    {
        DeviceTypes = deviceTypes;
        Specification = new(userId, rigsIds, true);
    }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="GetDevicesQuery"/>.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="deviceTypes"> Типы устройств. </param>
    public GetDevicesQuery(Guid userId, Guid rigId, DeviceType? deviceTypes = null)
    {
        DeviceTypes = deviceTypes;
        Specification = new(userId, new Guid[] { rigId }, true);
    }
}
