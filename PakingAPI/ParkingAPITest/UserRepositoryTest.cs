using Castle.Core.Logging;
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
    public class UserRepositoryTest
    {
        [Fact]
        public async void GetAllUser_IfUserExist_ExpectedTrue()
        {
            //Arrange
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(GetUsers());

            var logger = Mock.Of<ILogger<UserRepository>>();
            var userRepo = new UserRepository(mockContext.Object, logger);

            //Act
            var result = await userRepo.GetAllUser(false, false);

            //Assert
            Assert.True(result.Length > 0);
        }

        [Theory]
        [InlineData(1, "Adam")]
        [InlineData(2, "Lisa")]
        public async void GetUserByID_IfIDExist_ExpectedUserName(int userId, string expectedName)
        {
            //Arrange
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(GetUsers());

            var logger = Mock.Of<ILogger<UserRepository>>();
            var userRepo = new UserRepository(mockContext.Object, logger);

            //Act
            var result = await userRepo.GetUserByID(userId);

            //Asser
            Assert.Equal(expectedName, result.FirstName);
        }

        [Theory]
        [InlineData("Adam", "Berg")]
        [InlineData("Lisa", "Jahnson")]
        public async void GetUserByName_IfNameExist_ExpectedLastName(string firstName, string expectedLastName)
        {
            //Arrage
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Users).ReturnsDbSet(GetUsers());

            var logger = Mock.Of<ILogger<UserRepository>>();
            var userRepo = new UserRepository(mockContext.Object, logger);
            //Act
            var result = await userRepo.GetUserByName(firstName, false, false);
            //Assert
            Assert.Equal(expectedLastName, result.LastName);
        }
        public List<User>GetUsers()
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
