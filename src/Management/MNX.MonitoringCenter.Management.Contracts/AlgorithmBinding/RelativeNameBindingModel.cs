namespace MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

/// <summary>
/// Модель привязки относительного наименования алгоритма к майнеру.
/// </summary>
/// <param name="RelativeName"> Относительное наименование алгоритма. </param>
/// <param name="MinerId"> Идентификатор майнера. </param>
public record RelativeNameBindingModel(string RelativeName,
                                       Guid MinerId);
