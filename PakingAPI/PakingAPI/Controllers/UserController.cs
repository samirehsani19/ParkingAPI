using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PakingAPI.DTO;
using PakingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class UserController:HateoasControllerBase
    {
        private readonly IUserRepo repo;
        private readonly IMapper mapper;
        public UserController(IUserRepo repo, IMapper mapper, IActionDescriptorCollectionProvider provider) : base(provider) 
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet(Name ="GetAllUsers")]
        public async Task<IActionResult>GetUsers([FromQuery]bool includeLinks=true, [FromQuery] bool includeParking = false,
            [FromQuery]bool includeFeedback=false)
        {
            try
            {
                var result = await repo.GetAllUser(includeParking, includeFeedback);
                if (result==null)
                {
                    return NotFound();
                }
                IEnumerable<UserDTO> mapped = mapper.Map<UserDTO[]>(result);
                if (includeLinks)
                {
                    mapped = mapped.Select(x => HateoasMainLinks(x));
                }
                return Ok(mapped);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }

        }

    }
}
