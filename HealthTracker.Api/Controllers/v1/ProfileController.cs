using AutoMapper;

using HealthTracker.Configuration.Messages;
using HealthTracker.DataService.IConfiguration;
using HealthTracker.Entities.Dtos.Errors;
using HealthTracker.Entities.Dtos.Generic;
using HealthTracker.Entities.Dtos.Incoming.Profile;
using HealthTracker.Entities.Dtos.outgoing.Profile;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthTracker.Api.Controllers.v1;

[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
public class ProfileController:BaseController
{
    public ProfileController(IUnitOfWork unitOfWork,UserManager<IdentityUser> userManager
        ,IMapper mapper):base(unitOfWork, userManager, mapper)
    {
        
    }



    [HttpGet]
    public async Task<IActionResult>GetProfile()
    {

        var LoggedInUser = await _userManager.GetUserAsync(HttpContext.User);
        var result = new Result<ProfileDto>();
        if (LoggedInUser is null)
        {

            

            result.Error = new Error()
            {
                Code = 400,
                Message = ErrorMessages.Profile.UserNotFound,
                Type = ErrorMessages.Generic.TypeBadRequest
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
                Message = ErrorMessages.Profile.UserNotFound,
                Type = ErrorMessages.Generic.TypeBadRequest
            };



            return BadRequest(result);
        }

        var mappedProfile = _mapper.Map<ProfileDto>(profile);
       
        result.Content = mappedProfile;

        return Ok(result);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileDto profile)
    {

        var result =new Result<ProfileDto>();


        if (!ModelState.IsValid)
        {
            result.Error = new Error()
            {
                Code = 400,
                Message = ErrorMessages.Generic.InvalidPayload,
                Type = ErrorMessages.Generic.TypeBadRequest
            };



            return BadRequest(result);
         
        }

        //this action below happens because we added claims in identity framework to check logged in or not
        var LoggedInUser = await _userManager.GetUserAsync(HttpContext.User);


        if (LoggedInUser is null)
        {
            result.Error = new Error()
            {
                Code = 400,
                Message = ErrorMessages.Profile.UserNotFound,
                Type = ErrorMessages.Generic.TypeBadRequest
            };



            return BadRequest(result);

        }


        var identityId = new Guid(LoggedInUser.Id);
        var userProfile = await _unitOfWork.Users.GetByIdentityId(identityId);


        if (userProfile is null)
        {
            result.Error = new Error()
            {
                Code = 400,
                Message = ErrorMessages.Profile.UserNotFound,
                Type = ErrorMessages.Generic.TypeBadRequest
            };



            return BadRequest(result);
      
        }

        userProfile.Address = profile.Address;
        userProfile.Sex = profile.Sex;
        userProfile.Country = profile.Country;
        userProfile.MobileNumber = profile.MobileNumber;

        var isUpdated = await _unitOfWork.Users.UpdateUserProfile(userProfile);

        if(isUpdated)
        {
            await _unitOfWork.CompleteAsync();
            var mappedUser = _mapper.Map<ProfileDto>(profile);
            result.Content = mappedUser;
            return Ok(result);
        }



        result.Error = PopulateError(500, ErrorMessages.Generic.SomethingWentWrong, ErrorMessages.Generic.UnableToProcess);
   


        return BadRequest(result);
    

    }

}
