using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MNX.MonitoringCenter.Management.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNX.MonitoringCenter.Management.DataAccess.Cfg;

internal class FlightSheetCfg : IEntityTypeConfiguration<FlightSheet>
{
    public void Configure(EntityTypeBuilder<FlightSheet> builder)
    {
        builder.HasIndex(x => x.UserId);
    }
}