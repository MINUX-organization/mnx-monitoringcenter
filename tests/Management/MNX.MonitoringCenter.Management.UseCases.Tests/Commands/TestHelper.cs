using AutoMapper;
using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands;

/// <summary>
/// Содержит необходимые для тестирования модели и компоненты.
/// </summary>
internal static class TestHelper
{
    /// <summary>
    /// Модель криптовалюты.
    /// </summary>
    internal static Cryptocurrency Cryptocurrency { get; } = new()
    {
        Id = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a"),
        FullName = "Bitcoin",
        ShortName = "BTC",
        Algorithm = "Algorithm",
        UserId = 1
    };

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    internal static long UserId { get; } = 1;

    /// <summary>
    /// Видеокарта.
    /// </summary>
    internal static string GpuName { get; } = "GeForce Gtx 1060";

    /// <summary>
    /// Получить экземпляр маппера.
    /// </summary>
    /// <returns> Маппер. </returns>
    internal static IMapper GetMapper()
    {
        var cfg = new MapperConfigurationExpression();
        cfg.AddProfile(new MappingProfile());
        return new Mapper(new MapperConfiguration(cfg));
    }
}
