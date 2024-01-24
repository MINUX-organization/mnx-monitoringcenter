using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Обработчик команды сохранения пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommandHandler : IRequestHandler<SavePresetCommand, Result<Unit>>
{
    private readonly IPresetRepository _presetRepository;

    private readonly IHardwareParametersRepository _hardwareRepository;

    private readonly IMapper _mapper;

    public SavePresetCommandHandler(IPresetRepository repository,
                                    IHardwareParametersRepository hardwareRepository,
                                    IMapper mapper)
    {
        _presetRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _hardwareRepository = hardwareRepository ?? throw new ArgumentNullException(nameof(hardwareRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Unit>> Handle(SavePresetCommand request, CancellationToken cancellationToken)
    {
        if (!(await _hardwareRepository.GetGpusParameters()).Any(gpu => gpu.Name == request.GpuName))
        {
            return Result<Unit>.Invalid("GPU with this name wasn`t found");
        }

        var preset = _mapper.Map<Preset>(request.Model);
        preset.GpuName = request.GpuName;
        await _presetRepository.Save(preset);
        return Result<Unit>.SuccessfullyCreated(Unit.Value);
    }
}
