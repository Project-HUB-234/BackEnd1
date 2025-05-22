using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Hub.Data;
using Project_Hub.DTOs;
using Project_Hub.Models;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet("ByPost/{postId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByPost(int postId)
        {
            return await _context.Comments.Where(c => c.PostId == postId).ToListAsync();
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetCommentsByUser(int userId)
        {
            return await _context.Comments.Where(c => c.UserId == userId).ToListAsync();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentDTO comment)
        {

            var commentToUpdate = await _context.Comments.FindAsync(comment.CommentId);

            commentToUpdate.Content = comment.Content;
            _context.Comments.Update(commentToUpdate);

            await _context.SaveChangesAsync();



            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> AddComment(AddCommentDTO comment)
        {
            var commentToAdd = new Comment()
            {
                PostId = comment.PostId,
                UserId = comment.UserId,
                Content = comment.Contant

            };
            _context.Comments.Add(commentToAdd);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
