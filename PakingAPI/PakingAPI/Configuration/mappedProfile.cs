using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PakingAPI.DTO;
using PakingAPI.Model;
using PakingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PakingAPI.Configuration
{
    public class mappedProfile:Profile
    {
        public mappedProfile()
        {
            CreateMap<Parking, ParkingDTO>().ForMember(x => x.feedbacks, x => x.MapFrom(z => z.feedbacks
              .Select(c => new Feedback
              {
                  FeedbackID = c.FeedbackID,
                  Rate = c.Rate,
                  Comment = c.Comment,
                  User = null,
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
        }
    }
}
