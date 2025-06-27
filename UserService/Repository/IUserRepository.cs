
using SharedLibrary.Response;
using UserService.Model;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        Task<ResponseBody> CreateUser(EventPlanner eventPlannerObj);
        Task<EventPlanner> CheckUserExistOrNot(string email,string password);
    }
}
