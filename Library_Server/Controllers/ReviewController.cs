using Azure;
using Library_Server.Dtos.Review;
using Library_Server.Models;
using Library_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Server.Controllers
{
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
        public async Task<ActionResult<List<Review>>> AddReview(AddReviewDto review)
        {
            try
            {
                _logger.LogInformation("Start: ReviewController/AddReview");
                return Ok(await _reviewService.AddReview(review));
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
                var response = await _reviewService.GetReviewByBookId(bookId);
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

        [HttpGet("getByUserId/{userId}")]
        public async Task<ActionResult<List<Review>>> GetReviewByUser(string userId)
        {
            try
            {
                _logger.LogInformation("Start: ReviewController/GetReviewByBook");
                var response = await _reviewService.GetReviewByUserId(userId);
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
                var response = await _reviewService.UpdateReviewContent(review);
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
                var response = await _reviewService.DeleteReview(ReviewId);
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
    }
}
