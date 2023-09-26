using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using PedalProAPI.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeTypeController : ControllerBase
    {
        private readonly IRepository _repsository;
        private readonly UserManager<PedalProUser> _userManager;
        public EmployeeTypeController(IRepository repository, UserManager<PedalProUser> userManager)
        {

            _repsository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllEmployeeTypes")]
        public async Task<IActionResult> GetAllEmployeeTypes()
        {
            var employeeTypes = await _repsository.GetAllEmployeeTypeAsync();
            return Ok(employeeTypes);
        }

        [HttpGet]
        [Route("GetEmployeeType/{employeeTypeId}")]
        public async Task<IActionResult> GetEmployeeType(int employeeTypeId)
        {
            try
            {
                var result = await _repsository.GetEmployeeTypeAsync(employeeTypeId);
                if (result == null) return NotFound("Employee Type does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddEmployeetypes")]
        
        public async Task<IActionResult> AddRole(EmployeeTypeViewModel empTypeAdd)
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

            var existingType = await _repsository.GetEmployeeTypeByNameAsync(empTypeAdd.EmpTypeName);
            if (existingType != null)
            {
                return BadRequest("Employee type already exists.");
            }

            var empType = new EmployeeType { EmpTypeName = empTypeAdd.EmpTypeName, EmpTypeDescription = empTypeAdd.EmpTypeDescription };

            try
            {
                _repsository.Add(empType);
                await _repsository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            return Ok(empType);
        }


        [HttpPut]
        [Route("EditEmployeeType/{employeeTypeId}")]
        
        public async Task<ActionResult<EmployeeTypeViewModel>> UpdateEmployeeType(int employeeTypeId, EmployeeTypeViewModel employeeTypeModel)
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
                var existingType = await _repsository.GetEmployeeTypeAsync(employeeTypeId);
                if (existingType == null) return NotFound("The Employee type does not exist");

                var existingTypetwo = await _repsository.GetEmployeeTypeByNameAsync(employeeTypeModel.EmpTypeName);
                if (existingTypetwo != null)
                {
                    return BadRequest("Employee type already exists.");
                }

                existingType.EmpTypeName = employeeTypeModel.EmpTypeName;
                existingType.EmpTypeDescription = employeeTypeModel.EmpTypeDescription;

                if (await _repsository.SaveChangesAsync())
                {
                    return Ok(existingType);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }

        [HttpDelete]
        [Route("DeleteEmployeeType/{employeeTypeId}")]
        
        public async Task<IActionResult> DeleteEmployeeType(int employeeTypeId)
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
                var existingType = await _repsository.GetEmployeeTypeAsync(employeeTypeId);
                if (existingType == null) return NotFound($"The Employee type does not exist");

                _repsository.Delete(existingType);

                if (await _repsository.SaveChangesAsync()) return Ok(existingType);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }
    }
}
