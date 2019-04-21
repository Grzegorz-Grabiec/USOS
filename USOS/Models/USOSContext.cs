using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace USOS.Models
{
    public class USOSContext : IdentityDbContext<AppUser>
    {
        public USOSContext(DbContextOptions<USOSContext> options) : base(options)
        {
        }
    }
}
