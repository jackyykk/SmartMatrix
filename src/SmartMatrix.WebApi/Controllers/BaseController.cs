using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.WebApi.Controllers
{
    // Endpoint Instructions for Web API functions
    //
    // Controller should be properly categorized
    //    
    // Function Name should be in lower case
    //
    // {action}-{conditions}[optional]-{entity}[optional]
    //
    // {action}: e.g. insert, update, delete, get, getfirst, getlist, login, logout, etc.
    // {conditions}: e.g. by_id, by_name, by_email, by_xxx, etc.
    // {entity}: e.g. main_config
    //
    // Use - to separte the entity into category and sub-category
    // Use _ to connect the words to the same entity

    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        protected readonly IConfiguration _configuration;        
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;        

        protected BaseController(ILogger<T> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;            
            _mediator = mediator;
            _mapper = mapper;            
        }

        protected string GetErrorMessage(Exception ex)
        {
            string message = ex.Message + (ex.InnerException != null ? " || " + ex.InnerException.Message : "");
            return message;
        }                                                
    }
}