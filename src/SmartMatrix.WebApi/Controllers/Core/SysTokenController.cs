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

namespace SmartMatrix.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/systoken")]
    //[Authorize(Policy = WebConstants.Authorizations.Policies.Standard_Api_Policy)]
    public class SysTokenController : BaseController<SysTokenController>
    {
        protected readonly ITokenService _tokenService;

        public SysTokenController(ILogger<SysTokenController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper, ITokenService tokenService)
            : base(logger, configuration, mediator, mapper)
        {
            _tokenService = tokenService;
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

            if (string.IsNullOrEmpty(request!.PartitionKey))
            {
                // Default partition key
                request.PartitionKey = SysLogin.PartitionKeyOptions.SmartMatrix;
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
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_Disabled, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingLogin.Status == SysLogin.StatusOptions.Deleted || existingLogin.IsDeleted)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_Deleted, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
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
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_Disabled, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (existingUser.Status == SysLogin.StatusOptions.Deleted || existingUser.IsDeleted)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_Deleted, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }

                // Step 3: Generate token
                var user = _mapper.Map<SysUser>(existingUser);
                var token = _tokenService.GenerateToken(existingLogin.LoginProvider, existingLogin.LoginName, user);
                
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
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.RefreshToken_Update_Failed, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
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