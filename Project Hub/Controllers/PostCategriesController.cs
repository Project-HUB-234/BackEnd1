using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.DTOs;
using Project_Hub.Models;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostCategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostCategory>>> GetPostCategories()
        {
            return await _context.PostCategories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostCategory>> GetPostCategoryById(int id)
        {
            var postCategory = await _context.PostCategories.FindAsync(id);

            if (postCategory == null)
            {
                return NotFound();
            }

            return postCategory;
        }

        [HttpPut]
        public async Task<IActionResult> PutPostCategory([FromBody] UpdatePostCategoryDTO postCategory)
        {
            if (postCategory.Id == null)
            {
                return BadRequest();
            }
            var updatedCategory = new PostCategory()
            {
                CategoryId = postCategory.Id,
                CategoryName = postCategory.CategoryName
            };
             _context.PostCategories.Update(updatedCategory);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<PostCategory>> PostPostCategory([FromBody]AddPostCategoryDTO postCategory)
        {
            PostCategory newCaegry = new PostCategory()
            {
                CategoryName = postCategory.CategoryName,
            };
            _context.PostCategories.Add(newCaegry);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostCategory(int id)
        {
            var postCategory = await _context.PostCategories.FindAsync(id);
            if (postCategory == null)
            {
                return NotFound();
            }

             _context.PostCategories.Remove(postCategory);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
