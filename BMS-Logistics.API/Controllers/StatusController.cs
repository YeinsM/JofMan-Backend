using BMS_Logistics.Application.Interfaces;
using BMS_Logistics.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BMS_Logistics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IGeneric<Status> _genericRepository;

        public StatusController(IGeneric<Status> genericRepository)
        {
            _genericRepository = genericRepository;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Status))]
        public async Task<ActionResult<IEnumerable<Status>>> GetAll()
        {
            var data = await _genericRepository.GetAll();
            return Ok(data);
        }
    }
}
