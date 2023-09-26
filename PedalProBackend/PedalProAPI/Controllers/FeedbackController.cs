﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedalProAPI.Context;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using PedalProAPI.ViewModels;
using System.Security.Claims;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedbackController : ControllerBase
    {

        private readonly UserManager<PedalProUser> _userManager;
        private readonly IRepository _repository;
        private readonly IUserClaimsPrincipalFactory<PedalProUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly PedalProDbContext _context;

        public FeedbackController(UserManager<PedalProUser> userManager, ILogger<AuthenticationController> logger, IUserClaimsPrincipalFactory<PedalProUser> claimsPrincipalFactory, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IRepository repository, PedalProDbContext context)
        {
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _repository = repository;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllFeedback")]
        public async Task<IActionResult> GetAllFeedback()
        {
            var feedbacks = await _repository.GetAllFeedbackAsync();
            return Ok(feedbacks);
        }

        [HttpGet]
        [Route("GetFeedback/{feedbackId}")]
        public async Task<IActionResult> GetFeedback(int feedbackId)
        {
            try
            {
                var result = await _repository.GetFeedbackAsync(feedbackId);
                if (result == null) return NotFound("Feedback does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("ProvideFeedback")]
        public async Task<IActionResult> ProvideFeedback(FeedbackViewModel feedbackAdd)
        {
            

            var feedback = new Feedback
            {
                FeedbackDescription = feedbackAdd.FeedbackDescription,
                FeedbackRating = feedbackAdd.FeedbackRating,
                FeedbackCategoryId = feedbackAdd.FeedbackCategoryId,
            };

            try
            {
                _repository.Add(feedback);
                await _repository.SaveChangesAsync();

            }
            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            return Ok(feedback);
        }

        [HttpGet]
        [Route("GetAllFeedbackCategories")]
        public async Task<IActionResult> GetAllFeedbackCategories()
        {
            var feedbackCategories = await _repository.GetAllFeedbackCategoriesAsync();
            return Ok(feedbackCategories);
        }

        [HttpGet]
        [Route("GetFeedbackCategory/{feedbackCategoryId}")]
        public async Task<IActionResult> GetFeedbackCategory(int feedbackCategoryId)
        {
            try
            {
                var result = await _repository.GetFeedbackCategoryAsync(feedbackCategoryId);
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
