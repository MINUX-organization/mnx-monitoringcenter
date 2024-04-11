using AutoMapper;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;

namespace MNX.MonitoringCenter.Monitoring.Service.Infrastructure;

/// <summary>
/// Конфигурация автомаппера.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<int, ParameterModelWithMeasureUnit>()
            .ConstructUsing((value, context)
            => ConvertToModelWithMeasureUnit(value,
                                             context.Items["DefaultMeasureUnit"].ToString()!,
                                             (string[])context.Items["MeasureUnits"]));
    }

    private static ParameterModelWithMeasureUnit ConvertToModelWithMeasureUnit(int defaultValue,
                                                                               string defaultMeasureUnit,
                                                                               string[] measureUnits)
    {
        float newValue = defaultValue;
        string newUnit = defaultMeasureUnit;

        for (int i = 1; i < measureUnits.Length; i++)
        {
            if (newValue < 1000)
            {
                break;
            }

            newValue /= 1000;
            newUnit = measureUnits[i];
        }

        return new ParameterModelWithMeasureUnit()
        {
            Value = newValue,
            MeasureUnit = newUnit
        };
    }
}
