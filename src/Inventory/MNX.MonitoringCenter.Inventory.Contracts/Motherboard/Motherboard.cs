namespace MNX.MonitoringCenter.Inventory.Contracts.Motherboard;

/// <summary>
/// Материнская плата.
/// </summary>
public class Motherboard : IEquatable<Motherboard>
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Информация о материнской плате.
    /// </summary>
    public required MotherboardInformation Information { get; init; }

    /// <summary>
    /// Список PCI.     
    /// </summary>
    public List<MotherboardPci> Pcies { get; init; } = new();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Motherboard motherboard)
        {
            return Equals(motherboard);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Motherboard? other)
    {
        if (other == null) return false;

        if (ReferenceEquals(this, other)) return true;

        return Id.Equals(other.Id) &&
               Information.Equals(other.Information) &&
               Pcies.SequenceEqual(other.Pcies);

    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int pciHash = 0;

        foreach (var pci in Pcies)
        {
            pciHash += pci.GetHashCode();
        }

        return HashCode.Combine(Id, Information, pciHash);
    }
}
