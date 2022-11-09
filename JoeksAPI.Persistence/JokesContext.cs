using JokesAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JokesAPI.Persistence
{
    public class JokesContext : DbContext
    {
        public JokesContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Joke> Jokes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Joke>().HasData(
                new Joke
                {
                    JokeId = 1,
                    Question = "Why did the chicken cross the road",
                    Answer = "To get to the other side"
                }
                );
        }
    }
}
