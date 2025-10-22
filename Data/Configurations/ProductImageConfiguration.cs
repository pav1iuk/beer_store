namespace BeerStore.Api.Data.Configurations
{
    using BeerStore.Api.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(i => i.ImageId);

            builder.Property(i => i.ImageUrl)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(i => i.IsMain)
                .HasDefaultValue(false);

            builder.HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
