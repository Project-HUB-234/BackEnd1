using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.DTOs;
using Project_Hub.Models;
using Project_Hub.Services;
using System.Security.Cryptography;
using System.Text;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private static readonly Random random = new Random();
        private readonly EmailService _emailService;
        public LoginController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/Login
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery]LoginDTO loginDTO)
        {
            var login = await _context.Logins.FirstOrDefaultAsync(x=>x.Username == loginDTO.UserName && x.PasswordHash == loginDTO.PasswordHash);
            if(login == null)
            {
                return BadRequest();
            }
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Username);

            var loginResult = new LoginResponseDTO()
            {
                UserId = user.UserId,
                RoleId = user.RoleId,
                UserName = login.Username
            };
            return Ok(loginResult);
        }


        [HttpPut]
        [Route("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword updatePassword)
        {

            var loginInfo = await _context.Logins.FirstOrDefaultAsync(x => x.Username == updatePassword.Email && x.PasswordHash == updatePassword.OldPassword);

            

            if (loginInfo == null)
            {
                return BadRequest();
            }
            loginInfo.PasswordHash = updatePassword.NewPassword;

            _context.Logins.Update(loginInfo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("forget-password/{email}")]
        public async Task<IActionResult> ForgetPassword(string email)
        {

            var loginInfo = await _context.Logins.FirstOrDefaultAsync(x => x.Username == email);
            if (loginInfo == null)
            {
                return Ok();
            }

            string newPassword = GeneratePassword();
           
            loginInfo.PasswordHash = newPassword;

            _context.Logins.Update(loginInfo);
            await _context.SaveChangesAsync();

            //sendEmail

            return Ok();
        }


        public static string GeneratePassword()
        {
            
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_=+";
            var password = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }


    }


}
