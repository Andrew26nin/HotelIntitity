using System;
using System.Collections.Generic;
using System.Text;
using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Booking> Booking { get; set; }

        public DbSet<Room> Room { get; set; }

        public DbSet<Client> Client { get; set; }

        public DbSet<RoomType> RoomType { get; set; }
    }
}
