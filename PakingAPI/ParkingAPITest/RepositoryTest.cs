using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using PakingAPI;
using PakingAPI.Model;
using PakingAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingAPITest
{
    public class RepositoryTest
    {
        [Fact]
        public void Add_IfNotNull_ExpectedTrue()
        {
            //Arrange
            var mockContext = new Mock<DataContext>();
            var logger = Mock.Of<ILogger<Repository>>();

            var repo = new Repository(mockContext.Object,logger);
            repo.Add(GetUsers());

            //Act
            var save = repo.Save();

            //Assert
            Assert.True(save.Result);
        }
        public User GetUsers()
        {
            return new User
            {
                UserID = 1,
                FirstName = "Adam",
                LastName = "Berg",
                Age = 20,
                Email = "Adam2019@gmail.com",
            };
        }
    }
}
