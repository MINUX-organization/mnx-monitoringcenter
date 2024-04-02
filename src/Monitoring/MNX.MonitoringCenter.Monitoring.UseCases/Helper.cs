using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.UseCases;

/// <summary>
/// Класс помощник.
/// </summary>
public static class Helper
{
    /// <summary>
    /// Проецирует данные из бд на список динамических данных ригов.
    /// </summary>
    /// <param name="rigsFromRepository"> Список из бд. </param>
    /// <param name="rigDynamicData"> Список динамических данных ригов. </param>
    /// <returns> Спроецированный список. </returns>
    public static async Task<List<RigDynamicData?>> MapDbDataToRigDataAsync
        (IAsyncEnumerable<Rig> rigsFromRepository, List<RigDynamicData?> rigDynamicData)
    {
        var newRigDynamicData = new List<RigDynamicData?>();

        int index;
        var counter = 0;

        await foreach (var rig in rigsFromRepository)
        {
            var rigData = rigDynamicData.FirstOrDefault(r => r?.Id == rig.Id);
            if (rigData != null)
            {
                index = rigDynamicData.IndexOf(rigData);
                newRigDynamicData.Add(rigDynamicData[index]);
                newRigDynamicData.Last().Index = counter + 1;
            }
            else
            {
                newRigDynamicData.Add(null);
            }

            counter++;
        }

        return newRigDynamicData;
    }

    /// <summary>
    /// Проецирует данные из бд на список состояния ригов.
    /// </summary>
    /// <param name="rigsFromRepository"> Список из бд. </param>
    /// <param name="rigState"> Список состояния ригов. </param>
    /// <returns> Спроецированный список. </returns>
    public static async Task<List<RigState?>> MapDbDataToRigDataAsync
        (IAsyncEnumerable<Rig> rigsFromRepository, List<RigState?> rigState)
    {
        var newRigState = new List<RigState?>();

        int index;
        var counter = 0;

        await foreach (var rig in rigsFromRepository)
        {
            var rigData = rigState.FirstOrDefault(r => r?.Id == rig.Id);
            if (rigData != null)
            {
                index = rigState.IndexOf(rigData);
                newRigState.Add(rigState[index]);
            }
            else
            {
                newRigState.Add(null);
            }

            counter++;
        }

        return newRigState;
    }
}
