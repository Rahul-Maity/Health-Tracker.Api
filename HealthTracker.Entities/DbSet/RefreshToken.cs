using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace HealthTracker.Entities.DbSet;
public class RefreshToken:BaseEntity
{
    public string UserId { get; set; }//user id logged in

    public string Token { get; set; }

    public string JwtId { get; set; } // the id generated when the jwt id has been requested
    public bool IsUsed { get; set; } //make sure the token is only used once

    public bool IsRevoked { get; set; } // make sure they are valid

    public DateTime ExpiryDate { get; set; }

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }
}
