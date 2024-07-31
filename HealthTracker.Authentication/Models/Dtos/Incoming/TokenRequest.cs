using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker.Authentication.Models.Dtos.Incoming;
public class TokenRequest
{

    [Required]
    public string  JwtToken { get; set; }



    [Required]
    public string RefreshToken { get; set; }
}
