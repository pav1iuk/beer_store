namespace BeerStore.Api.Data.Mapping
{
    using AutoMapper;
    using BeerStore.Api.Data.DTOs;
    using BeerStore.Api.Models;

    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.ProductCategories.Select(pc => pc.Category.Name)))
                .ForMember(d => d.Images, opt => opt.MapFrom(s => s.Images.Select(i => i.ImageUrl)));

            CreateMap<ProductDetail, ProductDetailDto>();
            CreateMap<ProductCreateDto, Product>();
        }
    }

}
