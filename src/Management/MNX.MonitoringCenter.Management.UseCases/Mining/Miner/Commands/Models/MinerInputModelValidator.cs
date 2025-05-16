using FluentValidation;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

/// <summary>
/// Валидатор параметров сущности <see cref="MinerInputModel"/>.
/// </summary>
public class MinerInputModelValidator : AbstractValidator<MinerInputModel>
{
    public MinerInputModelValidator(IMinerRepository minerRepository, Guid userId)
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9\-_./ ]+$")
            .WithMessage("Incorrect custom miner's name format");

        RuleFor(model => model.Version)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9\-_./ ]+$")
            .WithMessage("Incorrect custom miner's version format");

        RuleFor(model => model.InstallationUrl)
            .NotEmpty()
            .WithMessage("Installation url is required");

        RuleFor(model => model.SupportedDevices)
            .NotEqual(DeviceTypeManufacturerCombination.None)
            .WithMessage("Supported devices are not specified");

        RuleFor(model => model.PoolTemplate)
            .NotEmpty()
            .Matches(@"^[\x20-\x7Ea-zA-Z0-9]+$")
            .WithMessage("Incorrect custom miner's pool template format");

        RuleFor(model => model.WalletWorkerTemplate)
            .NotEmpty()
            .Matches(@"^[\x20-\x7Ea-zA-Z0-9]+$")
            .WithMessage("Incorrect custom miner's wallet or worker template format");
    }
}