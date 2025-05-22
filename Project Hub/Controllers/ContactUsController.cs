using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.DTOs;
using Project_Hub.Models;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactUsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactU>>> GetContactUs()
        {
            return await _context.ContactUs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactU>> GetContactUs(int id)
        {
            var contactU = await _context.ContactUs.FindAsync(id);

            if (contactU == null)
            {
                return NotFound();
            }

            return contactU;
        }

        [HttpPost]
        public async Task<IActionResult> PostContactUs([FromBody] AddContactUsDTO contactUs)
        {
            var newContactUs = new ContactU()
            {
                UserEmail = contactUs.UserEmail,
                ContactTitle = contactUs.ContactTitle,
                ContactMessage = contactUs.ContactMessage
            };
            _context.ContactUs.Add(newContactUs);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactUs(int id)
        {
            var contactU = await _context.ContactUs.FindAsync(id);
            if (contactU == null)
            {
                return NotFound();
            }

            _context.ContactUs.Remove(contactU);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
