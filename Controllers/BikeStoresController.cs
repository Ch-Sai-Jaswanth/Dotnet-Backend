using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeDealersProject.Models;
using BikeDealersProject.Services;
using Microsoft.AspNetCore.Authorization;

namespace BikeDealersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeStoreController : ControllerBase
    {
        private readonly IBikeService _bikeService;

        public BikeStoreController(IBikeService bikeService)
        {
            _bikeService = bikeService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeStore>>> GetBikes()
        {
            var bikes = await _bikeService.GetBikes();
            return Ok(bikes);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<BikeStore>> GetBike(int id)
        {
            var bike = await _bikeService.FindBikeById(id);
            if (bike == null)
            {
                return NotFound();
            }
            return Ok(bike);
        }

        [AllowAnonymous]
        [HttpGet("byname/{name}")]
        public async Task<ActionResult<BikeStore>> GetBikeByName(string name)
        {
            var bike = await _bikeService.FindBikeByName(name);
            if (bike == null)
            {
                return NotFound();
            }
            return Ok(bike);
        }

        [Authorize(Roles = "Admin,Producer")]
        [HttpPost]
        public async Task<ActionResult<BikeStore>> PostBike(BikeStore bike)
        {
            var result = await _bikeService.AddBike(bike);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetBike), new { id = bike.BikeId }, bike);
            }
            return BadRequest("Failed to add bike.");
        }

        [Authorize(Roles = "Admin,Producer")]
        [HttpPost("bulk")]
        public async Task<IActionResult> AddBikesBulk(List<BikeStore> bikeStores)
        {
            var result = await _bikeService.AddBikesBulk(bikeStores);
            if (result > 0)
            {
                return Ok(new { Inserted = result });
            }
            return BadRequest("Failed to add bikes.");
        }

        [Authorize(Roles = "Admin,Producer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, BikeStore bike)
        {
            var result = await _bikeService.UpdateBike(id, bike);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            var result = await _bikeService.DeleteBike(id);
            if (result == 0)
            {
                return NotFound();
            }
            return Ok(new { message = "Bike deleted successfully." });
        }
    }
}