using FluentValidation;
using FluentValidation.Validators;
using MNX.MonitoringCenter.Management.Core.Enums;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;
using MNX.MonitoringCenter.Management.UseCases.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.PropertyValidators;

internal class TargetMinerValidator : IAsyncPropertyValidator<FlightSheetTargetInputModel, Guid>
{
    private readonly IMinerRepository _minerRepository;

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

        if (miner.DeviceTypes.All(deviceType => DeviceTargetTypeConvertor.Convert(deviceType.DeviceType) != model.Type))
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

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "Passed miner id is invalid!";
    }

    public string Name => "TargetMiner";
}