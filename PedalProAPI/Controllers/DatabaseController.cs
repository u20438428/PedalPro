using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Timers;
using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting.Server;
using static NBitcoin.Scripting.PubKeyProvider;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PedalProAPI.Models;
using Google.Type;
using PedalProAPI.Repositories;


namespace PedalProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DatabaseController : ControllerBase
    {
        private readonly string connectionString = "Server=.\\SQLEXPRESS;Database=FinalFinalFinalPedalPedalProDBDBDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=true";
        private System.Timers.Timer backupTimer;
        private readonly UserManager<PedalProUser> _userManager;
        private readonly IRepository _repository;

      
        public DatabaseController(UserManager<PedalProUser> userManager, IRepository repository)
        {
            _userManager = userManager;
            _repository = repository;

            var help = CalculateIntervalUntilMidnight().Result;

            
            // Create a timer that triggers the backup every day at midnight
            backupTimer = new System.Timers.Timer
            {
                Interval = help,
                AutoReset = true,
                Enabled = true
            };
            backupTimer.Elapsed += BackupTimerElapsed;
        }


        [HttpPut]
        [Route("UpdateHoursBackup/{hours}")]
        
        public async Task<IActionResult> UpdateHoursBackup(int hours)
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
                var databasetimer = await _repository.GetDatabaseTimerTwo(1);

                if(databasetimer==null)
                {
                    return BadRequest("No databasetimer found");
                }

                databasetimer.DatabaseTimerHours = hours;

                await _repository.SaveChangesAsync();

                return Ok(databasetimer);
            }
            catch (Exception)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }

            
        }
        private async Task<double> CalculateIntervalUntilMidnight()
        {
            try{
                var databasetimer = await _repository.GetDatabaseTimer(1);

                if (databasetimer == null)
                {
                    return 0;
                }
                else
                {
                    var timer = databasetimer.DatabaseTimerHours;

                    var now = System.DateTime.Now;
                    var midnight = now.Date.AddHours((double)timer);
                    var interval = (midnight - now).TotalMilliseconds;
                    

                    return (double)timer;
                    
                }
            }
            catch (Exception)
            {
                return 1;
            }
                
            
        }



        private void BackupTimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Perform the backup logic
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var backupPath = @"C:\Backup\PedalPro\backup.bak";
                   
                    var backupQuery = $"BACKUP DATABASE FinalFinalFinalPedalPedalProDBDBDB TO DISK = '{backupPath}'";
                    using (var command = new SqlCommand(backupQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Backup completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating backup: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("ManualBackup")]
        [Authorize(Roles = "Admin")]
        public IActionResult ManualBackup()
        {
            

            try
            {
                BackupTimerElapsed(null, null); // Call the backup logic immediately
                return Ok("Manual backup initiated.");
            }
            catch (Exception ex)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }
        }

        [HttpPost]
        [Route("RestoreDatabase")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreDatabase()
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
                var backupPath = @"C:\Backup\PedalPro\backup.bak";

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var restoreQuery = @"
                USE master;
                RESTORE DATABASE FinalFinalFinalPedalPedalProDBDBDB FROM DISK = @BackupPath WITH REPLACE;
            ";

                    using (var command = new SqlCommand(restoreQuery, connection))
                    {
                        command.Parameters.AddWithValue("@BackupPath", backupPath);
                        command.ExecuteNonQuery();
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("An error occured. If it persists, please contact support");
            }
        }

        
    }
}

