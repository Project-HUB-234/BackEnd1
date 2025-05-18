using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.DTOs;
using Project_Hub.Models;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUser(int userId)
        {
            return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePost(UpdatePostDTO updatePostDTO)
        {

            var postToUpdate = await _context.Posts.FindAsync(updatePostDTO.PostId);

            postToUpdate.Content = updatePostDTO.Content;
            postToUpdate.CategoryId = updatePostDTO.PostCategory;
             _context.Posts.Add(postToUpdate);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<Post>> AddPost(AddPostDTO post)
        {
            var newPost = new Post()
            {
                UserId = post.UserId,
                CategoryId = post.PostCategory,
                Content = post.Content
            };
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            //sendEmail

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

 
    }
}
