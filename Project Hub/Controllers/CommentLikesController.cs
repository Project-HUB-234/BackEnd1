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

        

        [HttpGet("{commentId}")]
        public async Task<ActionResult<List<CommentLike>>> GetCommentLike(int commentId)
        {
            var commentLike = await _context.CommentLikes.Where(x=>x.CommentId==commentId).ToListAsync();

            if (commentLike == null)
            {
                return NotFound();
            }

            return commentLike;
        }



        [HttpPost]
        public async Task<ActionResult<CommentLike>> PostCommentLike(int commentId , int userId)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentLike(int id)
        {
            var commentLike = await _context.CommentLikes.FindAsync(id);
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
