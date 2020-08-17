using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using PakingAPI.Configuration;
using PakingAPI.Controllers;
using PakingAPI.DTO;
using PakingAPI.Models;
using PakingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ParkingAPITest
{
    public class FeedbackControllerTest
    {
        [Fact]
        public async void PostFeedback_ShouldPost_WhenIDEqualOne()
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

            //Assert
            var createdResult = result as CreatedResult;
            var dtoResult = (FeedbackDTO)createdResult.Value;
            Assert.Equal(1, dtoResult.FeedbackID);
        }
    }
}
