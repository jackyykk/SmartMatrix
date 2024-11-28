using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Application.Interfaces.Services.Essential;
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

        protected readonly ITokenService _tokenService;

        public StandardController(ILogger<StandardController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper, ITokenService tokenService)
            : base(logger, configuration, mediator, mapper)
        {
            _tokenService = tokenService;
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

            if (string.IsNullOrEmpty(request!.PartitionKey))
            {
                // Default partition key
                request.PartitionKey = SysLogin.PartitionKeyOptions.SmartMatrix;
            }

            if (string.IsNullOrEmpty(request!.LoginName)
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
                    var token = _tokenService.GenerateToken(LOGIN_PROVIDER_NAME, request.LoginName, user);

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

        [HttpGet("renew-token-by-ott")]
        public async Task<IActionResult> GetTokenByOneTimeToken([FromQuery] SysLogin_RenewTokenByOneTimeToken_Request request)
        {
            SysLogin_RenewTokenByOneTimeToken_Response response = new SysLogin_RenewTokenByOneTimeToken_Response();

            if (request == null)
            {
                return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.Invalid_Request, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Request));
            }

            if (string.IsNullOrEmpty(request!.PartitionKey))
            {
                // Default partition key
                request.PartitionKey = SysLogin.PartitionKeyOptions.SmartMatrix;
            }

            if (string.IsNullOrEmpty(request.OneTimeToken))
            {
                return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.Invalid_Request, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Request));
            }

            try
            {
                // The whole process can't be done in feature/command because it involves generating token

                // Step 1: Get login by refresh token
                var getLoginResult = await _mediator.Send(new SysLogin_GetFirstByOneTimeToken_Query
                {
                    Request = new SysLogin_GetFirstByOneTimeToken_Request
                    {
                        PartitionKey = request.PartitionKey,
                        OneTimeToken = request.OneTimeToken
                    }
                });

                if (!getLoginResult.Succeeded || getLoginResult.Data == null || getLoginResult.Data.Login == null)
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.Login_NotFound, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }
                
                var existingLogin = getLoginResult.Data.Login;

                if (string.IsNullOrEmpty(existingLogin.LoginName))
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.LoginName_Empty, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingLogin.OneTimeTokenExpires < DateTime.UtcNow)
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.OneTimeToken_Expired, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingLogin.Status == SysLogin.StatusOptions.Disabled)
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.Login_Disabled, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingLogin.Status == SysLogin.StatusOptions.Deleted || existingLogin.IsDeleted)
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.Login_Deleted, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
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
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.User_NotFound, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }

                var existingUser = getUserResult.Data.User;
                
                if (existingUser.Status == SysLogin.StatusOptions.Disabled)
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.User_Disabled, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingUser.Status == SysLogin.StatusOptions.Deleted || existingUser.IsDeleted)
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.User_Deleted, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }

                // Step 3: Generate token
                var user = _mapper.Map<SysUser>(existingUser);
                var token = _tokenService.GenerateToken(existingLogin.LoginProvider, existingLogin.LoginName, user);
                
                // Step 4: Update tokens, and removed one-time token
                // In debug mode, the function will run twice, and the second time will fail because the one-time token is already removed
                // So, it's better to keep the one-time token in the database
                /*
                var updateTokensResult = await _mediator.Send(new SysLogin_UpdateTokens_Command
                {
                    Request = new SysLogin_UpdateTokens_Request
                    {
                        LoginId = existingLogin.Id,
                        RefreshToken = token.RefreshToken,
                        RefreshTokenExpires = token.RefreshToken_Expires,
                        OneTimeToken = string.Empty,
                        OneTimeTokenExpires = null
                    }
                });
                */
                var updateTokensResult = await _mediator.Send(new SysLogin_UpdateRefreshToken_Command
                {
                    Request = new SysLogin_UpdateRefreshToken_Request
                    {
                        LoginId = existingLogin.Id,
                        RefreshToken = token.RefreshToken,
                        RefreshTokenExpires = token.RefreshToken_Expires                        
                    }
                });

                if (!updateTokensResult.Succeeded)
                {
                    return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.Tokens_Update_Failed, SysLogin_RenewTokenByOneTimeToken_Response.StatusTexts.Invalid_Token));
                }

                var outputToken = _mapper.Map<SysToken_OutputPayload>(token);

                response.Token = outputToken;

                return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Success(response));
            }
            catch (Exception ex)
            {
                return Ok(Result<SysLogin_RenewTokenByOneTimeToken_Response>.Fail(SysLogin_RenewTokenByOneTimeToken_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex)));
            }            
        }                      
    }
}