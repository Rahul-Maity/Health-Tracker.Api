using HealthTracker.DataService.IConfiguration;
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


  

   
    public UsersController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager) :base(unitOfWork, userManager)
    {

       
    }

    //Get All

    [HttpGet]
    [HttpHead]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _unitOfWork.Users.All();
        return Ok(users);
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


        return Ok(user);
    }
}
