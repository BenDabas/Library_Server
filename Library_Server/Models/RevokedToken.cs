namespace Library_Server.Models
{
    public class RevokedToken
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public RevokedToken(string token, DateTime expiryDate)
        {
            Id = Guid.NewGuid().ToString();
            Token = token;
            ExpiryDate = expiryDate;
        }
    }
}
