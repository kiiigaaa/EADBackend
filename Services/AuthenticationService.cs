// File Name: AuthenticationService.cs
// Date : 08/10/2024
// This file defines the AuthenticationService class, which implements the IAuthenticationService interface. The class provides several methods for managing user authentication and authorization, primarily using MongoDB for storing user and role data, and ASP.NET Core Identity for user management. It also integrates JWT (JSON Web Tokens) for user authentication.

using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Configurations;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMongoCollection<Role> _roles;
        private readonly IMongoCollection<User> _users;
        public IOptions<DatabaseSettings> _databaseSettings;
        private readonly IConfiguration _configuration;

        public AuthenticationService(UserManager<User> userManager, RoleManager<Role> roleManager, IOptions<DatabaseSettings> databaseSettings, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _databaseSettings = databaseSettings;
            _configuration = configuration;

            // Initialize MongoDb client
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

            // Connect to the MongoDb database
            var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

            _roles = mongoDb.GetCollection<Role>(databaseSettings.Value.RolesCollectionName);
            _users = mongoDb.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
        }

        public async Task<string> CreateRole(RoleDto request)
        {
            try
            {
                var roleExist = await _roleManager.FindByNameAsync(request.RoleName);

                if (roleExist != null)
                {
                    return "role-exist";
                }
                else
                {
                    var appRole = new Role
                    {
                        Name = request.RoleName,
                        Description = request.Description,
                        CreatedBy = request.CreatedBy,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = request.CreatedBy,
                        ModifiedOn = DateTime.Now,
                        IsActive = true,
                    };

                    await _roleManager.CreateAsync(appRole);

                    return "role-created-succesfully";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetActiveRoles()
        {
            try
            {
                var roles = await _roles.Find(p => p.IsActive == true).ToListAsync();

                if (roles != null)
                {
                    return roles;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<RegisterResponseDto> UserRegistration(RegisterRequestDto request)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(request.Email);

                if (userExists != null)
                {
                    return new RegisterResponseDto
                    {
                        Message = "User-already-exists",
                        Success = false
                    };
                }
                else
                {
                    userExists = new User
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        Address = request.Address,
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        UserName = request.Email,
                        CreatedBy = request.CreatedBy,
                        ModifiedBy = request.CreatedBy,
                        ModifiedOn = DateTime.Now,
                        IsActive = true,
                    };

                    if(request.Role == "Customer")
                    {
                        userExists.IsActive = false;
                    }

                    var createUserResult = await _userManager.CreateAsync(userExists, request.Password);

                    if (!createUserResult.Succeeded)
                    {
                        return new RegisterResponseDto
                        {
                            Message = $"Create user failed {createUserResult?.Errors?.First()?.Description}",
                            Success = false
                        };
                    }
                    else
                    {
                        var addUserToRoleResult = await _userManager.AddToRoleAsync(userExists, request.Role);

                        if (!addUserToRoleResult.Succeeded)
                        {
                            return new RegisterResponseDto
                            {
                                Message = $"Create user succeeded but could not add user to role {addUserToRoleResult?.Errors?.First()?.Description}",
                                Success = false
                            };
                        }
                        else
                        {
                            return new RegisterResponseDto
                            {
                                Message = "user-registered-successfully",
                                Success = true
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new RegisterResponseDto
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersByRole(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                // Retrieve all users
                var allUsers = _userManager.Users.ToList();

                // Filter users by role
                var usersByRole = new List<User>();
                foreach (var user in allUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains(role.Name))
                    {
                        usersByRole.Add(user);
                    }
                }

                if (usersByRole != null)
                {
                    return usersByRole;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<IEnumerable<User>> GetAllVendors()
        //{
        //    try
        //    {
        //        var role = await _roleManager.FindByNameAsync("Customer");

        //        // Retrieve all users
        //        var allUsers = _userManager.Users.ToList();

        //        // Filter users by role
        //        var customerUsers = new List<User>();
        //        foreach (var user in allUsers)
        //        {
        //            var roles = await _userManager.GetRolesAsync(user);
        //            if (roles.Contains(role.Name))
        //            {
        //                customerUsers.Add(user);
        //            }
        //        }

        //        if (customerUsers != null)
        //        {
        //            return customerUsers;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<UserDetailResponseDto> GetUserDetailById(Guid userId)
        {
            try
            {
                var userIdString = userId.ToString();

                var user = await _userManager.FindByIdAsync(userIdString);

                if (user == null)
                {
                    return new UserDetailResponseDto
                    {
                        Message = "User-not-found",
                        Success = false
                    };
                }

                return new UserDetailResponseDto
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Address = user.Address,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new UserDetailResponseDto
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<string> UpdateUserStatus(string id, bool isActive, Guid userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());

                if (user != null)
                {
                    user.ModifiedOn = DateTime.UtcNow;
                    user.ModifiedBy = userId;
                    user.IsActive = isActive;
                    var userUpdate = await _userManager.UpdateAsync(user);

                    if (userUpdate.Succeeded)
                    {
                        return "user-update-success";
                    }
                    else
                    {
                        return "user-update-failed";
                    }
                }
                else
                {
                    return "user-not-exist";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoginResponseDto> UserLogin(LoginRequestDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null || user.IsActive == false)
                {
                    return new LoginResponseDto { Message = "invalid-email/password.", Success = false };
                }
                else
                {
                    var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

                    if (isPasswordCorrect)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.FirstName),
                            new Claim(ClaimTypes.Surname, user.LastName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        var roles = await _userManager.GetRolesAsync(user);
                        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
                        claims.AddRange(roleClaims);

                        // Pass user claims with the JWT token
                        var token = GenerateNewJasonWebToken(claims);

                        return new LoginResponseDto
                        {
                            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                            ValidTo = token.ValidTo,
                            Message = "login-successful",
                            Email = user?.Email,
                            Success = true,
                            UserId = user?.Id.ToString()
                        };
                    }
                    else
                    {
                        return new LoginResponseDto
                        {
                            Message = "invalid-email/password.",
                            Success = false
                        };
                    }
                }
                
            }
            catch (Exception ex)
            {
                return new LoginResponseDto { Success = false, Message = ex.Message };
            }
        }

        private JwtSecurityToken GenerateNewJasonWebToken(List<Claim> Claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EadAssignmentAppLoginJWTAuthenticationKey@2024"));

            var tokenObject = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(8),
                claims: Claims,
                signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
            );
            return tokenObject;
        }

        public async Task<RegisterResponseDto> UpdateUser(UpdateUserDto updateUserRequest)
        {
            try
            {
                var userIdString = updateUserRequest.UserId.ToString();

                var userDetails = await _userManager.FindByIdAsync(userIdString);

                if (userDetails == null)
                {
                    return new RegisterResponseDto
                    {
                        Message = "User-not-found",
                        Success = false
                    };
                }

                userDetails.FirstName = updateUserRequest.FirstName;
                userDetails.LastName = updateUserRequest.LastName;
                userDetails.Email = updateUserRequest.Email;
                userDetails.UserName = updateUserRequest.Email;
                userDetails.Address = updateUserRequest.Address;
                userDetails.ModifiedBy = updateUserRequest.ModifiedBy;
                userDetails.ModifiedOn = DateTime.Now;

                var updateUserResult = await _userManager.UpdateAsync(userDetails);

                if (!updateUserResult.Succeeded)
                {
                    return new RegisterResponseDto
                    {
                        Message = $"Failed to update user: {updateUserResult?.Errors?.First()?.Description}",
                        Success = false
                    };
                }
                else
                {
                    if (!string.IsNullOrEmpty(updateUserRequest.NewPassword))
                    {
                        var removePasswordResult = await _userManager.RemovePasswordAsync(userDetails);

                        if (removePasswordResult.Succeeded)
                        {
                            var addPasswordResult = await _userManager.AddPasswordAsync(userDetails, updateUserRequest.NewPassword);

                            if (!addPasswordResult.Succeeded)
                            {
                                return new RegisterResponseDto
                                {
                                    Message = $"Failed to update password: {addPasswordResult?.Errors?.First()?.Description}",
                                    Success = false
                                };
                            }
                        }
                        else if (!removePasswordResult.Succeeded)
                        {
                            return new RegisterResponseDto
                            {
                                Message = $"Failed to update password: {removePasswordResult?.Errors?.First()?.Description}",
                                Success = false
                            };
                        }
                    }

                    return new RegisterResponseDto
                    {
                        Message = "User-updated-successfully",
                        Success = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new RegisterResponseDto
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
