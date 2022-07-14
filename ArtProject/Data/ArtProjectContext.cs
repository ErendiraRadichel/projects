using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ArtProject.Models;

namespace ArtProject.Data
{
    public class ArtProjectContext : DbContext
    {
        public ArtProjectContext (DbContextOptions<ArtProjectContext> options)
            : base(options)
        {
        }

        public DbSet<ArtProject.Models.Art> Art { get; set; } = default!;
    }
}
