using BikeDealersProject.Models;
using BikeDealersProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeDealersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerController : ControllerBase
    {
        private readonly IDealerService _dealerService;

        public DealerController(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dealer>>> GetDealers()
        {
            var dealers = await _dealerService.GetDealers();
            return Ok(dealers);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Dealer>> GetDealer(int id)
        {
            var dealer = await _dealerService.FindDealerById(id);
            if (dealer == null)
            {
                return NotFound();
            }
            return Ok(dealer);
        }

        [AllowAnonymous]
        [HttpGet("byname/{name}")]
        public async Task<ActionResult<Dealer>> GetDealerByName(string name)
        {
            var dealer = await _dealerService.FindDealerByName(name);
            if (dealer == null)
            {
                return NotFound();
            }
            return Ok(dealer);
        }

        [AllowAnonymous]
        [HttpGet("exists/{id}")]
        public async Task<IActionResult> CheckDealerExists(int id)
        {
            var result = await _dealerService.Exists(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Dealer")]
        [HttpPost]
        public async Task<ActionResult<Dealer>> PostDealer(Dealer dealer)
        {
            var result = await _dealerService.AddDealer(dealer);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetDealer), new { id = dealer.DealerId }, dealer);
            }
            return BadRequest("Failed to add dealer.");
        }

        [Authorize(Roles = "Admin,Dealer")]
        [HttpPost("bulk")]
        public async Task<IActionResult> AddDealersBulk(List<Dealer> dealers)
        {
            var result = await _dealerService.AddDealersBulk(dealers);
            if (result > 0)
            {
                return Ok(new { Inserted = result });
            }
            return BadRequest("Failed to add dealers.");
        }

        [Authorize(Roles = "Admin,Dealer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDealer(int id, Dealer dealer)
        {
            var result = await _dealerService.UpdateDealer(id, dealer);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealer(int id)
        {
            var result = await _dealerService.DeleteDealer(id);
            if (result == 0)
            {
                return NotFound(new { message = "Dealernot found" });
            }
            return Ok(new { message = "Dealer deleted successfully" });
        }
    }
}
