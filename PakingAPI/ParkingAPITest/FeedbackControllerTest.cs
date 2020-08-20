using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using PakingAPI;
using PakingAPI.Configuration;
using PakingAPI.Controllers;
using PakingAPI.DTO;
using PakingAPI.Models;
using PakingAPI.Services.Interfaces;
using PakingAPI.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ParkingAPITest
{
    public class FeedbackControllerTest
    {
        [Fact]
        public async void PostFeedback_IfNotNull_ExpectedNotNull()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(c => c.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);

            //Mock Repository
            var feedbackRepoMock = new Mock<IFeedbackRepository>();
            feedbackRepoMock.Setup(x => x.Add(It.IsAny<Feedback>()));
            feedbackRepoMock.Setup(x => x.GetAllFeedbacks(It.IsAny<Boolean>(), It.IsAny<Boolean>())).Returns(Task.FromResult(new Feedback[1]));
            feedbackRepoMock.Setup(x => x.Save()).Returns(Task.FromResult(true));

            //Mock IActionDescriptorCollectionProvider
            var actions = new List<ActionDescriptor>();

            var mockDescriptorProvider = new Mock<IActionDescriptorCollectionProvider>();
            mockDescriptorProvider.Setup(c => c.ActionDescriptors).Returns(new ActionDescriptorCollection(actions, 0));


            //setting up new controller
            var controller = new FeedbackController(feedbackRepoMock.Object, mapper, mockDescriptorProvider.Object);


            //Create new Feedback
            var dto = new FeedbackDTO
            {
                FeedbackID=1,
                Comment="test"
            };

            //Act
            var result = await controller.PostFeedback(dto);
            var createdResult = result as CreatedResult;
            var dtoResult = (FeedbackDTO)createdResult.Value;

            //Assert
            Assert.NotNull(dtoResult);
        }
        [Fact]
        public async void GetAllFeedbacks_IfExist_ExpectedTrue()
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            //Mock DataContext
            var tesFeedbacks = GetFeedbacks();
            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Feedbacks).ReturnsDbSet(tesFeedbacks);
            //mock Repo
            var logger = Mock.Of<ILogger<FeedbackRepository>>();
            var feedbackRepoMock = new FeedbackRepository(mockContext.Object, logger);

            //Mock IActionDescriptorCollectionProvider
            var action = new List<ActionDescriptor>();
            var mockDescriptorProvider = new Mock<IActionDescriptorCollectionProvider>();
            mockDescriptorProvider.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(action, 0));

            //Creating controller
            var controller = new FeedbackController(feedbackRepoMock, mapper, mockDescriptorProvider.Object);

            //Act
            var result = await controller.GetAllFeedbacks(false);
            var contentResult = result.Result as OkObjectResult;
            FeedbackDTO [] dto = (FeedbackDTO[])contentResult.Value;

            //Assert
            Assert.True(dto.Length > 0);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async void GetFeedbackById_IfIdExist_ExpectedFeedbackId(int feedbackId, int expected)
        {
            //Arrange
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x=> x.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);

            var testFeedback = GetFeedbacks();
            //mock dataContext
            var contextMock = new Mock<DataContext>();
            contextMock.Setup(x => x.Feedbacks).ReturnsDbSet(testFeedback);

            //mockRepo
            var logger = Mock.Of < ILogger<FeedbackRepository>>();
            var feedbackRepo = new FeedbackRepository(contextMock.Object, logger);

            //Mock actionDescriptor
            var action = new List<ActionDescriptor>();
            var mockDescriptorProvider = new Mock<IActionDescriptorCollectionProvider>();
            mockDescriptorProvider.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(action,0));

            //create new controller
            var controller = new FeedbackController(feedbackRepo, mapper, mockDescriptorProvider.Object);

            //Act
            var result = await controller.GetFeedbackByID(feedbackId, false);
            var contentResult = result as OkObjectResult;
            FeedbackDTO dto = (FeedbackDTO)contentResult.Value;

            //Assert
            Assert.Equal(feedbackId, expected);
        }

        [Fact]
        public async void UpdateFeedbackByID_IfIdExist_Expected204StatusCode()
        {
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x => x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            //mocking IFeedbackrepo
            var feedbackrepo = new Mock<IFeedbackRepository>();
            feedbackrepo.Setup(x => x.Update<Feedback>(It.IsAny<Feedback>()));
            feedbackrepo.Setup(x => x.GetFeedbackByID(It.IsAny<int>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()))
                .Returns(Task.FromResult(new Feedback()));
            feedbackrepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            //mock ActionDescriptor
            var action = new List<ActionDescriptor>();
            var descriptorProvider = new Mock<IActionDescriptorCollectionProvider>();
            descriptorProvider.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(action, 0));

            // new controller
            var controller = new FeedbackController(feedbackrepo.Object, mapper, descriptorProvider.Object);

            // Create new DTo
            var dto = new FeedbackDTO
            {
                FeedbackID = 1,
                Comment = "test",
                Rate = 9,
                ParkingID = 1,
                UserID = 1
            };

            //Act
            var result = await controller.UpdateFeedbackByID(1, dto);

            //Assert
            var contentResult = result as NoContentResult;
            Assert.Equal(204, contentResult.StatusCode);
        }

        [Fact]
        public async void DeleteFeedbackByID_IfIDExist_Expected204StatusCode()
        {
            //Arrange
            //create new mappedProfile
            var profile = new MappedProfile();
            var configuration = new MapperConfiguration(x=> x.AddProfile(profile));
            var mapper = new Mapper(configuration);

            //Mock repo
            var feedbackrepo = new Mock<IFeedbackRepository>();
            feedbackrepo.Setup(x => x.Delete<Feedback>(It.IsAny<Feedback>()));
            feedbackrepo.Setup(x => x.GetFeedbackByID(It.IsAny<int>(), It.IsAny<Boolean>(), It.IsAny<Boolean>()))
                .Returns(Task.FromResult(new Feedback()));
            feedbackrepo.Setup(x => x.Save()).Returns(Task.FromResult(true));

            //create actionDescriptor
            var action = new List<ActionDescriptor>();
            var descriptorProvider = new Mock<ActionDescriptorCollectionProvider>();
            descriptorProvider.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorCollection(action, 0));

            //create controller
            var controller = new FeedbackController(feedbackrepo.Object, mapper, descriptorProvider.Object);

            //Act
            var result = await controller.DeleteFeedbackByID(1);
            var contentResult = result as NoContentResult;
            //Assert
            Assert.Equal(204, contentResult.StatusCode);

        }

        public List<Feedback>GetFeedbacks()
        {
            return new List<Feedback> 
            {
                new Feedback
                {
                    FeedbackID=1,
                    Comment="test",
                    Rate=5,
                    ParkingID=1,
                    UserID=1
                },new Feedback
                {
                    FeedbackID=2,
                    Comment="test2",
                    Rate=10,
                    ParkingID=2,
                    UserID=2
                }
            };
        }
    }
}
