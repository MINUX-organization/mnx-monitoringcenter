namespace MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;

public interface IConverterFrom<TDestination>
{
    public abstract static TDestination? ConvertFrom<TSource>(TSource source);
}