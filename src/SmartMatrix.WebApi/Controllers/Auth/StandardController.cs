using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/standard")]
    public class StandardController : BaseController<StandardController>
    {
        const string LOGIN_PROVIDER_NAME = SysLogin.LoginProviderOptions.Standard;

        public StandardController(ILogger<StandardController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        /// <summary>
        /// Perform login including generating token
        /// </summary>
        /// <param name="request"></param>
        /// <returns>SysUser_PerformLogin_Response</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(SysUser_PerformLogin_Request request)
        {
            // Output variables
            SysUser_OutputPayload outputUser;
            SysToken_OutputPayload outputToken;

            if (request == null)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Invalid_Request, SysUser_PerformLogin_Response.StatusTexts.Invalid_Request));
            }

            if (string.IsNullOrEmpty(request!.PartitionKey)
                || string.IsNullOrEmpty(request!.LoginName)
                || string.IsNullOrEmpty(request!.Password))
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Invalid_Request, SysUser_PerformLogin_Response.StatusTexts.Invalid_Request));
            }

            try
            {                
                // The whole process can't be done in feature/command because it involves generating token

                // Step 1: Perform login
                var result = await _mediator.Send(new SysUser_PerformLogin_Command
                {
                    Request = request
                });

                if (result.Succeeded && result.Data != null && result.Data.User != null)
                {
                    var existingUser = result.Data.User;

                    var existingLogin = existingUser.Logins.FirstOrDefault(x => x.LoginName == request.LoginName);
                    // Suppose user should have the login
                    if (existingLogin == null)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Login_NotFound, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                    }                 

                    // Step 2: Generate token
                    var user = _mapper.Map<SysUser>(existingUser);
                    var token = Auth_Generate_SysToken(LOGIN_PROVIDER_NAME, request.LoginName, user);

                    if (token == null)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Token_Generation_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                    }

                    existingLogin.RefreshToken = token.RefreshToken;
                    existingLogin.RefreshTokenExpires = token.RefreshToken_Expires;

                    // Step 3: Update refresh token
                    var updateRefreshTokenResult = await _mediator.Send(new SysLogin_UpdateRefreshToken_Command
                    {
                        Request = new SysLogin_UpdateRefreshToken_Request
                        {
                            LoginId = existingLogin.Id,
                            RefreshToken = token.RefreshToken,
                            RefreshTokenExpires = token.RefreshToken_Expires
                        }
                    });

                    if (!updateRefreshTokenResult.Succeeded)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.RefreshToken_Update_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                    }

                    // Output the user and token
                    outputUser = existingUser;
                    outputToken = _mapper.Map<SysToken_OutputPayload>(token);

                    var response = new SysUser_PerformLogin_Response
                    {
                        User = outputUser,
                        Token = outputToken
                    };

                    return Ok(Result<SysUser_PerformLogin_Response>.Success(response));
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex)));
            }            
        }


        /// <summary>
        /// Renew token including generating new access token and refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns>SysLogin_RenewToken_Response</returns>
        [HttpPost("renew-token")]
        public async Task<IActionResult> RenewToken(SysLogin_RenewToken_Request request)
        {
            SysLogin_RenewToken_Response response = new SysLogin_RenewToken_Response();

            if (request == null)
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Invalid_Request, SysLogin_RenewToken_Response.StatusTexts.Invalid_Request));
            }

            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Invalid_Request, SysLogin_RenewToken_Response.StatusTexts.Invalid_Request));
            }

            try
            {
                // The whole process can't be done in feature/command because it involves generating token

                // Step 1: Get login by refresh token
                var getLoginResult = await _mediator.Send(new SysLogin_GetFirstByRefreshToken_Query
                {
                    Request = new SysLogin_GetFirstByRefreshToken_Request
                    {
                        PartitionKey = request.PartitionKey,
                        RefreshToken = request.RefreshToken
                    }
                });

                if (!getLoginResult.Succeeded || getLoginResult.Data == null || getLoginResult.Data.Login == null)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_NotFound, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                
                var existingLogin = getLoginResult.Data.Login;

                if (string.IsNullOrEmpty(existingLogin.LoginName))
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.LoginName_Empty, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingLogin.RefreshTokenExpires < DateTime.UtcNow)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.RefreshToken_Expired, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingLogin.Status == SysLogin.StatusOptions.Disabled)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_Disabled, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingLogin.Status == SysLogin.StatusOptions.Deleted || existingLogin.IsDeleted)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_Deleted, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                } 

                // Step 2: Get user by login
                var getUserResult = await _mediator.Send(new SysUser_GetById_Query
                {
                    Request = new SysUser_GetById_Request
                    {
                        PartitionKey = request.PartitionKey,
                        Id = existingLogin.SysUserId
                    }
                });

                if (!getUserResult.Succeeded || getUserResult.Data == null || getUserResult.Data.User == null)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_NotFound, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }

                var existingUser = getUserResult.Data.User;
                
                if (existingUser.Status == SysLogin.StatusOptions.Disabled)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_Disabled, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingUser.Status == SysLogin.StatusOptions.Deleted || existingUser.IsDeleted)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_Deleted, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }

                // Step 3: Generate token
                var user = _mapper.Map<SysUser>(existingUser);
                var token = Auth_Generate_SysToken(LOGIN_PROVIDER_NAME, existingLogin.LoginName, user);
                
                // Step 4: Update refresh token
                var updateRefreshTokenResult = await _mediator.Send(new SysLogin_UpdateRefreshToken_Command
                {
                    Request = new SysLogin_UpdateRefreshToken_Request
                    {
                        LoginId = existingLogin.Id,
                        RefreshToken = token.RefreshToken,
                        RefreshTokenExpires = token.RefreshToken_Expires
                    }
                });

                if (!updateRefreshTokenResult.Succeeded)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.RefreshToken_Update_Failed, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }

                var outputToken = _mapper.Map<SysToken_OutputPayload>(token);

                response.Token = outputToken;

                return Ok(Result<SysLogin_RenewToken_Response>.Success(response));
            }
            catch (Exception ex)
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex)));
            }            
        }            
    }
}