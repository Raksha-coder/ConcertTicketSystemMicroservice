using Microsoft.Data.SqlClient;
using SharedLibrary.Models;
using SharedLibrary.Response;
using UserService.Data;
using UserService.Model;

namespace UserService.Repository
{
    public class UserRepository :IUserRepository
    {
        private readonly UserdbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(UserdbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

      

        public async Task<ResponseBody> CreateUser(EventPlanner eventPlannerObj)
        {
            await _context.EventPlanners.AddAsync(eventPlannerObj);
            await _context.SaveChangesAsync();
            _logger.LogInformation("user created successfully.");
            return new ResponseBody(true, "user created successfully.");
        }

        public async Task<EventPlanner> CheckUserExistOrNot(string email, string password)
        {

            string encryptedEmail = CommonMethods.Encrypt(email);
            string encryptedPassword = CommonMethods.Encrypt(password);

            var isExist = _context.EventPlanners
                .AsEnumerable()
                .FirstOrDefault(e => e.Email == encryptedEmail && e.PasswordHash == encryptedPassword);

            if (isExist != null)
            {
                return isExist;
            }
            else
            {
                return null;
            }
        }
    }
}
