using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

/// <summary>
/// Базовый обработчик сохранение разгона.
/// </summary>
public abstract class SaveOverclockingBaseHandler
{
    protected readonly IMapper _mapper;

    protected readonly IMediator _mediator;

    public SaveOverclockingBaseHandler(IMapper mapper,
                                       IMediator mediator)
    {
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ??
            throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Проверка валидности разгона по имени видеокарты или идентификатору.
    /// </summary>
    protected async Task<Result<Unit>> ValidateOverclocking<T>(T gpuIdentifier, IOverclocking overclocking, CancellationToken cancellationToken)
    {
        if (overclocking.TargetDeviceType != OverclockingTargetDeviceType.GPU)
        {
            return Result<Unit>.Error("Target device type is not supported");
        }

        var restrictions = await GetRestrictionsByGpuIdentifier(gpuIdentifier, cancellationToken);
        var validator = new GpuOverclockingValidator(restrictions.GetValue());
        var validationResult = await validator.ValidateAsync((GpuOverclocking)overclocking, cancellationToken);

        return validationResult.IsValid
                ? Result<Unit>.Empty()
                : Result<Unit>.Invalid(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());
    }

    private async Task<Result<GpuRestrictions>> GetRestrictionsByGpuIdentifier<T>(T gpuIdentifier, CancellationToken cancellationToken)
    {
        if (gpuIdentifier is Guid gpuId)
        {
            return await _mediator.Send(new GetGpuRestrictionsByIdQuery(gpuId), cancellationToken);
        }

        if (gpuIdentifier is string gpuName)
        {
            return await _mediator.Send(new GetGpuRestrictionsQuery(gpuName), cancellationToken);
        }

        throw new ArgumentException("Invalid GPU identifier type");
    }
}
