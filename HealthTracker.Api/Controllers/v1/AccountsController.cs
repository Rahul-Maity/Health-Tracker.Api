using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using HealthTracker.Authentication.Configuration;
using HealthTracker.Authentication.Models.Dtos.Incoming;
using HealthTracker.Authentication.Models.Dtos.Outgoing;
using HealthTracker.DataService.IConfiguration;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HealthTracker.Api.Controllers.v1;

public class AccountsController:BaseController
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;
    public AccountsController(IUnitOfWork unitOfWork,
       UserManager<IdentityUser> userManager,
       IOptionsMonitor<JwtConfig>optionsMonitor
       
        ):base(unitOfWork)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;
    }


    //Register action

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationRequestsDto registrationDto)
    {
        if(ModelState.IsValid)
        {
            var userExists = await _userManager.FindByEmailAsync(registrationDto.Email);
            if(userExists != null){
                return BadRequest(new UserRegistrationResponseDto
                {
                    Success = false,
                    Errors = new List<string>() { "Email already in use" }
                });
            }

            var newUser = new IdentityUser()
            {
                UserName = registrationDto.Email,
                Email = registrationDto.Email,
                EmailConfirmed = true
            };
            var isCreated = await _userManager.CreateAsync(newUser, registrationDto.Password);

            if (!isCreated.Succeeded)
            {
                return BadRequest(new UserRegistrationResponseDto
                {
                    Success= isCreated.Succeeded,
                    Errors = isCreated.Errors.Select(x=>x.Description).ToList()

                });
            }


        }
        else
        {
            return BadRequest(new UserRegistrationResponseDto
            {
                Success = false,
                Errors = new List<string>() { "Invalid Payload" }
            });

        }
    }



    #region

    private string GenerateJwtToken(IdentityUser user)
    {
        //This handler responsible for creating the token
        var jwtHandler = new JwtSecurityTokenHandler();




        //get the security key
        var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret_Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id",user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email
                ),//unique id
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            });
        }

    }



    #endregion

}
