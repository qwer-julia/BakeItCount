using BakeItCountApi.Cn.Users;

namespace BakeItCountApi.Cn.Login
{
    public class LoginResult
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
