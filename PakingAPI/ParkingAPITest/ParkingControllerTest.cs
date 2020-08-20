using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using PakingAPI;
using PakingAPI.Configuration;
using PakingAPI.Controllers;
using PakingAPI.DTO;
using PakingAPI.Model;
using PakingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingAPITest
{
    public class ParkingControllerTest
    {
        [Fact]
        public async void GetParkings_IfExist_ExpectedCity()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Parkings).ReturnsDbSet(GetParking());

            var logger = Mock.Of<ILogger<ParkingRepository>>();
            var parkingRepo = new ParkingRepository(mockContext.Object, logger);

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new ParkingController(parkingRepo, mapper, descriptorProviderMock.Object);
            //Act
            var result = await controller.GetParkings(false, false, false);
            var contentResut = result as OkObjectResult;
            var dto = contentResut.Value as ParkingDTO[];
            string city = "Stockholm";

            //Assert
            Assert.Equal(city, dto[0].City);
        }

        [Fact]
        public async void GetParkingByID_IfAnyParkingFound_ExpectedId()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Parkings).ReturnsDbSet(GetParking());

            var logger = Mock.Of<ILogger<ParkingRepository>>();
            var parkingRepo = new ParkingRepository(mockContext.Object, logger);

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new ParkingController(parkingRepo, mapper, descriptorProviderMock.Object);

            //Act
            var result = await controller.GetParkingByID(1, false);

            var contentResult = result as OkObjectResult;
            var dto = contentResult.Value as ParkingDTO;

            //Assert
            Assert.Equal(1, dto.ParkingID);
        }

        [Theory]
        [InlineData("Drottninggatan", "Sweden")]
        public async void GetParkingByStreetName_IfParkingExist_ExpectedCountry(string sName, string expected)
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Parkings).ReturnsDbSet(GetParking());
            var logger = Mock.Of<ILogger<ParkingRepository>>();

            var parkingRepo = new ParkingRepository(mockContext.Object, logger);

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new ParkingController(parkingRepo, mapper, descriptorProviderMock.Object);

            //Act
            var result = await controller.GetParkingByStreetName(sName, false);
            var contentResult = result as OkObjectResult;
            var dto = contentResult.Value as ParkingDTO;

            //Assert
            Assert.Equal(expected, dto.Country);
        }

        [Fact]
        public async Task PostParking_IfPakingNotNull_Expected201StatusCode()
        {
            //arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var parkingRepo = new Mock<IParkingRepo>();
            parkingRepo.Setup(x => x.GetAllParking(It.IsAny<Boolean>(), It.IsAny<Boolean>()))
                .Returns(Task.FromResult(new Parking[1]));
            parkingRepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));            //Act

            var controller = new ParkingController(parkingRepo.Object, mapper, descriptorProviderMock.Object);

            var dto = new ParkingDTO
            {
                ParkingID = 1,
                Country = "Sweden",
                City = "Stockholm",
                StreetAdress = "Drottninggatan",
                FreeParkingStart = "18:00",
                FreeParkingEnd = "07:00",
                UserID = 1
            };

            //Act
            var result = await controller.PostParking(dto);
            var contentResult = result as CreatedResult;
            //Assert
            Assert.Equal(201, contentResult.StatusCode);
        }

        [Fact]
        public async void UpdateParkingByID_IfIdExist_Expected204StatusCode()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var parkingRepo = new Mock<IParkingRepo>();
            parkingRepo.Setup(x => x.GetParkingById(It.IsAny<int>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()))
                .Returns(Task.FromResult(new Parking()));
            parkingRepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new ParkingController(parkingRepo.Object, mapper, descriptorProviderMock.Object);
            
            int id = 1;
            var dto = new ParkingDTO
            {
                ParkingID = 1,
                Country = "Sweden",
                City = "Stockholm",
                StreetAdress = "Drottninggatan",
                FreeParkingStart = "18:00",
                FreeParkingEnd = "07:00",
                UserID = 1
            };
            
            //Act
            var result = await controller.UpdateParkingByID(id, dto);
            var contentResult = result as NoContentResult;

            //Assert
            Assert.Equal(204, contentResult.StatusCode);
        }
             
        [Fact]
        public async void DeleteParkingByID_IfIdFound_Expected204StatusCode()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var parkingRepo = new Mock<IParkingRepo>();
            parkingRepo.Setup(x => x.GetParkingById(It.IsAny<int>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()))
                .Returns(Task.FromResult(new Parking()));
            parkingRepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new ParkingController(parkingRepo.Object, mapper, descriptorProviderMock.Object);
            int id = 1;
            //Act
            var result = await controller.DeleteParkingByID(id);
            var contentResult = result as NoContentResult;

            //Assert
            Assert.Equal(204, contentResult.StatusCode);
        }
        public List<Parking> GetParking()
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
