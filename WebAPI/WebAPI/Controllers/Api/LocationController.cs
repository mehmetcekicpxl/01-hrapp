using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Models;

namespace WebAPI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly AppDbContext _context;
        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            try
            {
                var locations = await _context.Locations.ToListAsync();
                return Ok(locations);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetDetails(long id)
        {
            Location? location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpPost]
        public async Task<ActionResult<Location>> AddLocation(CreateLocationRequest request)
        {
            try
            {
                Location location = new Location
                {
                    City = request.City,
                    State = request.State
                };
                await _context.Locations.AddAsync(location);
                await _context.SaveChangesAsync();

                return Ok(location);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLocation(Location model)
        {
            try
            {
                _context.Locations.Update(model);
                int rows = await _context.SaveChangesAsync();
                if (rows == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLocation(long id)
        {
            try
            {
                Location? location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    return NotFound();
                }
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
