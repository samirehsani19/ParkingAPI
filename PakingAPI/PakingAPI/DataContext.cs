using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PakingAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI
{
    public class DataContext:DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Parking> Parkings { get; set; }
        public virtual DbSet<UserParking> UserParkings { get; set; }
        public DataContext() { }
       
        protected override void OnConfiguring (DbContextOptionsBuilder optionBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = config.GetConnectionString("DefaultConnection");
            optionBuilder.UseSqlServer(connection);
        }
    }
}
