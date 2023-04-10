using Library_Server.Dtos.WishList;
using Library_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly WishlistService _wishlistService;

        public WishlistController(ILogger<WishlistController> logger, WishlistService wishlistService)
        {
            _logger = logger;
            _wishlistService = wishlistService;
        }

        [HttpPost]
        public async Task<ActionResult<List<AddWishlistItemDto>>> AddBookToUserWishlist(AddWishlistItemDto addWishlistItemDto)
        {
            try
            {
                _logger.LogInformation("Start: WishlistController/AddBookToUserWishlist");
                return Ok(await _wishlistService.AddWishListItem(addWishlistItemDto, GetUserIdFromToken()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in WishlistController/AddBookToUserWishlist", ex.Message);
                return BadRequest(ex.Message);
            }
        } 


        [HttpGet]
        public async Task<ActionResult<List<AddWishlistItemDto>>> GetUserBooksWishlist()
        {
            try
            {
                _logger.LogInformation("Start: WishlistController/GetUserBooksWishlist");
                return Ok(await _wishlistService.GetWishlistByUserId(GetUserIdFromToken()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in WishlistController/GetUserBooksWishlist", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<List<AddWishlistItemDto>>> DeleteBookFromUserWishlist(DeleteWishlistItem deleteWishlistItem)
        {
            try
            {
                _logger.LogInformation("Start: WishlistController/DeleteBookFromUserWishlist");
                return Ok(await _wishlistService.DeleteWishlistItem(deleteWishlistItem, GetUserIdFromToken()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in WishlistController/DeleteBookFromUserWishlist", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private string GetUserIdFromToken()
        {
            if (HttpContext.Items.TryGetValue("UserId", out var userIdObj))
                return (userIdObj.ToString());

            return string.Empty;
        }
    }
}
