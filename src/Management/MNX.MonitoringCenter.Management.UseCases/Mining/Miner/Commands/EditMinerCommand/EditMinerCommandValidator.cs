using FluentValidation;
using FluentValidation.Validators;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;

public class EditMinerCommandValidator : AbstractValidator<EditMinerCommand>
{
    public EditMinerCommandValidator(IMinerRepository minerRepository)
    {
        RuleFor(x => x)
            .SetAsyncValidator(new EditMinerCommandPropertyValidator(minerRepository));

        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Miner data is required!")
            .SetValidator(cmd => new MinerInputModelValidator(minerRepository, cmd.UserId));
    }

    private class EditMinerCommandPropertyValidator : IAsyncPropertyValidator<EditMinerCommand, EditMinerCommand>
    {
        private readonly IMinerRepository _minerRepository;

        public EditMinerCommandPropertyValidator(IMinerRepository minerRepository)
        {
            _minerRepository = minerRepository ?? throw new ArgumentNullException(nameof(minerRepository));
        }

        public async Task<bool> IsValidAsync(
            ValidationContext<EditMinerCommand> context, EditMinerCommand value, CancellationToken cancellation)
        {
            var miner = await _minerRepository.GetMinerById(value.MinerId, cancellation);

            if (miner == null)
            {
                context.AddFailure("model", $"Miner with id equaled {value.MinerId} was not found!");
                return false;
            }

            if (miner.OwnerId != value.UserId)
            {
                context.AddFailure("OwnerId", $"Miner with id equaled {value.MinerId} is owned by another user!");
                return false;
            }

            return true;
        }

        public string GetDefaultMessageTemplate(string errorCode)
        {
            return "A value for `{PropertyName}` is not valid";
        }

        public string Name { get; } = nameof(EditMinerCommandPropertyValidator);
    }
}