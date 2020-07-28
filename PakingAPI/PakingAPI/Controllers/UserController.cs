using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.FileProviders;
using PakingAPI.DTO;
using PakingAPI.Model;
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

        /// <summary>
        /// /api/v1.0/User      Get All Users
        /// </summary>
        /// <param name="includeLinks"></param>
        /// <param name="includeParking"></param>
        /// <param name="includeFeedback"></param>
        /// <returns></returns>
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

        /// <summary>
        /// /api/v1.0/User/1      Get User By ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeLinks"></param>
        /// <param name="includeParking"></param>
        /// <param name="includeFeedback"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name ="GetUserByID")]
        public async Task<IActionResult>GetUserByID(int id,[FromQuery]bool includeLinks=true, [FromQuery]bool includeParking=false,
            [FromQuery]bool includeFeedback=false)
        {
            try
            {
                var result = await repo.GetUserByID(id, includeParking, includeFeedback);
                if (result == null)
                {
                    return NotFound();
                }
                UserDTO mapped = mapper.Map<UserDTO>(result);

                if (includeLinks)
                {
                    mapped = HateoasMainLinks(mapped);
                }
                return Ok(mapped);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure: {e.Message}");
            }
        }

        /// <summary>
        /// /api/v1.0/User/samir      Get User By Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="includeLinks"></param>
        /// <param name="includeParking"></param>
        /// <param name="includeFeedback"></param>
        /// <returns></returns>
        [HttpGet("{name}", Name ="GetUserByName")]
        public async Task<IActionResult>GetUserByName(string name,[FromQuery]bool includeLinks=true, [FromQuery]bool includeParking=false,
            [FromQuery]bool includeFeedback=false)
        {
            try 
            {
                var result = await repo.GetUserByName(name, includeParking, includeFeedback);
                if (result==null)
                {
                    return NotFound();
                }
                UserDTO mapped = mapper.Map<UserDTO>(result);
                if (includeLinks)
                {
                    mapped = HateoasMainLinks(mapped);
                }
                return Ok(mapped);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");

            }
        }

        [HttpPost(Name ="PostUser")]
        public async Task<ActionResult<UserDTO>>PostUser(UserDTO user)
        {
            try
            {
                var mappedEntity = mapper.Map<User>(user);
                repo.Add(mappedEntity);
                if (await repo.Save())
                {
                    return Created($"/api/v1.0/user/{user.UserID}", mapper.Map<UserDTO>(mappedEntity));
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure: {e.Message}");
            }
        }
        
    }
}
