using Planner.API.DTOs;

namespace Planner.API.Services
{
    public interface IAccountService
    {
        public LoginResultDTO Authenticate(LoginRequestDTO request);
    }
}
