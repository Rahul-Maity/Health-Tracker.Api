using AutoMapper;

using HealthTracker.Configuration.Messages;
using HealthTracker.DataService.IConfiguration;
using HealthTracker.Entities.Dtos.Generic;
using HealthTracker.Entities.Dtos.Incoming;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.Api.Controllers.v1;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : BaseController


{


    public UsersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager
        ,IMapper mapper) :base(unitOfWork, userManager, mapper)
    {


    }

    //Get All

    [HttpGet]
    [HttpHead]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _unitOfWork.Users.All();

        var result = new PagedResult<User>();
        result.Content = users.ToList();
        result.ResultCount = users.Count();


        return Ok(result);
    }


    //Post
    [HttpPost]
    public async Task<IActionResult> AddUser(UserDto user)
    {
        var _user = new User();
        _user.LastName = user.LastName;
        _user.FirstName = user.FirstName;
        _user.Email = user.Email;

        _user.DateOfBirth = DateTime.Parse(user.DateOfBirth).ToUniversalTime();
        _user.Phone = user.Phone;
        _user.Country = user.Country;
        _user.Status = 1;

        await _unitOfWork.Users.Add(_user);
        await _unitOfWork.CompleteAsync();


        return CreatedAtRoute("GetUser", new { id = _user.Id }, user);

    }



    //Get based on id 
    [HttpGet]
    [Route("GetUser", Name = "GetUser")]
    public async Task<IActionResult> GetUser(Guid id)
    {


        var user = await _unitOfWork.Users.GetById(id);

        
        var result = new Result<User>();

        if(user != null)
        {
            result.Content = user;
            return Ok(result);
        }

        result.Error = PopulateError(404, ErrorMessages.Users.UserNotFound, ErrorMessages.Generic.ObjectNotFound);



        return BadRequest(result);
    }
}
