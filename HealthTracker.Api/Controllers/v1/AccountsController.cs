using System;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

using HealthTracker.Authentication.Configuration;
using HealthTracker.Authentication.Models.Dtos.Generic;
using HealthTracker.Authentication.Models.Dtos.Incoming;
using HealthTracker.Authentication.Models.Dtos.Outgoing;
using HealthTracker.DataService.IConfiguration;
using HealthTracker.Entities.DbSet;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HealthTracker.Api.Controllers.v1;

public class AccountsController:BaseController
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtConfig _jwtConfig;

    private readonly TokenValidationParameters _tokenValidationParameters;

   
    public AccountsController(IUnitOfWork unitOfWork,
       UserManager<IdentityUser> userManager,

       TokenValidationParameters tokenValidationParameters,
       
       IOptionsMonitor<JwtConfig>optionsMonitor
       
        ):base(unitOfWork)
    {
        _userManager = userManager;
        _jwtConfig = optionsMonitor.CurrentValue;

        _tokenValidationParameters = tokenValidationParameters;
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


            //Adding user to the database

            var _user = new User();
            _user.IdentityId = new Guid(newUser.Id);
            _user.LastName = registrationDto.LastName;
            _user.FirstName = registrationDto.FirstName;
            _user.Email = registrationDto.Email;

            _user.DateOfBirth = DateTime.UtcNow; //DateTime.Parse(user.DateOfBirth).ToUniversalTime();
            _user.Phone = "";
            _user.Country = "";
            _user.Status = 1;

            await _unitOfWork.Users.Add(_user);
            await _unitOfWork.CompleteAsync();




            var token =await GenerateJwtToken(newUser);
            return Ok(new UserRegistrationResponseDto()
            {
                Success = true,
                Token = token.JwtToken,
                RefreshToken = token.RefreshToken
            });


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

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto logindto)
    {
        if(ModelState.IsValid)
        {
            var userExists = await _userManager.FindByEmailAsync(logindto.Email);
            if(userExists is null)
            {
                return BadRequest(new UserLoginResponseDto()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid authentication request" }
                });
            }

            //if user exists, check password correct or not
            var isCorrect = await _userManager.CheckPasswordAsync(userExists, logindto.Password);

            if (isCorrect)
            {
                //generate Jwt token
                var jwtToken =await GenerateJwtToken(userExists);
                return Ok(new UserLoginResponseDto()
                {
                    Success = true,
                    Token = jwtToken.JwtToken,
                    RefreshToken = jwtToken.RefreshToken
                });

            }
            else
            {
                return BadRequest(new UserLoginResponseDto()
                {
                    Success = false,
                    Errors = new List<string>() { "Invalid authentication request,wrong password" }
                });
            }
        }
        else
        {
            return BadRequest(new UserLoginResponseDto()
            {
                Success = false,
                Errors = new List<string>() { "Invalid Payload" }
            });
        }
    }

    
    #region

    private async Task<TokenData> GenerateJwtToken(IdentityUser user)
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
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),//unique id
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) //used by the refresh token

            }),
            Expires = DateTime.UtcNow.Add(_jwtConfig.ExpiryTimeFrame),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
                

        };

        var token = jwtHandler.CreateToken(tokenDescriptor);

        //convert the security object to an usable string token
        var JwtToken = jwtHandler.WriteToken(token);

        //Generate a new refresh token
        var refreshToken = new RefreshToken
        {
            AddedDate = DateTime.UtcNow,
            Token = $"{RandomStringGenerator(25)}_{Guid.NewGuid()}",
            UserId = user.Id,
            IsRevoked = false,
            IsUsed = false,
            Status = 1,
            JwtId = token.Id,
            ExpiryDate = DateTime.UtcNow.AddMonths(6),
        };

        //saving refresh token in db
        await _unitOfWork.RefreshTokens.Add(refreshToken);
        await _unitOfWork.CompleteAsync();

        var tokenData = new TokenData
        {
            JwtToken = JwtToken,
            RefreshToken = refreshToken.Token
        };

        
        return tokenData;

    }

    //method for random string , refresh token 
    private string RandomStringGenerator(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString();
    }


    #endregion

}
