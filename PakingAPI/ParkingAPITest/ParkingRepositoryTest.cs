using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using PakingAPI;
using PakingAPI.Model;
using PakingAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ParkingAPITest
{
    public class ParkingRepositoryTest
    {
        [Fact]
        public async void GetAllParkings_IfParkingExist_ExpectedTrue()
        {
            //Arrange
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Parkings).ReturnsDbSet(GetParking());

            var logger = Mock.Of<ILogger<ParkingRepository>>();
            var parkingRepo = new ParkingRepository(mockContext.Object, logger);

            //Fact
            var result = await parkingRepo.GetAllParking(false, false);

            //Assert
            Assert.True(result.Length>0);
        }

        [Theory]
        [InlineData(1, "Stockholm")]
        public async void GetParkingByID_IfParkingByIDFound_ExpectedCity(int parkingId, string expectedCity)
        {
            //Arrange 
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Parkings).ReturnsDbSet(GetParking());

            var logger = Mock.Of<ILogger<ParkingRepository>>();
            var parkingRepo = new ParkingRepository(mockContext.Object, logger);
            //Act
            var result = await parkingRepo.GetParkingById(parkingId);
            //Assert
            Assert.Equal(expectedCity, result.City);
        }

        [Theory]
        [InlineData("Drottninggatan", "Drottninggatan")]
        public async void GetParkingByStreetName_IfStreetNameExist_ExpectedStreetName(string streetName, string expectedName)
        {
            //Arrange
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Parkings).ReturnsDbSet(GetParking());

            var logger = Mock.Of<ILogger<ParkingRepository>>();
            var parkingRepo = new ParkingRepository(mockContext.Object, logger);

            //Act
            var result = await parkingRepo.GetParkingByStreet(streetName, false, false);

            //Assert
            Assert.Equal(expectedName, result.StreetAdress);
        }
        public List<Parking>GetParking()
        {
            return new List<Parking>
            {
                new Parking
                {
                    ParkingID=1,
                    Country="Sweden",
                    City="Stockholm",
                    StreetAdress="Drottninggatan",
                    FreeParkingStart="18:00",
                    FreeParkingEnd="07:00",
                    UserID=1
                }
            };
        }
    }
}
