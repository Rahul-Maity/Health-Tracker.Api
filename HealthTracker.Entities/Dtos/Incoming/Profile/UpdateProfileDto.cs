using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker.Entities.Dtos.Incoming.Profile;
public class UpdateProfileDto
{

    public string Country { get; set; }

    public string Address { get; set; }
    public string MobileNumber { get; set; }

    public string Sex { get; set; }
}
