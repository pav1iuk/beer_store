namespace BeerStore.Api.Data.Configurations
{
    using BeerStore.Api.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
    {
        public void Configure(EntityTypeBuilder<ProductDetail> builder)
        {
            builder.ToTable("ProductDetails");

            builder.HasKey(d => d.DetailID);

            builder.Property(d => d.Description)
                .HasMaxLength(500);

            builder.Property(d => d.AlcoholPercent)
                .HasColumnType("decimal(4,2)");

            builder.Property(d => d.VolumeMl)
                .HasDefaultValue(500);

            builder.Property(d => d.Country)
                .HasMaxLength(100);

            builder.HasOne(d => d.Product)
                .WithOne(p => p.Detail)
                .HasForeignKey<ProductDetail>(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
