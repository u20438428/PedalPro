using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using PedalProAPI.Context;
using PedalProAPI.Models;
using PedalProAPI.Repositories;
using System.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Voice;
using Vonage;
using Vonage.Request;
using Vonage.Messages.Sms;
using PedalProAPI.ViewModels;

namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PedalProUserController : ControllerBase
    {
        private readonly PedalProDbContext _context;
        private readonly IRepository _repsository;
        private readonly UserManager<PedalProUser> _userManager;
        
        private readonly IUserClaimsPrincipalFactory<PedalProUser> _claimsPrincipalFactory;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<PedalProUserController> _logger;

        
        public PedalProUserController(PedalProDbContext context, IRepository repsository, UserManager<PedalProUser> userManager, ILogger<PedalProUserController> logger, IUserClaimsPrincipalFactory<PedalProUser> claimsPrincipalFactory, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _repsository = repsository;
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _configuration = configuration;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.PedalProUsers.ToListAsync();
            return Ok(roles);
        }


        [HttpGet]
        [Route("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            var roles = await _context.Clients.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("GetClientsWithBookings")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<ActionResult> GetClientsWithBookings()
        {
           

            var clientIdsWithBookings = await _context.Bookings
                .Select(booking => booking.ClientId)
                .Distinct()
                .ToListAsync();

            var clientsWithBookings = await _context.Clients
                .Where(client => clientIdsWithBookings.Contains(client.ClientId))
                .ToListAsync();

            return Ok(clientsWithBookings);
        }


        [HttpPost]
        [Route("SendBookingReminder/{clientId}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> SendBookingReminder(int clientId)
        {

            var clientbookingRem = await _repsository.GetBookingsReminder(clientId);

            if(clientbookingRem!=null)
            {

                var clientClient=await _repsository.GetClientClient(clientId);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("nathantheawsome1234@gmail.com"));
                email.To.Add(MailboxAddress.Parse(clientClient.ClientEmailAddress));
                email.Subject = "CBT Booking Reminder";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "<h1>Dear " + clientClient.ClientTitle + " " + clientClient.ClientSurname + "<h1/> <p><small>This email serves as a reminder for you booking at Callan's Bike Tech. Please ensure that youa arrive in time for your session.<br/> <br/> If you wish to cancel your booking, please ensure that you do it 24 hours in advance. We hope to see you shortly. <br/><br/> The CBT Team </small></p>"
                };
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("nathantheawsome1234@gmail.com", "fanmgdiiigkpjnsc");
                smtp.Send(email);
                smtp.Disconnect(true);
                var responseObj = new { message = "Booking reminder has been sent" };
                return Ok(responseObj);
            }
            else
            {
                return BadRequest("Client does not have a booking under their name");
            }

            
        }

        [HttpGet]
        [Route("GetClientDetails")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetClientDetails()
        {
            try
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

                var client = await _repsository.GetClient(userId);

                if (client == null)
                {
                    return BadRequest("Client not found.");
                }

                var userClaims = User.Claims;
                bool hasClientRole = userClaims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Client");

                if (!hasClientRole)
                {
                    return Forbid("You do not have the necessary role to perform this action.");
                }

                if(client!=null)
                {
                    return Ok(client);
                }
            }
            catch
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("An error occured. If it persists, please contact support");
        } 


        [HttpPost]
        [Route("SendBookingReminderTwo/{clientId}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> SendBookingReminderTwo(int clientId)
        {

            var clientbookingRem = await _repsository.GetBookingsReminder(clientId);



            string accountSid = "AC68368a5b35f8229b7fb606140930c788";
            string authToken = "dc98852b6355e73666dedf096c036287";


            if (clientbookingRem != null)
            {
                var clientClient = await _repsository.GetClientClient(clientId);

                string clientPhoneNum = clientClient.ClientPhoneNum;
                string cleanedPhoneNum = clientPhoneNum.Replace("0", ""); // Remove all "0" occurrences
                string phoneNum = "+27" + cleanedPhoneNum;
                var name = clientClient.ClientName + " " + clientClient.ClientSurname;

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: "Dear "+ name+ ",this is a reminder of your booking(s) made at CBT. Please ensure that you arrive on time for your specified timeslot. Kind regards, CBT Team",
                    from: new Twilio.Types.PhoneNumber("+12568249491"),
                    to: new Twilio.Types.PhoneNumber(phoneNum)
                //+12722256251
                );

                return Ok($"SMS sent with SID: {message.Sid}");
            }
            else
            {
                return BadRequest("Client does not have a booking under their name");
            }

        }


        [HttpPost]
        [Route("SendSms/{clientId}")]
        public async Task<IActionResult> SendSms(int clientId)
        {
            string apiKey = "3d47e8ea";
            string apiSecret = "bIJKgd8M0rbmcdd2";

            var clientbookingRem = await _repsository.GetBookingsReminder(clientId);


            if (clientbookingRem != null)
            {
                var clientClient = await _repsository.GetClientClient(clientId);

                string clientPhoneNum = clientClient.ClientPhoneNum;
                string cleanedPhoneNum = clientPhoneNum.Replace("0", ""); // Remove all "0" occurrences
                string phoneNum = "+27" + cleanedPhoneNum;
                var name = clientClient.ClientName + " " + clientClient.ClientSurname;
                var message = "Dear " + name + ",this is a reminder of your booking(s) made at CBT. Please ensure that you arrive on time for your specified timeslot. Kind regards, CBT Team";
                VonageClient client = new VonageClient(new Credentials
                {
                    ApiKey = apiKey,
                    ApiSecret = apiSecret
                });

                

                var response = client.SmsClient.SendAnSms(new Vonage.Messaging.SendSmsRequest
                {
                    To = phoneNum,
                    From = "CBT",
                    Text = message
                });

                if (response.Messages[0].Status == "0")
                {
                    // Create a new message object or use any appropriate message you want to return.
                    string successMessage = "SMS sent successfully.";

                    // Return the success message as a JSON response.
                    return Ok(new { message = successMessage });
                }
                else
                {
                    return BadRequest("Failed to send SMS.");
                }
            }
            else
            {
                return BadRequest("Client does not have a booking under their name");
            }

        }
    }
}
