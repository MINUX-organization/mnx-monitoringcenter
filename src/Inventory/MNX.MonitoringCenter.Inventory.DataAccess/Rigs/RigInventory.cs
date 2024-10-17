using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Drive;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Motherboard;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

namespace MNX.MonitoringCenter.Inventory.DataAccess.RigInventory;

/// <summary>
/// Инвентаризация рига.
/// </summary>
public class RigInventory : IEquatable<RigInventory>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    public Guid RigId { get; set; }

    /// <summary>
    /// Риг.
    /// </summary>
    public RigDto? Rig { get; set; }

    /// <summary>
    /// Дата и время проведения инвентаризации.
    /// </summary>
    public DateTimeOffset CreatedDateTime { get; set; }

    /// <summary>
    /// Дата и время окончания действия инвентаризации.
    /// </summary>
    public DateTimeOffset? EndDateTime { get; set; }

    /// <summary>
    /// Процессоры.
    /// </summary>
    public List<Cpu> Cpus { get; set; } = new();

    /// <summary>
    /// Диски.
    /// </summary>
    public List<Drive> Drives { get; set; } = new();

    /// <summary>
    /// Видеокарты.
    /// </summary>
    public List<Gpu> Gpus { get; set; } = new();

    /// <summary>
    /// Сетевые адаптеры.
    /// </summary>
    public List<NetworkAdapter> NetworkAdapters { get; set; } = new();

    /// <summary>
    /// Материнская плата.
    /// </summary>
    public required Motherboard Motherboard { get; set; }

    /// <summary>
    /// Программное обеспечение.
    /// </summary>
    public required SoftwareInventory Software { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is RigInventory inventory)
        {
            return Equals(inventory);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(RigInventory? other)
    {
        if (other == null) return false;

        if (ReferenceEquals(this, other)) return true;

        return RigId == other.RigId &&
               Cpus.SequenceEqual(other.Cpus) &&
               Drives.SequenceEqual(other.Drives) &&
               Gpus.SequenceEqual(other.Gpus) &&
               NetworkAdapters.SequenceEqual(other.NetworkAdapters) &&
               Motherboard.Equals(other.Motherboard) &&
               Software.Equals(other.Software);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(RigId,
                                ComputeListHashCode(Cpus),
                                ComputeListHashCode(Drives),
                                ComputeListHashCode(Gpus),
                                ComputeListHashCode(NetworkAdapters),
                                Motherboard,
                                Software);
    }

    /// <summary>
    /// Вычислить хеш-код списка.
    /// </summary>
    /// <typeparam name="T"> Тип элементов в списке. </typeparam>
    /// <param name="list"> Список. </param>
    /// <returns> Хеш-код </returns>
    private static int ComputeListHashCode<T>(List<T> list)
    {
        int hash = 0;

        foreach (T item in list)
        {
            hash += item!.GetHashCode();
        }

        return hash;
    }
}
