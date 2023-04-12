using AutoMapper;
using Library_Server.Dtos.Review;
using Library_Server.Dtos.WishList;
using Library_Server.Models;

namespace Library_Server.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddReviewDto, Review>();
            CreateMap<AddWishlistItemDto, WishlistItem>();
        }
    }
}
