using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Commands.DeleteCommand;

internal class DeleteCommandFactory
{
    private readonly Dictionary<DeleteCommandEnum, IRepository> _repositories;

    internal DeleteCommandFactory(IMainRepository _mainRepository)
    {
        _repositories = new Dictionary<DeleteCommandEnum, IRepository>()
        {
            { DeleteCommandEnum.DeleteCryptocurrencyCommand, _mainRepository.Cryptocurrencies },
            { DeleteCommandEnum.DeleteFlightSheetCommand, _mainRepository.FlightSheets },
            { DeleteCommandEnum.DeletePoolCommand, _mainRepository.Pools },
            { DeleteCommandEnum.DeletePresetCommand, _mainRepository.Presets },
            { DeleteCommandEnum.DeleteWalletCommand, _mainRepository.Wallets },
        };
    }

    internal IRepository GetRepository(DeleteCommandEnum typeCommand)
    {
        return _repositories[typeCommand];
    }
}