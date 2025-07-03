using AutoMapper;
using FluentValidation.Results;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Validation;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

/// <summary>
/// Базовый обработчик сохранение разгона.
/// </summary>
public abstract class SaveOverclockingBaseHandler
{
    protected readonly IMapper _mapper;

    protected readonly IMediator _mediator;

    ///
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
    protected async Task<Result<Unit>> ValidateOverclocking<T>(T gpuIdentifier,
                                                               IOverclocking overclocking,
                                                               CancellationToken cancellationToken)
    {
        if (overclocking.TargetDeviceType == OverclockingTargetDeviceType.CPU)
        {
            return Result<Unit>.Error("Target device type is not supported");
        }

        var restrictions = await GetRestrictionsByGpuIdentifier(gpuIdentifier!.ToString()!, cancellationToken);
        var validationResult = await ValidateOverclocking(overclocking, restrictions.GetValue(), cancellationToken);

        return validationResult.IsValid
                ? Result<Unit>.Empty()
                : Result<Unit>.Invalid(validationResult.Errors.Select(x => x.ErrorMessage).ToArray());
    }

    private async Task<ValidationResult> ValidateOverclocking(IOverclocking overclocking,
                                                              GpuRestrictions restrictions,
                                                              CancellationToken cancellationToken)
    {
        switch (overclocking)
        {
            case NvidiaGpuOverclocking nvidia:
                var nvidiaValidator = new NvidiaGpuOverclockingValidator((NvidiaGpuRestrictions)restrictions);
                return await nvidiaValidator.ValidateAsync(nvidia, cancellationToken);

            case AmdGpuOverclocking amd:
                var amdValidator = new AmdGpuOverclockingValidator((AmdGpuRestrictions)restrictions);
                return await amdValidator.ValidateAsync(amd, cancellationToken);

            case IntelGpuOverclocking intel:
                var intelValidator = new IntelGpuOverclockingValidation();
                return await intelValidator.ValidateAsync(intel, cancellationToken);

            default:
                throw new InvalidCastException($"Unsupported type of overclocking: {overclocking.GetType().Name}");
        }
    }

    private async Task<Result<GpuRestrictions>>
        GetRestrictionsByGpuIdentifier(string gpuIdentifier, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(gpuIdentifier, out Guid id))
        {
            return await _mediator.Send(new GetGpuRestrictionsByIdQuery(id), cancellationToken);
        }
        else
        {
            return await _mediator.Send(new GetGpuRestrictionsQuery(gpuIdentifier), cancellationToken);
        }

        throw new ArgumentException("Invalid GPU identifier type");
    }
}
