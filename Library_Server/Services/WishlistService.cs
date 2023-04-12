using AutoMapper;
using Library_Server.DB;
using Library_Server.Dtos.WishList;
using Library_Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<ServiceResponse<List<WishlistItem>>> AddWishListItem(AddWishlistItemDto addWishlistItemDto, string userId)
        {
            _logger.LogInformation("Start: WishlistService/AddWishListItem");
            var serviceResponse = new ServiceResponse<List<WishlistItem>>();
            var wishlistItem = _mapper.Map<WishlistItem>(addWishlistItemDto);
            wishlistItem.UserId = userId;
            var isDuplicatedWishlistItem = _context.Wishlists.Where(w => w.BookId == addWishlistItemDto.BookId && w.UserId == userId);
            if (!isDuplicatedWishlistItem.IsNullOrEmpty())
            {
                _logger.LogError("Error: WishlistService/AddWishListItem: Duplicate BookId", addWishlistItemDto.BookId);
                throw new Exception($"Error: WishlistService/AddWishListItem: Duplicate BookId = {addWishlistItemDto.BookId}");
            }
            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();

            serviceResponse.Data = _context.Wishlists.AsNoTracking().Where(w => w.UserId == wishlistItem.UserId).ToList();
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

        public async Task<ServiceResponse<List<WishlistItem>>> DeleteWishlistItem(DeleteWishlistItem deleteWishlistItem, string userId)
        {
            _logger.LogInformation("Start: WishlistService/DeleteWishlistItem");
            var serviceResponse = new ServiceResponse<List<WishlistItem>>();
            var wishlistItem = await _context.Wishlists.SingleOrDefaultAsync(w => w.UserId == userId && w.BookId == deleteWishlistItem.BookId);

            if (wishlistItem == null)
            {
                var messageResponse = "Book " + deleteWishlistItem.BookId + " not found in user id " + userId + " wishlist.";
                _logger.LogError(messageResponse);
                serviceResponse.Success = false;
                serviceResponse.Message = messageResponse;
                _logger.LogInformation("End: WishlistService/DeleteWishlistItem");
                return serviceResponse;
            }

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Wishlists
                .AsNoTracking()
                .Where(w => w.UserId == userId)
                .ToListAsync();

            _logger.LogInformation("End: WishlistService/DeleteWishlistItem");
            return serviceResponse;
        }
    }
}
