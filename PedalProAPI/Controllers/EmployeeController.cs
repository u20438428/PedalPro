using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using PedalProAPI.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PedalProAPI.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : ControllerBase
    {
        private readonly IRepository _repsository;

        private readonly UserManager<PedalProUser> _userManager;
        private readonly IRepository _repository;
        private readonly IUserClaimsPrincipalFactory<PedalProUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly PedalProDbContext _context;

        public EmployeeController(IRepository repository, UserManager<PedalProUser> userManager, ILogger<AuthenticationController> logger, IUserClaimsPrincipalFactory<PedalProUser> claimsPrincipalFactory, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IRepository repsository, PedalProDbContext context)
        {
            _repsository = repository;
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _repository = repsository;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var employees = await _repsository.GetAllEmployeeAsync();
            return Ok(employees);
        }

        [HttpGet]
        [Route("GetAllEmployeeTwo")]
        public async Task<IActionResult> GetAllEmployeeTwo()
        {
            var employees = await _context.Employees
               .Where(t => t.EmpStatusId == 1)
               .ToListAsync();
            return Ok(employees);
        }

        [HttpGet]
        [Route("GetEmployee/{employeeId}")]
        public async Task<IActionResult> GetEmployee(int employeeId)
        {
            try
            {
                var result = await _repsository.GetEmployeeAsync(employeeId);
                if (result == null) return NotFound("Employee does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        
        public async Task<IActionResult> AddEmployee(EmployeeViewModel employeeAdd)
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

            if (!Regex.IsMatch(employeeAdd.Password, @"^(?=.*[A-Z])(?=.*\d.*).*[a-zA-Z\d]{7}$"))
            {
                return BadRequest("Please enter a password that contains at least 1 uppercase letter, 1 number, and is 7 characters long");
            }

            if (!IsValidEmail(employeeAdd.EmailAddress))
            {
                // Return an error response or handle the invalid email address as needed
                return BadRequest("Please enter a valid email address");
            }

            try
            {
                var userTwo = await _userManager.FindByEmailAsync(employeeAdd.EmailAddress);
                if (employeeAdd.EmpPhoneNum.Length == 10)
                {



                    if (userTwo == null)
                    {
                        userTwo = new PedalProUser
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserName = employeeAdd.EmailAddress,
                            Email = employeeAdd.EmailAddress
                        };
                        var result = await _userManager.CreateAsync(userTwo, employeeAdd.Password);
                        if (result.Succeeded)
                        {
                            var empRoleExists = await _roleManager.RoleExistsAsync("Employee");

                            if (!empRoleExists)
                            {
                                var empRole = new IdentityRole("Employee");
                                await _roleManager.CreateAsync(empRole);
                            }
                            await _userManager.AddToRoleAsync(userTwo, "Employee");


                        }
                        else
                        {
                            return BadRequest(result.Errors);
                        }
                    }
                    else
                    {
                        return BadRequest("Account already exists.");
                    }

                    var employee = new Employee
                    {
                        UserId = userTwo.Id,
                        EmpTitle = employeeAdd.EmpTitle,
                        EmpName = employeeAdd.EmpName,
                        EmpSurname = employeeAdd.EmpSurname,
                        EmpPhoneNum = employeeAdd.EmpPhoneNum,
                        EmpStatusId = employeeAdd.EmpStatusId,
                        EmpTypeId = employeeAdd.EmpTypeId,
                        EmpEmailAddress = userTwo.Email
                    };

                    _repsository.Add(employee);
                    await _repsository.SaveChangesAsync();

                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("nathantheawsome1234@gmail.com"));
                    email.To.Add(MailboxAddress.Parse(employee.EmpEmailAddress));
                    email.Subject = "PedalPro: New employee log in details";
                    email.Body = new TextPart(TextFormat.Html)
                    {
                        Text = "<h4>Dear " + employee.EmpTitle + " " + employee.EmpSurname + "</h4> <p>Your new details to access the PedalPro system are as follows: <br/> Email: " + employee.EmpEmailAddress + " <br/>Username:" + userTwo.UserName + " <br/> Password: " + employeeAdd.Password + "</p>"+
                        "<p>We look forward to having you on our team.<br/>Kind regards<br/>CBT Team</p>"
                    };

                    using var smtp = new SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("nathantheawsome1234@gmail.com", "fanmgdiiigkpjnsc");
                    smtp.Send(email);
                    smtp.Disconnect(true);

                    return Ok(employee);
                }
                else
                {
                    return BadRequest("Please enter a valid South African phone number. It must have exactly 10 digits.");
                }

            }
            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Define a regular expression for a valid email address
            const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        [HttpPut]
        [Route("EditEmployee/{employeeId}")]
        
        public async Task<ActionResult<EmployeeViewModel>> EditEmployee(int employeeId, EmployeeViewModelTwo employeeModel)
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

                var existingEmployee = await _repsository.GetEmployeeAsync(employeeId);
                if (existingEmployee == null) return NotFound("The Employee does not exist");

                var existingEmployeetwo = await _repsository.GetEmployeeAsync(employeeId);

                existingEmployee.EmpTitle = employeeModel.EmpTitle;
                existingEmployee.EmpName = employeeModel.EmpName;
                existingEmployee.EmpSurname = employeeModel.EmpSurname;
                existingEmployee.EmpStatusId = employeeModel.EmpStatusId;
                existingEmployee.EmpTypeId = employeeModel.EmpTypeId;


                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("nathantheawsome1234@gmail.com"));
                email.To.Add(MailboxAddress.Parse(existingEmployee.EmpEmailAddress));
                email.Subject = "PedalPro: Updated employee log in details";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<h4>Dear " + existingEmployee.EmpTitle + " " + existingEmployee.EmpSurname + "</h4> <p>Your updated details to access the PedalPro system are as follows: <br/> Email: " + existingEmployee.EmpEmailAddress + " <br/>Username:" + existingEmployee.EmpEmailAddress + " <br/> Password:Your original password please " + "</p>"+
                    "<p>Kind regards<br/>CBT Team</p>"
                };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("nathantheawsome1234@gmail.com", "fanmgdiiigkpjnsc");
                smtp.Send(email);
                smtp.Disconnect(true);





                if (await _repsository.SaveChangesAsync())
                {
                    return Ok(existingEmployee);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }

        [HttpDelete]
        [Route("DeleteEmployee/{employeeId}")]
        
        public async Task<IActionResult> DeleteEmployee(int employeeId)
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
                
                var existingEmployee = await _repsository.GetEmployeeAsync(employeeId);
                if (existingEmployee == null) return NotFound($"The employee does not exist");

                var existingUser = await _repsository.GetUserAsync(existingEmployee.UserId.ToString());
                if (existingUser != null)
                {
                    _repsository.Delete(existingUser);
                }

                _repsository.Delete(existingEmployee);

                if (await _repsository.SaveChangesAsync()) return Ok(existingEmployee);

            }
            catch
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        }


        [HttpGet]
        [Route("GetAllEmployeeStatuses")]
        public async Task<IActionResult> GetAllEmployeeStatus()
        {
            var employeeStatuses = await _repsository.GetAllEmployeeStatusAsync();
            return Ok(employeeStatuses);
        }

        [HttpGet]
        [Route("GetEmployeeStatus/{employeeStatusId}")]
        public async Task<IActionResult> GetEmployeeStatus(int employeeStatusId)
        {
            try
            {
                var result = await _repsository.GetEmployeeStatusAsync(employeeStatusId);
                if (result == null) return NotFound("Employee status does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

    }
}
