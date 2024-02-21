using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets
{
    public class PresetCommandTestCase
    {
        public static IEnumerable<TestCaseData> GetCommandTestCases()
        {
            yield return new TestCaseData(CreateCorrectPresetModel());
            yield return new TestCaseData(CreateIncorrectPresetModel());
        }

        private static PresetModel CreateCorrectPresetModel()
        {
            var memoryClock = 1313;
            var coreClock = 2235;
            var powerLimit = 150;
            var criticalTemperature = 105;
            var fanSpeed = 99;

            return new PresetModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);
        }

        private static PresetModel CreateIncorrectPresetModel()
        {
            var memoryClock = -1;
            var coreClock = -1;
            var powerLimit = -1;
            var criticalTemperature = -1;
            var fanSpeed = -1;

            return new PresetModel(
                memoryClock, coreClock, powerLimit,
                criticalTemperature, fanSpeed);
        }
    }
}
