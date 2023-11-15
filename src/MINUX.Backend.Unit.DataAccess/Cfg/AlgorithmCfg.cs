using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.DataAccess.Cfg;

public class AlgorithmCfg : IEntityTypeConfiguration<Algorithm>
{
    public void Configure(EntityTypeBuilder<Algorithm> builder)
    {
        builder.HasKey(x => x.Name);
    }
}