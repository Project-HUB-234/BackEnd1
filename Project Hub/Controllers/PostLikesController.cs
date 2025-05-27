using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.DTOs;
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

        [HttpPost("postLikeCounts")]
        public async Task<ActionResult<IEnumerable<UserPostLikesDTO>>> GetPostLikeCounts([FromBody] List<int> id)
        {
            var result = await _context.PostLikes
                .Where(x => id.Contains(x.PostId))
                .GroupBy(x => x.PostId)
                .Select(g => new UserPostLikesDTO
                {
                    PostId = g.Key,
                    LikeCount = g.Count()
                })
                .ToListAsync();

            return Ok(result);
        }


        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<List<int>>> GetLikesByUser(int userId)
        {
            return await _context.PostLikes
                .Where(pl => pl.UserId == userId)
                .Select(x => x.PostId)
                .ToListAsync();
        }

        [HttpGet("AddLike/{postId}/{userId}")]
        public async Task<IActionResult> AddPostLike([FromRoute]int postId, [FromRoute] int userId)
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


        [HttpDelete("{postId}/{userId}")]
        public async Task<IActionResult> DeletePostLike([FromRoute] int postId , [FromRoute] int userId)
        {
            var postLike = await _context.PostLikes.FirstOrDefaultAsync(x=>x.PostId == postId && x.UserId == userId);
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
