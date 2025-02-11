using FluentValidation;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands;

/// <summary>
/// Валидатор класса <see cref="AlgorithmBindingModel"/>.
/// </summary>
public class AlgorithmModelValidator : AbstractValidator<AlgorithmBindingModel>
{
    public AlgorithmModelValidator()
    {
        RuleFor(model => model.FullName)
            .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Algorithm name is required")
            .Matches(@"^[A-Za-z0-9-_./\\s]+$")
                .WithMessage("Incorrect algorithm name format");

        RuleFor(model => model.Bindings)
            .NotNull().WithMessage("Algorithm bindings list cannot be NULL")
            .ForEach(x => x.SetValidator(new BindingValidator()));
    }

    private class BindingValidator : AbstractValidator<RelativeNameBindingModel>
    {
        public BindingValidator()
        {
            RuleFor(bind => bind.RelativeName)
                .Must(name => !string.IsNullOrWhiteSpace(name))
                    .WithMessage("Relative algorithm name is required")
                .Matches(@"^[A-Za-z0-9-_./\\s]+$")
                    .WithMessage("Incorrect relative algorithm name format");

            RuleFor(bind => bind.MinerId)
                .NotEmpty().WithMessage("Miner identifier cannot be NULL");
        }
    }
}