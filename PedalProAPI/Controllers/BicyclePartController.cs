using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using PedalProAPI.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BicyclePartController : ControllerBase
    {
        private readonly IRepository _repsository;
        private readonly UserManager<PedalProUser> _userManager;
        public BicyclePartController(IRepository repository, UserManager<PedalProUser> userManager)
        {
            _repsository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllBicyclePart")]
        public async Task<IActionResult> GetAllBicyclePart()
        {
            var bicyclePart = await _repsository.GetAllBicyclePartAsync();
             return Ok(bicyclePart);
        }

        [HttpGet]
        [Route("GetBicyclePart/{bicyclePartId}")]
        public async Task<IActionResult> GetBicyclePart(int bicyclePartId)
        {
            try
            {
                var result = await _repsository.GetBicyclePartAsync(bicyclePartId);
                if (result == null) return NotFound("Bicycle part does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpPost]
        [Route("AddbicyclePart")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Addbicyclepart(BicyclePartViewModel bicyclePartAdd)
        {

            var existingPart = await _repsository.GetBicyclePartByNameAsync(bicyclePartAdd.BicyclePartName);
            if (existingPart != null)
            {
                return BadRequest("Bicycle part already exists.");
            }

            var bicyclepartAdd = new BicyclePart { BicyclePartName = bicyclePartAdd.BicyclePartName };

            try
            {
                _repsository.Add(bicyclepartAdd);
                await _repsository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            return Ok(bicyclepartAdd);
        }


        [HttpPut]
        [Route("Editbicyclepart/{bicyclePartId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<BicyclePartViewModel>> Updatebicyclepart(int bicyclePartId, BicyclePartViewModel bicyclePartModel)
        {
            try
            {

                var existingPart = await _repsository.GetBicyclePartAsync(bicyclePartId);
                if (existingPart == null) return NotFound("The Bicycle Part does not exist");

                var existingParttwo = await _repsository.GetBicyclePartByNameAsync(bicyclePartModel.BicyclePartName);
                if (existingParttwo != null)
                {
                    return BadRequest("Bicycle part already exists.");
                }

                existingPart.BicyclePartName = bicyclePartModel.BicyclePartName;

                if (await _repsository.SaveChangesAsync())
                {
                    return Ok(existingPart);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }

        [HttpDelete]
        [Route("DeleteBicyclePart/{bicyclePartId}")]
        
        public async Task<IActionResult> Deletebicyclepart(int bicyclePartId)
        {

            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username not found.");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userId = user.Id;

            var userClaims = User.Claims;

            bool hasAdminRole = userClaims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin");

            if (!hasAdminRole)
            {
                return BadRequest("You do not have the necessary role to perform this action.");
            }

            try
            {
                

                var existingPart = await _repsository.GetBicyclePartAsync(bicyclePartId);
                if (existingPart == null) return NotFound($"The Bicycle Part does not exist");

                _repsository.Delete(existingPart);

                if (await _repsository.SaveChangesAsync()) return Ok(existingPart);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }
    }
}
