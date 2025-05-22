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

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery]LoginDTO loginDTO)
        {
            var login = await _context.Logins.FirstOrDefaultAsync(x=>x.Username == loginDTO.UserName && x.PasswordHash == loginDTO.PasswordHash);
            if(login == null)
            {
                return BadRequest(new { message = "Invalid username or password" });
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
            var email = new EmailDTO()
            {
                Receiver = updatePassword.Email,
                Title = "Update Password",
                Body = $"We are happy to inform you that your password updated successfully.\n\nRegards,"
            };
            await _emailService.SendEmailAsync(email);

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

            var emaill = new EmailDTO()
            {
                Receiver = email,
                Title = "Forget Password",
                Body = $"We are happy to inform you that your new password is {newPassword}  .\n\nRegards,"
            };
            await _emailService.SendEmailAsync(emaill);

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
