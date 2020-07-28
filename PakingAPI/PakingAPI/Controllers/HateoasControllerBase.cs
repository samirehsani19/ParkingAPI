using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using PakingAPI.DTO;
using PakingAPI.Hateoas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Controllers
{
    public class HateoasControllerBase: ControllerBase
    {
        private readonly IReadOnlyList<ActionDescriptor> route;
        public HateoasControllerBase(IActionDescriptorCollectionProvider provider)
        {
            route = provider.ActionDescriptors.Items;
        }
        internal Link UrlLink(string relation, string routeName, object values) 
        {
            var route = this.route.FirstOrDefault(x => x.AttributeRouteInfo.Name == routeName);
            var method = route.ActionConstraints.OfType<HttpMethodActionConstraint>().FirstOrDefault().HttpMethods.FirstOrDefault();
            var url = Url.Link(routeName, values).ToLower();
            return new Link(url, relation, method);
        }
        internal FeedbackDTO HateoasMainLinks(FeedbackDTO fDto)
        {
            fDto.Links.Add(UrlLink("all", "GetAllFeedback", null));
            fDto.Links.Add(UrlLink("Self", "GetFeedbackByID", new { id = fDto.FeedbackID }));
            return fDto;
        }
        internal ParkingDTO HateoasMainLinks(ParkingDTO pdto)
        {
            pdto.Links.Add(UrlLink("All", "GetAllParking", null));
            pdto.Links.Add(UrlLink("Self", "GetParkingByID", new { id = pdto.ParkingID }));
            pdto.Links.Add(UrlLink("Self", "GetParkingByStreetName", new { name = pdto.StreetAdress }));
            return pdto;
        }

        internal UserDTO HateoasMainLinks(UserDTO dto)
        {
            dto.Links.Add(UrlLink("All", "GetAllUsers", null));
            dto.Links.Add(UrlLink("Self", "GetUserByID", new { id = dto.UserID }));
            dto.Links.Add(UrlLink("Self", "GetUserByName", new { name = dto.FirstName }));
            return dto;
        }
    }
}
