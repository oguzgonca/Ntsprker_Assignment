using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Ntsprkr_Assignment.Data.Entities;


namespace Ntsprkr_Assignment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<TargetApp> TargetApps { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
