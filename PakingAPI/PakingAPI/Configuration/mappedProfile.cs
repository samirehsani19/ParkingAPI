using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PakingAPI.DTO;
using PakingAPI.Model;
using PakingAPI.Models;
using PakingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PakingAPI.Configuration
{
    public class MappedProfile:Profile
    {
        public MappedProfile()
        {
            CreateMap<Parking, ParkingDTO>().ForMember(x => x.feedbacks, x => x.MapFrom(z => z.feedbacks
              .Select(c => new Feedback
              {
                  FeedbackID = c.FeedbackID,
                  Rate = c.Rate,
                  Comment = c.Comment,
                  UserID=c.UserID,
                  User = null,
                  ParkingID=c.ParkingID,
                  Parking = null
              })))
                .ForPath(x => x.User, z => z.MapFrom(x => new User 
                {
                    UserID = x.UserID,
                    FirstName=x.User.FirstName,
                    LastName=x.User.LastName,
                    Age=x.User.Age,
                    Email=x.User.Email,
                    Feedbacks=null,
                    Parkings=null
                })).ReverseMap();


            CreateMap<Feedback, FeedbackDTO>().ForMember(x => x.Parking, z => z.MapFrom(c =>
              new Parking
              {
                  ParkingID=c.Parking.ParkingID,
                  Country=c.Parking.Country,
                  City=c.Parking.City,
                  StreetAdress=c.Parking.StreetAdress,
                  FreeParkingStart=c.Parking.FreeParkingStart,
                  FreeParkingEnd=c.Parking.FreeParkingEnd,
                  feedbacks=null,
                  UserID=c.Parking.UserID,
                  User=null,
              }))
                .ForMember(x=> x.User, z=> z.MapFrom(c=> 
                new User 
                {
                    UserID=c.User.UserID,
                    FirstName=c.User.FirstName,
                    LastName=c.User.LastName,
                    Age=c.User.Age,
                    Email=c.User.Email,
                    Feedbacks=null,
                    Parkings=null
                })).ReverseMap();

            CreateMap<User, UserDTO>().ForMember(x => x.Parkings, z => z.MapFrom(c => c.Parkings
              .Select(s => new Parking
              {
                  ParkingID = s.ParkingID,
                  Country = s.Country,
                  City = s.City,
                  StreetAdress = s.StreetAdress,
                  FreeParkingStart = s.FreeParkingStart,
                  FreeParkingEnd = s.FreeParkingEnd,
                  UserID = s.UserID,
                  User = null,
                  feedbacks = null,
              })))
                .ForMember(x => x.Feedbacks, z => z.MapFrom(c => c.Feedbacks
                .Select(s => new Feedback
                {
                    FeedbackID = s.FeedbackID,
                    Rate = s.Rate,
                    Comment = s.Comment,
                    ParkingID = s.ParkingID,
                    UserID = s.UserID,
                    Parking = null,
                    User = null
                }))).ReverseMap();
        }
    }
}
