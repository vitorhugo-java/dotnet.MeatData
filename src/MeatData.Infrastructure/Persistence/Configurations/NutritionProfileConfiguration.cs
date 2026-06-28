using MeatData.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Infrastructure.Persistence.Configurations
{
    public class NutritionProfileConfiguration : IEntityTypeConfiguration<NutritionProfile>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<NutritionProfile> builder)
        {
            builder.HasKey(np => np.Id);
            builder.Property(np => np.FdclId).IsRequired().HasMaxLength(50);
            builder.Property(np => np.Calories).HasPrecision(10, 2);
            builder.Property(np => np.ProteinGrams).HasPrecision(10, 2);
            builder.Property(np => np.FatGrams).HasPrecision(10, 2);
            builder.Property(np => np.CarbsGrams).HasPrecision(10, 2);
            builder.Property(np => np.SodiumMg).HasPrecision(10, 2);
            builder.Property(np => np.Source).IsRequired().HasMaxLength(100);
            builder.HasOne(np => np.Product)
               .WithOne(p => p.NutritionProfile)
               .HasForeignKey<NutritionProfile>(np => np.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
