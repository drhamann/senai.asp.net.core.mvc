using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Senai.Asp.Net.Core.Mvc.MvcMovie.Models;

namespace Senai.Asp.Net.Core.Mvc.MvcMovie.Data
{
    public class SenaiAspNetCoreMvcMvcMovieContext : DbContext
    {
        public SenaiAspNetCoreMvcMvcMovieContext (DbContextOptions<SenaiAspNetCoreMvcMvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Senai.Asp.Net.Core.Mvc.MvcMovie.Models.Movie> Movie { get; set; } = default!;
    }
}
