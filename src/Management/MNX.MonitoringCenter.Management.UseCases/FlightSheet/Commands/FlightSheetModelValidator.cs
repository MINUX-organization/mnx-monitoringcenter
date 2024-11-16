using AutoMapper;
using FluentValidation;
using FluentValidation.Validators;
using MNX.MonitoringCenter.Management.Core.Miner.Configs;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands;

/// <summary>
/// Валидатор модели полётного листа.
/// </summary>
internal class FlightSheetModelValidator : AbstractValidator<FlightSheetInputModel>
{
    public FlightSheetModelValidator(Guid userId,
                                     IMapper mapper,
                                     IMinerRepository minerRepository,
                                     IWalletRepository walletRepository,
                                     IPoolRepository poolRepository)
    {
        RuleFor(model => model.Name)
            .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Name is required!");

        RuleFor(model => model.Targets)
            .NotNull()
                .WithMessage("Targets is required!")
            .NotEmpty()
                .WithMessage("Targets is required!");

        RuleForEach(model => model.Targets)
            .NotNull()
                .WithMessage("Target is cannot nullable!")
            .SetAsyncValidator(new FlightSheetTargetModelValidator(userId,
                                                                   mapper,
                                                                   minerRepository,
                                                                   walletRepository,
                                                                   poolRepository));
    }

    private class FlightSheetTargetModelValidator
        : IAsyncPropertyValidator<FlightSheetInputModel, FlightSheetTargetInputModel>
    {
        private readonly Guid _userId;

        private readonly IMapper _mapper;

        private readonly IMinerRepository _minerRepository;

        private readonly IWalletRepository _walletRepository;

        private readonly IPoolRepository _poolRepository;

        /// <inheritdoc/>
        public string Name { get => nameof(FlightSheetTargetModelValidator); }

        public FlightSheetTargetModelValidator(Guid userId,
                                               IMapper mapper,
                                               IMinerRepository minerRepository,
                                               IWalletRepository walletRepository,
                                               IPoolRepository poolRepository)
        {
            _userId = userId;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
            _poolRepository = poolRepository ?? throw new ArgumentNullException(nameof(poolRepository));
        }

        /// <inheritdoc/>
        public string GetDefaultMessageTemplate(string errorCode)
        {
            return "A value for {PropertyName} is not valid";
        }

        /// <inheritdoc/>
        public async Task<bool> IsValidAsync(ValidationContext<FlightSheetInputModel> context,
                                             FlightSheetTargetInputModel value,
                                             CancellationToken cancellation)
        {
            var miner = await _minerRepository.GetMinerById(value.MinerId);

            if (miner == null)
            {
                context.AddFailure($"Miner with id equaled {value.MinerId} was not found!");
                return false;
            }

            var miningConfig = _mapper.Map<BaseMiningConfig>(value.MiningConfig);
            if (!await ValidateMiningConfig(miningConfig, context))
            {
                return false;
            }

            if (!miner.IsSupportConfigs(miningConfig, out IReadOnlyCollection<string> errors))
            {
                foreach (var error in errors)
                {
                    context.AddFailure(error);
                }

                return false;
            }

            return true;
        }

        private async Task<bool> ValidateMiningConfig(BaseMiningConfig config,
                                                      ValidationContext<FlightSheetInputModel> context)
        {
            var isValid = true;

            foreach (var coinConfig in config.CoinConfigs)
            {
                if (!await ValidateCoinConfig(coinConfig, context))
                {
                    isValid = false;
                }
            }

            if (!config.IsValid(out IReadOnlyCollection<string> configErrors))
            {
                foreach (var error in configErrors)
                {
                    context.AddFailure(error);
                }

                return false;
            }

            return isValid;
        }

        private async Task<bool> ValidateCoinConfig(MiningCoinConfig config,
                                                    ValidationContext<FlightSheetInputModel> context)
        {
            var isValid = true;

            var wallet = await _walletRepository.GetAvailableById(config.WalletId, _userId);
            if (wallet == null)
            {
                context.AddFailure($"Wallet with id equaled {config.WalletId} was not found!");
                isValid = false;
            }

            var pool = await _poolRepository.GetAvailableById(config.PoolId, _userId);
            if (pool == null)
            {
                context.AddFailure($"Pool with id equaled {config.PoolId} was not found!");
                isValid = false;
            }

            config.Pool = pool;
            config.Wallet = wallet;

            if (!config.IsValid(out IReadOnlyCollection<string> errors))
            {
                foreach (var error in errors)
                {
                    context.AddFailure(error);
                }

                isValid = false;
            }

            return isValid;
        }
    }
}
