﻿using System;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class AppDbContext:IdentityDbContext
{

    public virtual DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
    {
        
    }
}
