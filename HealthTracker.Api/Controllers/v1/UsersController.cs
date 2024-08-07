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

        var mappedUser = _mapper.Map<User>(user); 

       

        await _unitOfWork.Users.Add(mappedUser);
        await _unitOfWork.CompleteAsync();

        var result = new Result<UserDto>();
        result.Content = user;

        return CreatedAtRoute("GetUser", new { id = mappedUser.Id }, result);

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
