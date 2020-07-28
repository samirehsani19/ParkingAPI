using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PakingAPI.DTO;
using PakingAPI.Models;
using PakingAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PakingAPI.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class FeedbackController: HateoasControllerBase
    {
        private readonly IFeedbackRepository repo;
        private readonly IMapper mapper;
        public FeedbackController(IFeedbackRepository repo, IMapper map, IActionDescriptorCollectionProvider provider):base(provider)
        {
            this.repo = repo;
            this.mapper = map;
        }

        /// <summary>
        /// /api/v1.0/feedback       Get All Feedbacks
        /// </summary>
        /// <param name="includeLinks"></param>
        /// <param name="includeParking"></param>
        /// <param name="includeUser"></param>
        /// <returns></returns>
        [HttpGet(Name ="GetAllFeedback")]
        public async Task<ActionResult<FeedbackDTO>>GetAllFeedbacks([FromQuery] bool includeLinks=true,
            [FromQuery] bool includeParking=false ,bool includeUser=false)
        {
            try
            {
                var result = await repo.GetAllFeedbacks(includeParking, includeUser);
                if (result==null)
                {
                    return NotFound();
                }

                IEnumerable<FeedbackDTO> mappedResult = mapper.Map<FeedbackDTO[]>(result);
                if (includeLinks)
                {
                    mappedResult= mappedResult.Select(x => HateoasMainLinks(x));
                }
                return Ok(mappedResult);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Database failure: {e.Message}");
            }
        }

        /// <summary>
        /// /api/v1.0/feedback/1      Get feedback by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeLinks"></param>
        /// <param name="includeParking"></param>
        /// <param name="includeUser"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name ="GetFeedbackByID")]
        public async Task<IActionResult>GetFeedbackByID(int id,[FromQuery]bool includeLinks=true,
            [FromQuery]bool includeParking=false, [FromQuery]bool includeUser=false)
        {
            try
            {
                var result = await repo.GetFeedbackByID(id, includeParking, includeUser);
                FeedbackDTO mapped = mapper.Map<FeedbackDTO>(result);
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
    }
}
