using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using PedalProAPI.ViewModels;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BicycleBrandController : ControllerBase
    {
        private readonly IRepository _repsository;
        private readonly UserManager<PedalProUser> _userManager;

        public BicycleBrandController(IRepository repository, UserManager<PedalProUser> userManager)
        {
            _repsository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllBicycleBrands")]
        
        public async Task<IActionResult> GetAllBicycleBrands()
        {
            var bicycleBrands = await _repsository.GetAllBicycleBrandAsync();
            return Ok(bicycleBrands);
        }


        [HttpGet]
        [Route("GetAllBicycleBrandsClient/{bicycleCategoryId}")]
        public async Task<IActionResult> GetAllBicycleBrandsClient(int bicycleCategoryId)
        {
            var bicycleBrands = await _repsository.GetBrandCategoriesAsync(bicycleCategoryId);
            return Ok(bicycleBrands);
        }

        [HttpGet]
        [Route("GetBicycleBrand/{bicycleBrandId}")]
        public async Task<IActionResult> GetBicycleBrand(int bicycleBrandId)
        {
            try
            {
                var result = await _repsository.GetBicycleBrandAsync(bicycleBrandId);
                if (result == null) return NotFound("Bicycle Brand does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddBicycleBrand")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> AddBicycleBrand(BicycleBrandViewModel brandAdd)
        {
            
            
            var existingBrand = await _repsository.GetBicycleBrandByNameAsync(brandAdd.BrandName);
            if (existingBrand != null)
            {
                return BadRequest("Brand name already exists.");
            }

            var existingBrandImage = await _repsository.GetBrandImageByNameAsync(brandAdd.BrandImgName);
            if (existingBrandImage != null)
            {
                return BadRequest("Brand image name already exists.");
            }

            var brandImage = new BrandImage
            {
                ImageTypeId = brandAdd.ImageTypeId,
                BrandImgName=brandAdd.BrandImgName,
                ImageUrl=brandAdd.ImageUrl
            };

            var brand = new BicycleBrand();

            try
            {

                _repsository.Add(brandImage);
                await _repsository.SaveChangesAsync();

                var test = await _repsository.GetBrandImageAsync(brandImage.BrandImageId);

                brand.BrandName = brandAdd.BrandName;
                brand.BrandImageId = test.BrandImageId;
                brand.BicycleCategoryId = brandAdd.BicycleCategoryId;

                _repsository.Add(brand);
                await _repsository.SaveChangesAsync();
            }

            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            return Ok(brand);
        }


        [HttpPut]
        [Route("EditBicycleBrand/{bicycleBrandId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> EditBicycleBrand(int bicycleBrandId, BicycleBrandViewModel brandModel)
        {

            try
            {
                var existingBicycleBrand= await _repsository.GetBicycleBrandAsync(bicycleBrandId);
                if (existingBicycleBrand == null) return NotFound("The bicycle brand does not exist");

                var existingBrandImage = await _repsository.GetBrandImageAsync((int)existingBicycleBrand.BrandImageId);
                if (existingBrandImage == null) return NotFound("The brand image does not exist");


                var existingBrand = await _repsository.GetBicycleBrandByNameAsync(brandModel.BrandName);
                if (existingBrand != null)
                {
                    return BadRequest("Brand name already exists.");
                }

                var existingBrandImagetwo = await _repsository.GetBrandImageByNameAsync(brandModel.BrandImgName);
                if (existingBrandImagetwo != null)
                {
                    return BadRequest("Brand image name already exists.");
                }


                existingBrandImage.ImageTypeId = brandModel.ImageTypeId;
                existingBrandImage.BrandImgName = brandModel.BrandImgName;
                existingBrandImage.ImageUrl = brandModel.ImageUrl;
                await _repsository.SaveChangesAsync();

                existingBicycleBrand.BrandName = brandModel.BrandName;
                existingBicycleBrand.BrandImageId= existingBrandImage.BrandImageId;
                existingBicycleBrand.BicycleCategoryId=brandModel.BicycleCategoryId;

                if (await _repsository.SaveChangesAsync())
                {
                    return Ok(existingBicycleBrand);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }



        [HttpDelete]
        [Route("DeleteBicycleBrand/{bicycleBrandId}")]
        
        public async Task<IActionResult> DeleteTrainingMaterial(int bicycleBrandId)
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
                var existingBicycleBrand = await _repsository.GetBicycleBrandAsync(bicycleBrandId);
                if (existingBicycleBrand == null) return NotFound("The bicycle brand does not exist");

                var existingBrandImage = await _repsository.GetImgBrandVidAsync((int)existingBicycleBrand.BrandImageId);
                if (existingBrandImage == null) return NotFound("The brand image does not exist");

                if (existingBrandImage.Any())
                {
                    foreach (var item in existingBrandImage)
                    {
                        _repsository.Delete(item);
                    }
                }

                _repsository.Delete(existingBicycleBrand);

                if (await _repsository.SaveChangesAsync()) return Ok(existingBicycleBrand);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }


        [HttpGet]
        [Route("GetAllImageTypes")]
        public async Task<IActionResult> GetAllImageTypes()
        {
            var imageTypes = await _repsository.GetAllImageTypeAsync();
            return Ok(imageTypes);
        }

        [HttpGet]
        [Route("GetImageType/{imageTypeId}")]
        public async Task<IActionResult> GetImageType(int imageTypeId)
        {
            try
            {
                var result = await _repsository.GetImageTypeAsync(imageTypeId);
                if (result == null) return NotFound("Image Type does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetBrandImage/{brandImageId}")]
        public async Task<IActionResult> GetBrandImage(int brandImageId)
        {
            try
            {
                var result = await _repsository.GetBrandImageAsync(brandImageId);
                if (result == null) return NotFound("Brand image does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

       

        [HttpPost]
        [Route("TestImageUpload")]
        public async Task<IActionResult> TestImageUpload(IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    // Configure your Cloudinary account
                    Account account = new Account(
                        "dcpmharuk",
                        "183493828529672",
                        "869tkBTJoV1UmiO0ubhatZ5rNSs"
                    );

                    Cloudinary cloudinary = new Cloudinary(account);

                    // Upload the image to Cloudinary
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, file.OpenReadStream())
                    };

                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    return Ok(uploadResult);
                }

                return BadRequest("No image uploaded.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
        
    }
}
