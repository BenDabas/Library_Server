namespace Library_Server.Dtos.User
{
    public class LogoutRequestDto
    {
        public string Token { get; set; }

        public LogoutRequestDto(string token)
        {
            Token = token;
        }
    }
}
