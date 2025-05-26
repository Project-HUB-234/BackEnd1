using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.Models;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentLikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentLikesController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet("{userId}")]
        public async Task<ActionResult<List<int>>> GetCommentLike(int userId)
        {
            var commentLike = await _context.CommentLikes
                .Where(x => x.UserId == userId)
                .Select(x=>x.CommentId)
                .ToListAsync();

            if (commentLike == null)
            {
                return NotFound();
            }

            return commentLike;
        }



        [HttpGet("{commentId}/{userId}")]
        public async Task<ActionResult<CommentLike>> AddCommentLike(int userId, int commentId)
        {
            var newCommentLike = new CommentLike()
            {
                CommentId = commentId,
                UserId = userId
            };

            _context.CommentLikes.Add(newCommentLike);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{commentId}/{userId}")]
        public async Task<IActionResult> DeleteCommentLike(int userId , int commentId)
        {
            var commentLike = await _context.CommentLikes.FirstOrDefaultAsync(x=>x.UserId == userId && x.CommentId == commentId);
            if (commentLike == null)
            {
                return NotFound();
            }

            _context.CommentLikes.Remove(commentLike);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
