﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class HelpController : ControllerBase
    {
        private readonly IRepository _repsository;
        private readonly UserManager<PedalProUser> _userManager;

        public HelpController(IRepository repository, UserManager<PedalProUser> userManager)
        {
            _repsository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllHelp")]
        public async Task<IActionResult> GetAllHelp()
        {
            var helps = await _repsository.GetAllHelpAsync();
            return Ok(helps);
        }

        [HttpGet]
        [Route("GetHelp/{helpId}")]
        public async Task<IActionResult> GetHelp(int helpId)
        {
            try
            {
                var result = await _repsository.GetHelpAsync(helpId);
                if (result == null) return NotFound("Help does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddHelp")]
        
        public async Task<IActionResult> AddHelp(HelpViewModel helpAdd)
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

            var existingHelp = await _repsository.GetPackageByNameAsync(helpAdd.HelpName);
            if (existingHelp != null)
            {
                return BadRequest("Help name already exists.");
            }


            var help = new Help
            {
                HelpName = helpAdd.HelpName,
                HelpDescription = helpAdd.HelpDescription,
                HelpCategoryId = helpAdd.HelpCategoryId
            };
            try
            {
                _repsository.Add(help);
                await _repsository.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            return Ok(help);
        }

        [HttpPut]
        [Route("EditHelp/{helpId}")]
        
        public async Task<ActionResult<HelpViewModel>> UpdateHelp(int helpId, HelpViewModel helpModel)
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
                var existingHelp = await _repsository.GetHelpAsync(helpId);
                if (existingHelp == null) return NotFound("The Help does not exist");


                var existingHelptwo = await _repsository.GetPackageByNameAsync(helpModel.HelpName);
                if (existingHelptwo != null)
                {
                    return BadRequest("Help name already exists.");
                }

                existingHelp.HelpName = helpModel.HelpName;
                existingHelp.HelpDescription = helpModel.HelpDescription;
                existingHelp.HelpCategoryId = helpModel.HelpCategoryId;




                if (await _repsository.SaveChangesAsync())
                {
                    return Ok(existingHelp);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }

        [HttpDelete]
        [Route("DeleteHelp/{helpId}")]
        
        public async Task<IActionResult> DeleteHelp(int helpId)
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
                var existingHelp = await _repsository.GetHelpAsync(helpId);
                if (existingHelp == null) return NotFound($"The help does not exist");


                _repsository.Delete(existingHelp);

                if (await _repsository.SaveChangesAsync()) return Ok(existingHelp);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }


        [HttpGet]
        [Route("GetAllHelpCategories")]
        public async Task<IActionResult> GetAllHelpCategories()
        {
            var helpCategories = await _repsository.GetAllHelpCategoriesAsync();
            return Ok(helpCategories);
        }

        [HttpGet]
        [Route("GetHelpCategory/{helpCategoryId}")]
        public async Task<IActionResult> GetHelpCategory(int helpCategoryId)
        {
            try
            {
                var result = await _repsository.GetHelpCategoryAsync(helpCategoryId);
                if (result == null) return NotFound("Category does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
    }
}
