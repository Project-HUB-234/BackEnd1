using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.Models;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostLikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostLikesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<List<PostLike>>> GetPostLikesById(int postId)
        {
            return await _context.PostLikes.Where(x => x.PostId == postId).ToListAsync();
        }


        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<List<PostLike>>> GetLikesByUser(int userId)
        {
            return await _context.PostLikes.Where(pl => pl.UserId == userId).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddPostLike([FromBody] int postId, int userId)
        {
            var newLike = new PostLike()
            {
                PostId = postId,
                UserId = userId
            };

            _context.PostLikes.Add(newLike);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("{likeId}")]
        public async Task<IActionResult> DeletePostLike(int likeId)
        {
            var postLike = await _context.PostLikes.FindAsync(likeId);
            if (postLike == null)
            {
                return NotFound();
            }

            _context.PostLikes.Remove(postLike);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
