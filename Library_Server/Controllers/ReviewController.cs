using Library_Server.Dtos.Review;
using Library_Server.Models;
using Library_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ReviewService _reviewService;

        public ReviewController(ILogger<ReviewController> logger, ReviewService reviewService)
        {
            _logger = logger;
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<ActionResult<List<Review>>> AddReview(AddReviewDto addReviewDto)
        {
            try
            {
                _logger.LogInformation("Start: ReviewController/AddReview");
                return Ok(await _reviewService.AddReview(addReviewDto, GetUserIdFromToken()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewController/AddReview", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getByBookId/{bookId}")]
        public async Task<ActionResult<List<Review>>> GetReviewByBook(string bookId)
        {
            try
            {
                _logger.LogInformation("Start: ReviewController/GetReviewByBook");
                var response = await _reviewService.GetReviewByBookId(bookId, GetUserIdFromToken());
                if (response.Data == null)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewController/GetReviewByBook", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getByUserId")]
        public async Task<ActionResult<List<Review>>> GetReviewByUser()
        {
            try
            {
                _logger.LogInformation("Start: ReviewController/GetReviewByBook");
                var response = await _reviewService.GetReviewByUserId(GetUserIdFromToken());
                if (response.Data == null)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewController/GetReviewByBook", ex.Message);
                return BadRequest(ex.Message);

            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateReview(UpdateReviewContentDto review)
        {
            try
            {
                _logger.LogInformation("Start: ReviewController/UpdateReview");
                var response = await _reviewService.UpdateReviewContent(review, GetUserIdFromToken());
                if (response.Data == null)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewController/UpdateReview", ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{ReviewId}")]
        public async Task<ActionResult<ServiceResponse<List<Review>>>> DeleteReview(string ReviewId)
        {
            try
            {
                _logger.LogInformation("Start: ReviewController/DeleteReview");
                var response = await _reviewService.DeleteReview(ReviewId, GetUserIdFromToken());
                if (response.Data == null)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in ReviewController/DeleteReview", ex.Message);
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
