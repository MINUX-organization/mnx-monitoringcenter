using FluentValidation;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

public record MinerInputModel(
    string Name,
    string? Version,
    string InstallationUrl,
    DeviceTypeManufacturerCombination SupportedDevices,
    string PoolTemplate,
    string WalletWorkerTemplate);

public class MinerInputModelValidator : AbstractValidator<MinerInputModel>
{
    public MinerInputModelValidator()
    {
        RuleFor(model => model.Name)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9\-_./ ]+$");

        RuleFor(model => model.Version)
            .NotEqual("")
            .Matches(@"^[a-zA-Z0-9\-_./ ]+$");

        RuleFor(model => model.InstallationUrl)
            .NotEmpty();

        RuleFor(model => model.SupportedDevices)
            .NotEqual(DeviceTypeManufacturerCombination.None);

        RuleFor(model => model.PoolTemplate)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9\-_./% ]+$")
            .Matches("(%POOL%|%POOL_TEMPLATE%)");

        RuleFor(model => model.WalletWorkerTemplate)
            .NotEmpty()
            .Matches(@"^[a-zA-Z0-9\-_./% ]+$")
            .Matches("(%WALLET%|%WALL%)")
            .Matches("%WORKER%");
    }
}