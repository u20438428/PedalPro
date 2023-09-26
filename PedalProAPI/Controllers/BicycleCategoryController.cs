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
    public class BicycleCategoryController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly UserManager<PedalProUser> _userManager;
        public BicycleCategoryController(IRepository repository, UserManager<PedalProUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllBicycleCategories")]
        public async Task<IActionResult> GetAllBicycleCategory()
        {
            var bicycleCategory = await _repository.GetAllBicycleCategoryAsync();
            return Ok(bicycleCategory);
        }

        [HttpGet]
        [Route("GetAllBicycleCategory/{bicycleCategoryId}")]
        public async Task<IActionResult> GetBicycleCategory(int bicycleCategoryId)
        {
            try
            {
                var result = await _repository.GetBicycleCategoryAsync(bicycleCategoryId);
                if (result == null) return NotFound("Bicycle  Category does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddBicycleCategory")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> AddBicycleCategory(BicycleCategoryViewModel cvm)
        {

            var existingCategory = await _repository.GetBicycleCategoryByNameAsync(cvm.BicycleCategoryName);
            if (existingCategory != null)
            {
                return BadRequest("Bicycle category already exists.");
            }

            var bicycleCategory = new BicycleCategory { BicycleCategoryName = cvm.BicycleCategoryName };

            try
            {
                _repository.Add(bicycleCategory);
                await _repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            return Ok(bicycleCategory);
        }
        [HttpPut]
        [Route("EditBicycleCategory/{bicycleCategoryId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<BicycleCategoryViewModel>> EditBicycleCategory(int bicycleCategoryId, BicycleCategoryViewModel bicycleCategoryModel)
        {
            try
            {
                
                var existingBicycleCategory = await _repository.GetBicycleCategoryAsync(bicycleCategoryId);
                if (existingBicycleCategory == null) return NotFound($"The Bicycle category does not exist");

                var existingCategory = await _repository.GetBicycleCategoryByNameAsync(bicycleCategoryModel.BicycleCategoryName);
                if (existingCategory != null)
                {
                    return BadRequest("Bicycle category already exists.");
                }

                existingBicycleCategory.BicycleCategoryName = bicycleCategoryModel.BicycleCategoryName;


                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingBicycleCategory);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }

        [HttpDelete]
        [Route("DeleteCategoryBicycle/{bicycleCategoryId}")]
        
        public async Task<IActionResult> DeleteBicycleCategory(int bicycleCategoryId)
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
                var existingBicycleCategory = await _repository.GetBicycleCategoryAsync(bicycleCategoryId);
                if (existingBicycleCategory == null) return NotFound($"The Bicycle Category does not exist");

                _repository.Delete(existingBicycleCategory);

                if (await _repository.SaveChangesAsync()) return Ok(existingBicycleCategory);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }
    }
}
