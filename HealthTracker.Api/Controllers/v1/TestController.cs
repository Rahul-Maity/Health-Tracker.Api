﻿using AutoMapper;

using HealthTracker.DataService.Data;

using Microsoft.AspNetCore.Identity;

namespace HealthTracker.Api.Controllers.v1;

public class TestController:BaseController
{

    public TestController(UnitOfWork unitOfWork,UserManager<IdentityUser> userManager,IMapper mapper
        ) :base(unitOfWork, userManager, mapper)
    {
        
    }
}
