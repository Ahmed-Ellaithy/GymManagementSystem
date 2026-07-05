using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.FluentConfigurations
{
    internal class TrainerConfiguration : GymUserConfiguration<Trainer> , IEntityTypeConfiguration<Trainer>
    {

        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(x => x.CreatedAt)
                .HasColumnName("HireDate")
                .HasDefaultValueSql("GETDATE()");


            // importat .. call the base class's configure method to apply the configurations defined in the base class
            base.Configure(builder);
        }
    }
}
