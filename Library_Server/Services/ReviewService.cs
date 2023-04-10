using AutoMapper;
using Library_Server.DB;
using Library_Server.Dtos.Review;
using Library_Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Server.Services
{
    public class ReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ReviewService(ApplicationDbContext context, ILogger<ReviewService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<Review>>> AddReview(AddReviewDto AddReviewDto, string userId)
        {
            _logger.LogInformation("Start: ReviewService/AddReview");
            var serviceResponse = new ServiceResponse<List<Review>>();
            var review = _mapper.Map<Review>(AddReviewDto);
            review.UserId = userId;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.BookId == AddReviewDto.BookId && r.UserId == userId)
                .ToListAsync();
            _logger.LogInformation("End: ReviewService/AddReview");
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Review>>> GetReviewByBookId(string bookId, string userId)
        {
            _logger.LogInformation("Start: ReviewService/GetReviewByBookId");
            var serviceResponse = new ServiceResponse<List<Review>>();
            serviceResponse.Data = await _context.Reviews.Where(r => r.BookId == bookId && r.UserId == userId).ToListAsync();
            if (serviceResponse.Data == null || serviceResponse.Data.Count == 0)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Reviews not found.";
            }
            _logger.LogInformation("End: ReviewService/GetReviewByBookId");
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Review>>> GetReviewByUserId(string userId)
        {
            _logger.LogInformation("Start: ReviewService/GetReviewByBookId");
            var serviceResponse = new ServiceResponse<List<Review>>();
            serviceResponse.Data = await _context.Reviews.Where(r => r.UserId == userId).ToListAsync();
            if (serviceResponse.Data == null || serviceResponse.Data.Count == 0)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "Reviews not found.";
            }
            _logger.LogInformation("End: ReviewService/GetReviewByBookId");
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Review>>> DeleteReview(string reviewId, string userId)
        {
            _logger.LogInformation("Start: ReviewService/DeleteReview");
            var serviceResponse = new ServiceResponse<List<Review>>();
            var deletedReview = await _context.Reviews          
                .SingleOrDefaultAsync(r => r.Id == reviewId && r.UserId == userId);
            if (deletedReview == null)
            {
                var messageResponse = "Review not found: " + reviewId;
                _logger.LogInformation(messageResponse);
                serviceResponse.Success = false;
                serviceResponse.Message = messageResponse;
                return serviceResponse;
            }
            _context.Reviews.Remove(deletedReview);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.BookId == deletedReview.BookId)
                .ToListAsync();
            _logger.LogInformation("End: ReviewService/DeleteReview");
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Review>>> UpdateReviewContent(UpdateReviewContentDto updateReviewContentDto, string userId)
        {

            _logger.LogInformation("Start: ReviewService/UpdateReviewContent");
            var serviceResponse = new ServiceResponse<List<Review>>();

            var updatedReview = await _context.Reviews.SingleOrDefaultAsync(r => r.Id.Equals(updateReviewContentDto.Id) && r.UserId == userId);
            if (updatedReview == null)
            {
                var messageResponse = "Review not found" + updateReviewContentDto.Id;
                _logger.LogInformation(messageResponse);
                serviceResponse.Success = false;
                serviceResponse.Message = messageResponse;
                return serviceResponse;
            }

            updatedReview.Content = updateReviewContentDto.Content;
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Reviews.Where(r => r.BookId == updatedReview.BookId && r.UserId == userId).ToListAsync();
            _logger.LogInformation("End: ReviewService/DeleteReview");
            return serviceResponse;
        }
    }
}
