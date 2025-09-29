namespace BakeItCountApi.Cn.Users
{
    public class CnUser
    {

        private readonly DaoUser _userDAO;

        public CnUser(DaoUser userDAO)
        {
            _userDAO = userDAO;
        }

        public async Task<User> RegisterAsync(User user)
        {
            if (await _userDAO.EmailExistsAsync(user.Email))
                throw new InvalidOperationException("Email já cadastrado.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            return await _userDAO.AddUserAsync(user);
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _userDAO.ExistsAsync(userId);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userDAO.GetUserById(userId);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userDAO.GetAllAsync();
        }


    }
}
