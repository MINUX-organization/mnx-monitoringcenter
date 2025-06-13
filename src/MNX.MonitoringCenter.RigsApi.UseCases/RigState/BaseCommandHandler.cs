using MediatR;
using Microsoft.Extensions.Logging;
using MNX.MonitoringCenter.RigsApi.GrainWrapper;

namespace MNX.MonitoringCenter.RigsApi.UseCases.RigState;

/// <summary>
/// Базовый обработчик команд.
/// </summary>
public class BaseCommandHandler
{
    private readonly IMediator _mediator;

    private readonly ILogger<BaseCommandHandler> _logger;

    ///
    public BaseCommandHandler(IMediator mediator, ILogger<BaseCommandHandler> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Обработать результат действия над ригом.
    /// </summary>
    /// <param name="result"> Результат действия над ригом. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Признак успешности действия. </returns>
    protected async Task<bool> HandleResult(RigGrainActionResult result, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in result.GetDomainEvents())
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        if (!result.IsSuccess)
        {
            foreach (var error in result.GetErrors())
            {
                _logger.LogWarning("Rig action result is error - {error}", error);
            }

            return false;
        }

        return true;
    }
}
