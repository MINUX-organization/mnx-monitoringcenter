using FluentValidation;
using FluentValidation.Validators;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Miner.Enums;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;
using MNX.MonitoringCenter.Management.UseCases.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Validators;

/// <summary>
/// Валидатор майнера таргета.
/// </summary>
internal class TargetMinerValidator : IAsyncPropertyValidator<FlightSheetTargetInputModel, Guid>
{
    private readonly IMinerRepository _minerRepository;

    /// <inheritdoc/>
    public string Name { get => "TargetMiner"; }

    public TargetMinerValidator(IMinerRepository minerRepository)
    {
        _minerRepository = minerRepository;
    }

    public async Task<bool> IsValidAsync(ValidationContext<FlightSheetTargetInputModel> context, Guid value, CancellationToken cancellation)
    {
        var model = context.InstanceToValidate;
        var miner = await _minerRepository.GetMinerById(model.MinerId);

        if (miner == null)
        {
            context.AddFailure($"Miner with id equaled {model.MinerId} was not found!");
            return true;
        }

        if (miner.SupportedDevices.IsDeviceSupported(model.Type))
        {
            context.AddFailure("Target type is not supported by the miner.");
        }

        if (model.Type == FlightSheetTargetType.GPU && model.Configs.Count != (int)miner.MiningMode)
        {
            context.AddFailure($"Invalid number of coins was passed for mining mode " +
                               $"{miner.MiningMode}: {model.Configs.Count}.");
        }

        return true;
    }

    /// <inheritdoc/>
    public string GetDefaultMessageTemplate(string errorCode) => "Passed miner id is invalid!";
}