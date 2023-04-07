using AutoMapper;
using Library_Server.DB;
using Library_Server.Dtos.WishList;
using Library_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Server.Services
{
    public class WishlistService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public WishlistService(ApplicationDbContext context, ILogger<WishlistService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<WishlistItem>>> AddWishListItem(AddWishlistItemDto addWishlistItemDto)
        {
            _logger.LogInformation("Start: WishlistService/AddWishListItem");
            var serviceResponse = new ServiceResponse<List<WishlistItem>>();
            var wishListItem = _mapper.Map<WishlistItem>(addWishlistItemDto);

            _context.Wishlists.Add(wishListItem);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _context.Wishlists.Where(w => w.UserId == wishListItem.UserId).ToList();
            _logger.LogInformation("End: WishlistService/AddWishListItem");
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<WishlistItem>>> GetWishlistByUserId(string userId)
        {
            _logger.LogInformation("Start: WishlistService/GetWishlistByUserId");
            var serviceResponse = new ServiceResponse<List<WishlistItem>>();

            serviceResponse.Data = await _context.Wishlists.Where(w => w.UserId == userId).ToListAsync();

            if (serviceResponse.Data == null)
            {
                var messageResponse = "User " + userId + "wishlist not found.";
                _logger.LogInformation(messageResponse);
                serviceResponse.Success = false;
                serviceResponse.Message = messageResponse;
                _logger.LogInformation("End: WishlistService/GetWishlistByUserId");
                return serviceResponse;
            }

            _logger.LogInformation("End: WishlistService/GetWishlistByUserId");
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<WishlistItem>>> DeleteWishlistItem(DeleteWishlistItem deleteWishlistItem)
        {
            _logger.LogInformation("Start: WishlistService/DeleteWishlistItem");
            var serviceResponse = new ServiceResponse<List<WishlistItem>>();

            var wishlistItem = await _context.Wishlists.SingleOrDefaultAsync(w => w.UserId == deleteWishlistItem.UserId && w.BookId == deleteWishlistItem.BookId);

            if (wishlistItem == null)
            {
                var messageResponse = "Book " + deleteWishlistItem.BookId + "not found in user id " + deleteWishlistItem.UserId + "wishlist.";
                _logger.LogInformation(messageResponse);
                serviceResponse.Success = false;
                serviceResponse.Message = messageResponse;
                _logger.LogInformation("End: WishlistService/DeleteWishlistItem");
                return serviceResponse;
            }

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Wishlists.Where(w => w.UserId == deleteWishlistItem.UserId).ToListAsync();
            _logger.LogInformation("End: WishlistService/DeleteWishlistItem");
            return serviceResponse;
        }
    }
}
