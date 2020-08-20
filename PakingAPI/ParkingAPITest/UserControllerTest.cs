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
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingAPITest
{
    public class UserControllerTest
    {
        [Fact]
        public async void GetAllUser_IfUserExist_Expected200StatusCode()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            //mocking DataContext
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(GetUsers());

            //mocking ILogger
            var logger = Mock.Of<ILogger<UserRepository>>();
            var userRepo = new UserRepository(mockContext.Object, logger);

            //mocking Descriptor
            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProvider = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProvider.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new UserController(userRepo, mapper, descriptorProvider.Object);
            //Act
            var result = await controller.GetUsers();
            var contentResult = result as OkObjectResult;
            //Assert
            Assert.Equal(200, contentResult.StatusCode);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void GetUserByID_IfIdFound_ExpectedID(int userId, int expectedId)
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(GetUsers());

            var logger = Mock.Of<ILogger<UserRepository>>();
            var UserRepo = new UserRepository(mockContext.Object, logger);

            var actionDescriptor = new List<ActionDescriptor>();

            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new UserController(UserRepo, mapper, descriptorProviderMock.Object);

            //Act
            var result = await controller.GetUserByID(userId, false);
            var contentResult = result as OkObjectResult;
            UserDTO dto = (UserDTO)contentResult.Value;


            //Assert
            Assert.Equal(expectedId, dto.UserID);

        }

        [Theory]
        [InlineData("Adam")]
        [InlineData("Lisa")]
        public async void GetUserByName_IfNameExist_ExpectedTrue(string name)
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x=> x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(GetUsers());

            var logger = Mock.Of<ILogger<UserRepository>>();
            var userRepo = new UserRepository(mockContext.Object, logger);

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<IActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new UserController(userRepo, mapper,descriptorProviderMock.Object);

            //Act
            var result = await controller.GetUserByName(name,false);
            var contentResult = result as OkObjectResult;
            UserDTO dto = contentResult.Value as UserDTO;

            //Assert
            Assert.True(dto.FirstName !="");
        }

        [Fact]
        public async void PostUser_IfUserNotNull_Expected201StatusCode()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var userRepo = new Mock<IUserRepo>();
            userRepo.Setup(x => x.GetAllUser(false, false)).Returns(Task.FromResult(new User[1]));
            userRepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProvider = new Mock<IActionDescriptorCollectionProvider>();
            descriptorProvider.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new UserController(userRepo.Object, mapper, descriptorProvider.Object);
            var dto = new UserDTO
            {
                UserID = 1,
                FirstName = "Hanna",
                LastName = "Nilsson",
                Age = 20,
                Email = "hanna@gmail.com",
            };

            //Act
            var createdResult = await controller.PostUser(dto);
            var contentResult = createdResult as CreatedResult;

            //Assert
            Assert.Equal(201, contentResult.StatusCode);
        }

        [Fact]
        public async void UpdateUser_IfUserByIDExist_Expected204StatusCode()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x=> x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var userRepo = new Mock<IUserRepo>();
            userRepo.Setup(x => x.Update<User>(It.IsAny<User>()));
            userRepo.Setup(x => x.GetUserByID(It.IsAny<int>(), false, false)).Returns(Task.FromResult(new User()));
            userRepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptorProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new UserController(userRepo.Object, mapper, descriptorProviderMock.Object);

            var dto = new UserDTO
            {
                UserID = 1,
                FirstName = "Hanna",
                LastName = "Nilsson",
                Age = 20,
                Email = "hanna@gmail.com",
            };

            //Act
            var result = await controller.UpdateUserByID(1, dto);
            var contentResult = result as NoContentResult;

            //Asser
            Assert.Equal(204, contentResult.StatusCode);
        }

        [Fact]
        public async void DeleteUser_IfUserByIDFound_Expected204StatusCode()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var UserRepo = new Mock<IUserRepo>();
            UserRepo.Setup(x => x.Delete<User>(It.IsAny<User>()));
            UserRepo.Setup(x => x.GetUserByID(It.IsAny<int>(), false, false)).Returns(Task.FromResult(new User()));
            UserRepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            var actionDescriptor = new List<ActionDescriptor>();
            var descriptoProviderMock = new Mock<ActionDescriptorCollectionProvider>();
            descriptoProviderMock.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(actionDescriptor, 0));

            var controller = new UserController(UserRepo.Object, mapper,descriptoProviderMock.Object);
           
            //Act
            var result = await controller.DeleteUserByID(1);
            var contentResult = result as NoContentResult;

            //Assert
            Assert.Equal(204, contentResult.StatusCode);
        }
        public List<User> GetUsers()
        {
            return new List<User>
            {
                new User
                {
                    UserID=1,
                    FirstName="Adam",
                    LastName="Berg",
                    Age=20,
                    Email="Adam2019@gmail.com",
                },
                new User
                {
                    UserID=2,
                    FirstName="Lisa",
                    LastName="Jahnson",
                    Age=24,
                    Email="Lisa2020@gmail.com",
                }
            };
        }
    }
}
