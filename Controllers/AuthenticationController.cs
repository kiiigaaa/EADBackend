// File : AuthenticationController.cs
// Date : 08/10/2024
// This controller handles user authentication-related operations in the WebApi application. It provides endpoints for role creation, user registration, login, and user status updates, implementing methods defined in the IAuthenticationService interface.

using Microsoft.AspNetCore.Mvc;
using WebApi.Constant;
using WebApi.Dto;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

            private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("create-role")]
        public async Task<IActionResult> CreateRole(RoleDto request)
        {
            try
            {
                var result = await _authenticationService.CreateRole(request);

                if (result == "role-exist")
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgRoleAlreadyExist
                    });
                }
                else if (result == "role-created-succesfully")
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgProductCreationSuccess
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgInternalServerError
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpGet]
        [Route("active-roles")]
        public async Task<IActionResult> GetActiveRoles()
        {
            try
            {
                var roles = await _authenticationService.GetActiveRoles();

                if (roles != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgRolesRetrievalSuccess,
                        Data = roles
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgCategoryDoesNotExist
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegistration(RegisterRequestDto request)
        {
            try
            {
                var result = await _authenticationService.UserRegistration(request);

                if (result.Success == false)
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = result.Message,
                    });
                }
                else if (result.Success == true && result.Message == "user-registered-successfully")
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgUserCreationSuccess
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgInternalServerError
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpGet]
        [Route("users-by-role")]
        public async Task<IActionResult> GetAllUsersByRole(string roleName)
        {
            try
            {
                var usersByrole = await _authenticationService.GetAllUsersByRole(roleName);

                if (usersByrole != null)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgUsersByroleRetrievalSuccess,
                        Data = usersByrole
                    });
                }
                else
                {
                    return NotFound(new ResponseDto
                    {
                        Code = StatusCodes.Status404NotFound,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgUsersByroleDoesNotExist
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpGet]
        [Route("user-details")]
        public async Task<IActionResult> GetUserDetailById(Guid userId)
        {
            try
            {
                var result = await _authenticationService.GetUserDetailById(userId);

                if (result.Success == false && result.Message == "User-not-found")
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgUserNotExist
                    });
                }
                else if (result.Success == true)
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgUserDetailsRetrievalSuccess,
                        Data = result
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgInternalServerError
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpPut]
        [Route("update-user")]
        public async Task<IActionResult> UpdateUserStatus(string id, bool isActive, Guid userID)
        {
            try
            {
                var result = await _authenticationService.UpdateUserStatus(id, isActive, userID);

                if (result == "user-not-exist")
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgUserAlreadyExist
                    });
                }
                else if (result == "user-update-success")
                {

                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgUserUpdateSuccess
                    });
                }
                else if (result == "user-update-failed")
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgUserUpdateFailed
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgInternalServerError
                    });
                }
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            try
            {
                var result = await _authenticationService.UserLogin(request);

                if (result.Success == false && result.Message == "invalid-email/password.")
                {
                    return Unauthorized(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = result.Message,
                    });
                }
                else if (result.Success == true && result.Message == "login-successful")
                {
                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgUserLoginSuccess,
                        Data = new { token = result.AccessToken, expiration = result.ValidTo  }
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgInternalServerError
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }

        [HttpPut]
        [Route("update-user-details")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserRequest)
        {
            try
            {
                var result = await _authenticationService.UpdateUser(updateUserRequest);

                if (result.Success == false)
                {
                    return BadRequest(new ResponseDto
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = ApiConstant.Error,
                        Message = result.Message,
                    });
                }
                else if (result.Success == true && result.Message == "User-updated-successfully")
                {


                    var userDetails = await _authenticationService.GetUserDetailById(updateUserRequest.UserId);

                    return Ok(new ResponseDto
                    {
                        Code = StatusCodes.Status200OK,
                        Status = ApiConstant.Success,
                        Message = ApiConstant.MsgUserUpdateSuccess,
                        Data = userDetails
                    });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Status = ApiConstant.Error,
                        Message = ApiConstant.MsgInternalServerError
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Status = ApiConstant.Error,
                    Message = ApiConstant.MsgInternalServerError
                });
            }
        }
    }
}
