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
    private readonly IPresetRepository _repository;

    private readonly IMapper _mapper;

    public SavePresetCommandHandler(IPresetRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Unit>> Handle(SavePresetCommand request, CancellationToken cancellationToken)
    {
        // TODO: проверка на существование серии GPU

        var preset = _mapper.Map<Preset>(request.Model);
        preset.GpuName = request.GpuName;
        await _repository.Save(preset);
        return Result<Unit>.SuccessfullyCreated(Unit.Value);
    }
}
