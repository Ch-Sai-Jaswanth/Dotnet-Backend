using BikeDealersProject.Models;
using BikeDealersProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeDealersProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DMController : ControllerBase
    {
        private readonly IDealerMasterService _dmService;

        public DMController(IDealerMasterService dmService)
        {
            _dmService = dmService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DealerMaster>>> GetDealerMasters()
        {
            var dms = await _dmService.GetDMs();
            return Ok(dms);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<DealerMaster>> GetDealerMaster(int id)
        {
            var dm = await _dmService.FindDMById(id);
            if (dm == null)
            {
                return NotFound();
            }
            return Ok(dm);
        }

        [AllowAnonymous]
        [HttpGet("exists/{id}")]
        public async Task<IActionResult> CheckDMExists(int id)
        {
            var exists = await _dmService.Exists(id);
            return Ok(exists);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DealerMaster>> AddDealerMaster(DealerMaster dm)
        {
            var result = await _dmService.AddDealerMaster(dm);
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetDealerMaster), new { id = dm.DealerMasterId }, dm);
            }
            return BadRequest("Failed to add DealerMaster entry.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("bulk")]
        public async Task<IActionResult> AddDealerMastersBulk(List<DealerMaster> dealerMasters)
        {
            var result = await _dmService.AddDealerMastersBulk(dealerMasters);
            if(result > 0)
            {
                return Ok(new { Inserted = result });
            }
            return BadRequest("Failed to add DealerMasters.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDealerMaster(int id, DealerMaster dm)
        {
            var result = await _dmService.UpdateDM(id, dm);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDealerMaster(int id)
        {
            var result = await _dmService.DeleteDM(id);
            if (result == 0)
            {
                return NotFound(new { message = "DealerMaster not found" });
            }
            return Ok(new { message = "DealerMaster deleted successfully" });
        }
    }
}
