using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using PedalProAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientTypeController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly UserManager<PedalProUser> _userManager;
        public ClientTypeController(IRepository repository, UserManager<PedalProUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("GetAllClientTypes")]
        public async Task<IActionResult> GetAllClientType()
        {
            var ClientTypes = await _repository.GetAllClientTypeAsync();
            return Ok(ClientTypes);
        }

        [HttpGet]
        [Route("GetAllClientType/{clientTypeId}")]
        public async Task<IActionResult> GetClientType(int clientTypeId)
        {
            try
            {
                var result = await _repository.GetClientTypeAsync(clientTypeId);
                if (result == null) return NotFound("Client Type does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddClientTypes")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> AddClientTypes(ClientTypeViewModel cvm)
        {
            var existingType = await _repository.GetClientTypeByNameAsync(cvm.ClientTypeName);
            if (existingType != null)
            {
                return BadRequest("Client type already exists.");
            }

            var clientType = new ClientType { ClientTypeName = cvm.ClientTypeName };

            try
            {
                _repository.Add(clientType);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            return Ok(clientType);
        }

        [HttpPut]
        [Route("EditClientType/{clientTypeId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ClientTypeViewModel>> EditClientType(int clientTypeId, ClientTypeViewModel clientModel)
        {
            try
            {
                var existingclientType = await _repository.GetClientTypeAsync(clientTypeId);
                if (existingclientType == null) return NotFound($"The Client Type does not exist");

                var existingType = await _repository.GetClientTypeByNameAsync(clientModel.ClientTypeName);
                if (existingType != null)
                {
                    return BadRequest("Client type already exists.");
                }

                existingclientType.ClientTypeName = clientModel.ClientTypeName;


                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingclientType);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }


        [HttpDelete]
        [Route("DeleteClientType/{clientTypeId}")]
        
        public async Task<IActionResult> DeleteClientType(int clientTypeId)
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
                var existingclientType = await _repository.GetClientTypeAsync(clientTypeId);
                if (existingclientType == null) return NotFound($"The client type does not exist");

                _repository.Delete(existingclientType);

                if (await _repository.SaveChangesAsync()) return Ok(existingclientType);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }
    }
}
