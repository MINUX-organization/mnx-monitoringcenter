using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateMinerCommand;

public class CreateMinerCommandValidator : AbstractValidator<CreateMinerCommand>
{
    public CreateMinerCommandValidator(IMinerRepository minerRepository)
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Miner data is required!")
            .SetValidator(cmd => new MinerInputModelValidator(minerRepository, cmd.UserId));
    }
}