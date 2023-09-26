using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using PedalProAPI.ViewModels;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VatController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly UserManager<PedalProUser> _userManager;
        public VatController(IRepository repository, UserManager<PedalProUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("GetVat")]
        public async Task<IActionResult> GetVat()
        {
            try
            {
                var vat =await  _repository.GetVat(2);
                if(vat==null) return NotFound("No Vat could be found");    
                
                return Ok(vat);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpPut]
        [Route("EditVAT/{percentage}")]
        
        public async Task<IActionResult> EditVAT(double percentage)
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

                var vat = await _repository.GetVat(2);
                if (vat == null) return NotFound("No Vat could be found");

                vat.Vatpecerntage = percentage;
                vat.Vatdate = DateTime.Now;


                if (await _repository.SaveChangesAsync())
                {
                    return Ok(vat);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }
    }
}
