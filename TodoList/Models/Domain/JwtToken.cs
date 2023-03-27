
namespace TodoList.Models.Domain
{
    public class JwtToken
    {
        public class UserToken
        {
            public string Token { get; set; }
            public DateTime Expiration { get; set; }
        }
    }
}
