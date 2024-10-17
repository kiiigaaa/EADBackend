// File: IAuthenticationService.cs
// Date : 08/10/2024
// Description: Interface for authentication-related services, providing methods 
// for role creation, user registration, login, and status management.

using WebApi.Dto;
using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> CreateRole(RoleDto request);

        Task<IEnumerable<Role>> GetActiveRoles();

        Task<RegisterResponseDto> UserRegistration(RegisterRequestDto request);

        Task<IEnumerable<User>> GetAllUsersByRole(string roleName);

        Task<string> UpdateUserStatus(string id, bool isActive, Guid userId);

        Task<LoginResponseDto> UserLogin(LoginRequestDto request);

        Task<RegisterResponseDto> UpdateUser(UpdateUserDto updateUserRequest);

        Task<UserDetailResponseDto> GetUserDetailById(Guid userId);
    }
}
