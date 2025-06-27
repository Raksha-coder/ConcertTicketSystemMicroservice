using UserService.DTO;
using SharedLibrary.Response;
namespace UserService.Service
{
    public interface IUserServices
    {
        Task<ResponseBody> RegisterUserAsync(UserRegisterRequestDto request);
        Task<ResponseBody> LoginUserAsync(UserRegisterRequestDto request);

    }
}
