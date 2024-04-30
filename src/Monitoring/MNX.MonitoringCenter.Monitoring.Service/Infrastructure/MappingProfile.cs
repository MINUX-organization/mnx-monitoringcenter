using AutoMapper;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.Service.Messages.Models;
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

        CreateMap<RigDynamicData, RigDynamicDataModel>();

        CreateMap<CoinStatistics, FlightSheetCoin>()
            .ForMember(x => x.HashRate,
                       y => y.MapFrom(opt => ConvertToModelWithMeasureUnit(opt.HashRate,
                                                                           "H/s", // todo: вынести
                                                                           new string[] { "H/s", "KH/s", "MH/s", "TH/s" })));
    }

    private static ParameterModelWithMeasureUnit ConvertToModelWithMeasureUnit(int defaultValue,
                                                                               string defaultMeasureUnit,
                                                                               string[] measureUnits)
    {
        double newValue = defaultValue;
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
            Value = Math.Round(newValue, 2),
            MeasureUnit = newUnit
        };
    }
}
