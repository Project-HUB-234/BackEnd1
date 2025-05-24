using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.DTOs;
using Project_Hub.Models;
using Project_Hub.Services;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        private readonly EmailService _emailService;

        public UsersController(AppDbContext context, ImageService imageService, EmailService emailService)
        {
            _context = context;
            _imageService = imageService;
            _emailService = emailService;
        }

        [HttpGet]

        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            return await _context.Users.Select(u => new UserDTO
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                QuickAccessQrcode = u.QuickAccessQrcode,
                ProfilePicture = u.ProfilePicture,
                BackgroundPicture = u.BackgroundPicture,
                DateJoined = u.DateJoined,
            }).ToListAsync();
        }

        [HttpGet]
        [Route("user")]
        public async Task<ActionResult<UserDTO>> GetUser([FromQuery] int userId)
        {
            var user = await _context.Users.FindAsync(userId);


            if (user == null)
            {
                return BadRequest();
            }
            UserDTO userDTO = new UserDTO()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = user.ProfilePicture,
                PhoneNumber = user.PhoneNumber,
                QuickAccessQrcode = user.QuickAccessQrcode,
                BackgroundPicture = user.BackgroundPicture,
                DateJoined = user.DateJoined,
                Brif = user.Brif,
                Job = user.Job,
                Address = user.Address
            };
            return userDTO;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUser)
        {

            var user = await _context.Users.FindAsync(updateUser.UserId);
            if (user == null)
            {
                return BadRequest();
            }
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.PhoneNumber = updateUser.PhoneNumber;
            if (updateUser.QuickAccessQrcode != null)
            {
                user.QuickAccessQrcode = _imageService.UploadImage(updateUser.QuickAccessQrcode);
            }
            if (updateUser.ProfilePicture != null)
            {
                user.ProfilePicture = _imageService.UploadImage(updateUser.ProfilePicture);
            }
            if (updateUser.BackgroundPicture != null)
            {
                user.ProfilePicture = _imageService.UploadImage(updateUser.BackgroundPicture);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            var email = new EmailDTO()
            {
                Receiver = updateUser.UserName,
                Title = "Update Profile",
                Body = $"Dear {updateUser.FirstName} {updateUser.LastName}\n\nWe are happy to inform you that your personal data updated successfully.\n\nRegards,"
            };
            await _emailService.SendEmailAsync(email);


            return Ok();
        }

        // POST: api/Users
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<User>> Rigester([FromBody] RigesterDTO newUser)
        {

            var Existuser = await _context.Users.AnyAsync(x => x.Email == newUser.Email);
            if (Existuser)
            {
                return BadRequest(new { message = "User already exists" });
            }
            

            var user = new User()
            {
                Email = newUser.Email,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                PhoneNumber = null,
                RoleId = 2,
                QuickAccessQrcode = null,
                ProfilePicture = "https://localhost:7291/images/default-profile.jpg",
                BackgroundPicture = "https://localhost:7291/images/default-background.png",
                Address = null,
                Job = null
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var loginData = new Login()
            {
                Username = newUser.Email,
                PasswordHash = newUser.HashedPassword,
                UserId = user.UserId
            };
            _context.Logins.Add(loginData);
            await _context.SaveChangesAsync();
            var email = new EmailDTO()
            {
                Receiver = newUser.Email,
                Title = "Creat New Account",
                Body = $"Dear {newUser.FirstName} {newUser.LastName}\n\nWe are happy to inform you that you have successfully created a new account on our website.\n\nRegards,"
            };
            await _emailService.SendEmailAsync(email);
            return Ok(new { message = "Registration successful" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return BadRequest();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("getUserImage{userId}")]
        public async Task<ActionResult<string>> GetUserImage(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                return BadRequest();
            }
            return Ok(user!.ProfilePicture);
        }
    }
}
