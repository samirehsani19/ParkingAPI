using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PakingAPI.Model;
using PakingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PakingAPI
{
    public class DataContext:DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Parking> Parkings { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public DataContext() { }
       
        protected override void OnConfiguring (DbContextOptionsBuilder optionBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = config.GetConnectionString("DefaultConnection");
            optionBuilder.UseSqlServer(connection);
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Feedback>().HasOne(x => x.User).WithMany(x => x.Feedbacks).IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            model.Entity<Feedback>().HasOne(x => x.Parking).WithMany(x => x.feedbacks);
            model.Entity<User>().HasData(new User
            {
                UserID=1,
                FirstName="Samir",
                LastName="Jan",
                Age=24,
                Email="samir20jan@gmail.com",
            }, new User
            {
                UserID = 2,
                FirstName = "Johan",
                LastName = "Berg",
                Age = 30,
                Email = "johan@gmail.com",

            }, new User
            {
                UserID = 3,
                FirstName = "David",
                LastName = "Jahnson",
                Age = 20,
                Email = "david.jahn@gmail.com",

            });

            model.Entity<Parking>().HasData(new Parking 
            {
                ParkingID=1,
                Country="Sweden",
                City="Gathenburg",
                StreetAdress="Danska vägen",
                FreeParkingStart="free 24 h",
                FreeParkingEnd="Ends at 15 march",
                UserID=1,
            }, new Parking
            {
                ParkingID = 2,
                Country = "Sweden",
                City = "Gathenburg",
                StreetAdress = "Exportgatan",
                FreeParkingStart = "free from 18:00",
                FreeParkingEnd = "Ends at 08:00",
                UserID=3,
            }, new Parking
            {
                ParkingID = 3,
                Country = "Sweden",
                City = "Uppsala",
                StreetAdress = "Gränby vägen",
                FreeParkingStart = "free from 18:00",
                FreeParkingEnd = "Ends at 08:00",
                UserID=1,
            });

            model.Entity<Feedback>().HasData(new Feedback 
            {
                FeedbackID=1,
                Rate=8,
                Comment="Greate parking",
                ParkingID=1,
                UserID=2
            }, new Feedback
            {
                FeedbackID = 2,
                Rate = 1,
                Comment = "There is no parking at this adress",
                ParkingID = 2,
                UserID = 3
            });
        }
    }
}
