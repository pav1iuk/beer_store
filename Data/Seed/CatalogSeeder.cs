namespace BeerStore.Api.Data.Seed
{
    using System;
    using BeerStore.Api.Models;
    using Microsoft.EntityFrameworkCore;
    public static class CatalogSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Світле" },
                new Category { CategoryId = 2, Name = "Темне" },
                new Category { CategoryId = 3, Name = "IPA" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Sku = "LAG-500",
                    Name = "Lager 500ml",
                    Price = 2.50m,
                    StockQuantity = 100,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    ProductId = 2,
                    Sku = "IPA-330",
                    Name = "IPA 330ml",
                    Price = 3.20m,
                    StockQuantity = 50,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<ProductDetail>().HasData(
                new ProductDetail
                {
                    DetailID = 1,
                    ProductId = 1,
                    Description = "Класичний лагер з м'яким смаком",
                    AlcoholPercent = 4.7m,
                    VolumeMl = 500,
                    Country = "Україна"
                },
                new ProductDetail
                {
                    DetailID = 2,
                    ProductId = 2,
                    Description = "Свіжий ароматний IPA з цитрусовими нотками",
                    AlcoholPercent = 6.0m,
                    VolumeMl = 330,
                    Country = "Україна"
                }
            );

            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory { ProductId = 1, CategoryId = 1 },
                new ProductCategory { ProductId = 2, CategoryId = 3 }
            );

            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage
                {
                    ImageId = 1,
                    ProductId = 1,
                    ImageUrl = "https://example.com/lager.jpg",
                    IsMain = true
                },
                new ProductImage
                {
                    ImageId = 2,
                    ProductId = 2,
                    ImageUrl = "https://example.com/ipa.jpg",
                    IsMain = true
                }
            );
        }
    }
}
