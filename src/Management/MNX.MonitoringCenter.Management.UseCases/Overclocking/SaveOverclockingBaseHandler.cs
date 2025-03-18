using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

/// <summary>
/// Базовый обработчик сохранение разгона.
/// </summary>
public abstract class SaveOverclockingBaseHandler
{
    protected readonly IMediator _mediator;

    public SaveOverclockingBaseHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить признак валидности разгона.
    /// </summary>
    /// <param name="gpuName"> Название видеокарты. </param>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Результат валидации. </returns>
    protected async Task<Result<Unit>> IsValidOverclocking(string gpuName,
                                                           Core.Overclocking.IOverclocking overclocking,
                                                           CancellationToken cancellationToken)
    {
        if (overclocking.TargetDeviceType == OverclockingTargetDeviceType.GPU)
        {
            return await IsValidOverclocking(gpuName, (GpuOverclocking)overclocking, cancellationToken);
        }

        return Result<Unit>.Error("Target device type is not supported");
    }

    private async Task<Result<Unit>> IsValidOverclocking(string gpuName,
                                                         GpuOverclocking overclocking,
                                                         CancellationToken cancellationToken)
    {
        var restrictions = await _mediator.Send(new GetGpuRestrictionsQuery(gpuName), cancellationToken);
        var validator = new GpuOverclockingValidator(restrictions.GetValue());

        var validationResult = await validator.ValidateAsync(overclocking, cancellationToken);

        return validationResult.IsValid
                ? Result<Unit>.Empty()
                : Result<Unit>.Invalid(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());
    }
}
