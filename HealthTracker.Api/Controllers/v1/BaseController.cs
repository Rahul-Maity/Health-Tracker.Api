using HealthTracker.DataService.IConfiguration;
using HealthTracker.Entities.Dtos.Errors;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.Api.Controllers.v1;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class BaseController : ControllerBase
{
    public IUnitOfWork _unitOfWork;

    public UserManager<IdentityUser> _userManager;
    public BaseController(IUnitOfWork unitOfWork, UserManager<IdentityUser>userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    internal Error PopulateError(int code,string message,string type)
    {
        return new Error()
        {
            Code = code,
            Message = message,
            Type = type
        };
    }

}
