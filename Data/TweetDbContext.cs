using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tweet.Models;

namespace tweet.Data
{
    public class TweetDbContext : DbContext
    {
        public TweetDbContext(DbContextOptions<TweetDbContext> options)
          : base(options)
        {
        }
        public DbSet<Tweet> Tweets { get; set; } = null!;
    }

}