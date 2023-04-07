namespace Library_Server.Dtos.Review
{
    public class AddReviewDto
    {
        public string UserId { get; set; }
        public string BookId { get; set; }
        public string Content { get; set; }
    }
}
