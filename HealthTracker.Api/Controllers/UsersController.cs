using HealthTracker.DataService.IConfiguration;
using HealthTracker.Entities.Dtos.Incoming;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase

    
{
    // private readonly AppDbContext _context;

    private IUnitOfWork _unitOfWork;
    public UsersController(IUnitOfWork unitOfWork)//AppDbContext context)
    {
       // _context = context;
       _unitOfWork = unitOfWork;
    }

    //Get All

    [HttpGet]
    [HttpHead]
    public async  Task<IActionResult> GetUsers()
    {
      var users =await _unitOfWork.Users.All();
        return Ok(users);
    }


    //Post
    [HttpPost]
    public IActionResult AddUser(UserDto user)
    {
        var _user=new User();
        _user.LastName = user.LastName;
        _user.FirstName = user.FirstName;
        _user.Email = user.Email;
       
        _user.DateOfBirth = DateTime.Parse(user.DateOfBirth).ToUniversalTime();
        _user.Phone = user.Phone;
        _user.Country = user.Country;
        _user.Status = 1;
        
       // _context.Users.Add(_user);
        //_context.SaveChanges();

        return Ok();//return a 201

    }



    //Get based on id 
    [HttpGet]
    [Route("GetUser")]
    public IActionResult GetUser(Guid id)
    {
       // var user = _context.Users.FirstOrDefault(x => x.Id == id);


        return Ok();
    }
}
