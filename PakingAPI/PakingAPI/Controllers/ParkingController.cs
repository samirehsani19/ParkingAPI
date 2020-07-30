using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PakingAPI.DTO;
using Microsoft.AspNetCore.Http;
using PakingAPI.Model;
using PakingAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PakingAPI.Hateoas;
using System.Security.Policy;

namespace PakingAPI.Controllers
{
    [ApiController]
    [Route("api/v1.0/[Controller]")]
    public class ParkingController : HateoasControllerBase
    {
        private readonly IParkingRepo repo;
        private readonly IMapper mapper;
        public ParkingController(IParkingRepo repo, IMapper mapper, IActionDescriptorCollectionProvider provider)
            : base(provider)
        {
            this.repo = repo;
            this.mapper = mapper;
        }
        /// <summary>
        /// /api/v1.0/parking   To get all paarkings
        /// </summary>
        /// <param name="includeLinks"></param>
        /// <param name="includeFeedback"></param>
        /// <param name="includeUser"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetAllParking")]
        public async Task<IActionResult> GetParkings([FromQuery] bool includeLinks = true,
            [FromQuery] bool includeFeedback = false, [FromQuery] bool includeUser = false)
        {
            try
            {
                var result = await repo.GetAllParking(includeFeedback, includeUser);
                if (result == null)
                {
                    return NotFound();
                }
                IEnumerable<ParkingDTO> mapped = mapper.Map<ParkingDTO[]>(result);
                if (includeLinks)
                {
                    mapped = mapped.Select(x => HateoasMainLinks(x));
                }
                return Ok(mapped);

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }
        }
        /// <summary>
        /// /api/v1.0/parking/1      To get parking by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeLinks"></param>
        /// <param name="includeFeedback"></param>
        /// <param name="includeUser"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetParkingByID")]
        public async Task<IActionResult> GetParkingByID(int id, [FromQuery] bool includeLinks = true,
            [FromQuery] bool includeFeedback = false, [FromQuery] bool includeUser = false)
        {
            try
            {
                var result = await repo.GetParkingById(id, includeFeedback, includeUser);
                ParkingDTO mapped = mapper.Map<ParkingDTO>(result);
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
        /// /api/v1.0/parking/exportgatan  To get parking by street name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="includeLinks"></param>
        /// <param name="includeFeedback"></param>
        /// <param name="includeUser"></param>
        /// <returns></returns>
        [HttpGet("{name}", Name = "GetParkingByStreetName")]
        public async Task<IActionResult> GetParkingByStreetName(string name,
            [FromQuery] bool includeLinks = true, [FromQuery] bool includeFeedback = false, [FromQuery] bool includeUser = false)
        {
            try
            {
                var result = await repo.GetParkingByStreet(name, includeFeedback, includeUser);
                var mapped = mapper.Map<ParkingDTO>(result);
                if (includeLinks)
                {
                    mapped.feedbacks = mapped.feedbacks.Select(x => HateoasMainLinks(x)).ToList();
                }
                return Ok(HateoasMainLinks(mapped));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }
        }

        /// <summary>
        /// /api/v1.0/parking    To create a parking
        /// </summary>
        /// <param name="parkingDto"></param>
        /// <returns></returns>
        [HttpPost(Name ="PostParking")]
        public async Task<IActionResult>PostParking(ParkingDTO parkingDto)
        {
            try
            {
                var mappedEntity = mapper.Map<Parking>(parkingDto);
                repo.Add(mappedEntity);
                if ( await repo.Save())
                {
                    return Created($"/api/v1.0/parking/{parkingDto.ParkingID}", mapper.Map<ParkingDTO>(mappedEntity));
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }

        }

        /// <summary>
        /// /api/v1.0/parking/1    To update a parking
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parkingDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}", Name ="UpdateParkingByID")]
        public async Task<IActionResult>UpdateParkingByID(int id, ParkingDTO parkingDto)
        {
            try
            {
                var oldParking = await repo.GetParkingById(id);
                if (oldParking==null)
                {
                    return NotFound($"Parking with ID: {id} could not be found");
                }
                var newParking = mapper.Map(parkingDto, oldParking);
                repo.Update(newParking);

                if (await repo.Save())
                {
                    return NoContent();
                }
                else
                    return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }
        }

        /// <summary>
        /// /api/v1.0/parking/1    To delete parking by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}", Name ="DeleteParkingBydID")]
        public async Task<IActionResult>DeleteParkingByID(int id)
        {
            try
            {
                var parking = await repo.GetParkingById(id);
                if (parking==null)
                {
                    return NotFound($"Parking with ID: {id}  could not be found");
                }
                repo.Delete(parking);
                if (await repo.Save())
                {
                    return NoContent();
                }
                else
                    return BadRequest();

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database failure {e.Message}");
            }
        }
    }
}
