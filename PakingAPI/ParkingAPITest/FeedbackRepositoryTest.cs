using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using PakingAPI;
using PakingAPI.Models;
using PakingAPI.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ParkingAPITest
{
    public class FeedbackRepositoryTest
    {
        [Fact]
        public async void GetAllFeedbacks_IfAnyExist_ExpectedTrue()
        {
            //Arrange
            //mocking dataContext
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Feedbacks).ReturnsDbSet(GetFeedback());
            //mocking ILogger
            var logger = Mock.Of<ILogger<FeedbackRepository>>();
            var feedbackRepo = new FeedbackRepository(mockContext.Object, logger);
            //Act
            var result = await feedbackRepo.GetAllFeedbacks(false, false);
            //Assert
            Assert.True(result.Length> 0);
        }

        [Theory]
        [InlineData(1,"test 1")]
        [InlineData(2, "test 2")]
        public async void GetFeedbackByID_IFIdFound_ExpectedID(int feedbackId, string expectedComment )
        {
            //Arrange
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(x => x.Feedbacks).ReturnsDbSet(GetFeedback());

            var logger = Mock.Of<ILogger<FeedbackRepository>>();
            var feedbackRepo = new FeedbackRepository(mockContext.Object, logger);
            
            //Act
            var result = await feedbackRepo.GetFeedbackByID(feedbackId);

            //Assert
            Assert.Equal(expectedComment, result.Comment);
        }

        public List<Feedback> GetFeedback()
        {
            return new List<Feedback>
            {
                new Feedback
                {
                    FeedbackID=1,
                    Comment="test 1",
                    ParkingID=1,
                    Rate=9,
                    UserID=1
                },
                new Feedback
                {
                    FeedbackID=2,
                    Comment="test 2",
                    ParkingID=2,
                    Rate=10,
                    UserID=2
                }
            };
        }
    }
}
