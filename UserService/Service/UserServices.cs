using SharedLibrary.Models;
using UserService.DTO;
using UserService.Model;
using UserService.Repository;
using SharedLibrary.Response;
namespace UserService.Service
{
    public class UserServices :  IUserServices
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UserServices> _logger;
        private readonly IConfiguration _config;

        public UserServices(IUserRepository userRepo, ILogger<UserServices> logger, IConfiguration config)
        {
            _userRepo = userRepo;
            _logger = logger;
            _config = config;
        }

        public async Task<ResponseBody> RegisterUserAsync(UserRegisterRequestDto request)
        {
            var EventPlannerObj = new EventPlanner()
            {
                Email = CommonMethods.Encrypt(request.Email),
                PasswordHash = CommonMethods.Encrypt(request.PasswordHash),
                RoleId = request.RoleId ?? 0,
                FullName = CommonMethods.Encrypt(request.FullName),
                Status = 1,
                CreatedDate = DateTime.UtcNow
                
            };

            var response = await _userRepo.CreateUser(EventPlannerObj);
            if (response.Success) {
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "Error while creating user";
                return response;
            }

        }

        public async Task<ResponseBody> LoginUserAsync(UserRegisterRequestDto request)
        {
            if (!string.IsNullOrWhiteSpace(request.Email) && !string.IsNullOrWhiteSpace(request.PasswordHash)) { 
               
                var decryptEmail = CommonMethods.DecryptCrypto(request.Email);
                var decryptPassword = CommonMethods.DecryptCrypto(request.PasswordHash);

                var response =  await _userRepo.CheckUserExistOrNot(decryptEmail, decryptPassword);

                //create jwt with claims if user present 
                if (response != null)
                {
                    var token = JwtHelper.GenerateJwtToken(decryptEmail, CommonMethods.Decrypt(response.FullName), response.RoleId, response.Id, _config);
                    return new ResponseBody(true, "Login successful", token);
                }
                else
                {
                    return new ResponseBody(false, "You are not register, please register yourself");
                }
            }

            return new ResponseBody(false,"Email or password cannot be empty");
        }

    }
}
