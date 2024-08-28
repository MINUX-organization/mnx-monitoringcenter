using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Инвентаризация.
/// </summary>
internal class Inventory : IEquatable<Inventory>
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
    public List<Contracts.Cpu.Cpu> Cpus { get; set; } = new();

    /// <summary>
    /// Диски.
    /// </summary>
    public List<Contracts.Drive.Drive> Drives { get; set; } = new();

    /// <summary>
    /// Видеокарты.
    /// </summary>
    public List<Contracts.Gpu.Gpu> Gpus { get; set; } = new();

    /// <summary>
    /// Сетевые адаптеры.
    /// </summary>
    public List<Contracts.NetworkAdapter.NetworkAdapter> NetworkAdapters { get; set; } = new();

    /// <summary>
    /// Материнская плата.
    /// </summary>
    public Contracts.Motherboard.Motherboard Motherboard { get; set; }

    /// <summary>
    /// Программное обеспечение.
    /// </summary>
    public SoftwareInventory Software { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Inventory inventory)
        {
            return Equals(inventory);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Inventory? other)
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
