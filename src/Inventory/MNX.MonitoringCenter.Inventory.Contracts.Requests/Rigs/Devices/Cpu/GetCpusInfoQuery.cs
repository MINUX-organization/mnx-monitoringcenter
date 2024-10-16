using MediatR;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu;

using Cpu = Contracts.Devices.Cpu.Cpu;

/// <summary>
/// Запрос на получение списка процессоров.
/// </summary>
public class GetCpusInfoQuery : IStreamRequest<Cpu>
{
    /// <summary>
    /// Спецификация устройств.
    /// </summary>
    public DeviceSpecification Specification { get; }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="GetCpusInfoQuery"/>.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigsIds"> Идентификаторы запрашиваемых ригов. Если нет, то все доступные риги. </param>
    /// <param name="models"> Запрашиваемые модели процессоров. Если нет, то все доступные модели. </param>
    /// <param name="manufacturers"> Запрашиваемый производители процессоров. Если нет, то все доступные производители. </param>
    public GetCpusInfoQuery(Guid userId, Guid[]? rigsIds = null, string[]? models = null, string[]? manufacturers = null)
    {
        Specification = new(userId, rigsIds, models, manufacturers);
    }

    /// <summary>
    /// Создаёт экземпляр класса <see cref="GetCpusInfoQuery"/>.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="models"> Запрашиваемые модели процессоров. Если нет, то все доступные модели. </param>
    /// <param name="manufacturers"> Запрашиваемый производители процессоров. Если нет, то все доступные производители. </param>
    public GetCpusInfoQuery(Guid userId, Guid rigId, string[]? models = null, string[]? manufacturers = null)
    {
        Specification = new(userId, new Guid[] { rigId }, models, manufacturers);
    }
}
