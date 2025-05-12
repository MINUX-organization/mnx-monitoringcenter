using FluentValidation;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

/// <summary>
/// Модель ввода пользовательского майнера.
/// </summary>
/// <param name="Name"> Наименование майнера. </param>
/// <param name="Version"> Версия майнера. </param>
/// <param name="InstallationUrl"> Установочная ссылка. </param>
/// <param name="PoolTemplate"> Шаблон блока пула. </param>
/// <param name="WalletWorkerTemplate"> Адрес кошелька и имя воркера для идентификации на пуле. </param>
/// <param name="MiningMode"> Режим майнинга. </param>
public record MinerInputModel(
    string Name,
    string Version,
    string InstallationUrl,
    DeviceTypeManufacturerCombination SupportedDevices,
    string PoolTemplate,
    string WalletWorkerTemplate,
    MiningModeEnum MiningMode);

/// <summary>
/// Валидатор параметров сущности <see cref="MinerInputModel"/>.
/// </summary>
public class MinerInputModelValidator : AbstractValidator<MinerInputModel>
{
    public MinerInputModelValidator() { }

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