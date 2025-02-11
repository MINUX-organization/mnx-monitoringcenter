namespace MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

/// <summary>
/// Модель алгоритма с относительными наименованиями для майнеров.
/// </summary>
/// <param name="FullName"> Пользовательское имя алгоритма. </param>
/// <param name="Bindings">
/// Список привязок относительных 
/// имен алгоритма к майнерам.
/// </param>
public record AlgorithmBindingModel(string FullName,
                                    List<RelativeNameBindingModel> Bindings);
