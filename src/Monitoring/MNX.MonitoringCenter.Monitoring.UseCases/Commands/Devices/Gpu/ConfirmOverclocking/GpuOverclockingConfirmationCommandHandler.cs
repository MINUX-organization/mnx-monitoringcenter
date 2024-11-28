using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Gpu;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Devices.Gpu.ConfirmOverclocking;

/// <summary>
/// Обработчик команды подтверждения установки разгона для видеокарты.
/// </summary>
internal class GpuOverclockingConfirmationCommandHandler : IRequestHandler<GpuOverclockingConfirmationCommand, Result<Unit>>
{
    private readonly IMapper _mapper;

    private readonly IGpuRepository _gpuRepository;

    public GpuOverclockingConfirmationCommandHandler(IMapper mapper, IGpuRepository gpuRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _gpuRepository = gpuRepository ?? throw new ArgumentNullException(nameof(gpuRepository));
    }

    public async Task<Result<Unit>> Handle(GpuOverclockingConfirmationCommand request, CancellationToken cancellationToken)
    {
        var overclocking = _mapper.Map<GpuOverclocking>(request.Overclocking);
        await _gpuRepository.SetOverclocking(request.CardId, overclocking);
        return Result<Unit>.Empty();
    }
}
