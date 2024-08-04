using HealthTracker.DataService.IConfiguration;
using HealthTracker.Entities.Dtos.Errors;
using HealthTracker.Entities.Dtos.Generic;
using HealthTracker.Entities.Dtos.Incoming.Profile;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.Api.Controllers.v1;




[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController:BaseController
{
    public ProfileController(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager):base(unitOfWork, userManager)
    {
        
    }



    [HttpGet]
    public async Task<IActionResult>GetProfile()
    {

        var LoggedInUser = await _userManager.GetUserAsync(HttpContext.User);
        var result = new Result<User>();
        if (LoggedInUser is null)
        {

            

            result.Error = new Error()
            {
                Code = 400,
                Message = "User not found",
                Type = "Bad Request"
            };



            return BadRequest(result);
        }
        var identityId = new Guid(LoggedInUser.Id);
        var profile = await _unitOfWork.Users.GetByIdentityId(identityId);
        if (profile is null)
        {
           

            result.Error = new Error()
            {
                Code = 400,
                Message = "User not found",
                Type = "Bad Request"
            };



            return BadRequest(result);
        }

       
        result.Content = profile;

        return Ok(result);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileDto profile)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Invalid Payload");
        }

        //this action below happens because we added claims in identity framework to check logged in or not
        var LoggedInUser = await _userManager.GetUserAsync(HttpContext.User);


        if (LoggedInUser is null)
        {
            return BadRequest("User not found");
        }


        var identityId = new Guid(LoggedInUser.Id);
        var userProfile = await _unitOfWork.Users.GetByIdentityId(identityId);
        if (userProfile is null)
        {
            return BadRequest("User not found");
        }

        userProfile.Address = profile.Address;
        userProfile.Sex = profile.Sex;
        userProfile.Country = profile.Country;
        userProfile.MobileNumber = profile.MobileNumber;

        var isUpdated = await _unitOfWork.Users.UpdateUserProfile(userProfile);

        if(isUpdated)
        {
            await _unitOfWork.CompleteAsync();
            return Ok(userProfile);
        }


        return BadRequest("something went wrong, plz try again");

    }

}
