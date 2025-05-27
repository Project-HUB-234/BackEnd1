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
    public class CommentController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;

        public CommentController(AppDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
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
        public async Task<IActionResult> UpdateComment([FromForm]UpdateCommentDTO comment)
        {

            var commentToUpdate = await _context.Comments.FindAsync(comment.CommentId);

            commentToUpdate.Content = comment.Content;
            _context.Comments.Update(commentToUpdate);
            if(comment.Image is not null)
            {
                var oldImage = await _context.Attachments.FirstOrDefaultAsync(x => x.CommentId == comment.CommentId);
                if(oldImage is not null)
                {
                    oldImage.AttachmentPath = _imageService.UploadImage(comment.Image);
                    _context.Attachments.Update(oldImage);
                }
                else
                {
                    var image = new Attachment()
                    {
                        AttachmentPath = _imageService.UploadImage(comment.Image),
                        CommentId = comment.CommentId
                    };
                    _context.Attachments.Add(image);
                }
            }
            if (comment.RemoveImage)
            {
                var imageToDelete = await _context.Attachments.FirstOrDefaultAsync(x => x.CommentId == comment.CommentId);
                _context.Attachments.Remove(imageToDelete);
            }

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
            if(comment.image is not null)
            {
                var image = new Attachment()
                {
                    CommentId = commentToAdd.CommentId,
                    AttachmentPath = _imageService.UploadImage(comment.image)
                };
                _context.Attachments.Add(image);
                await _context.SaveChangesAsync();
            }
           

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments
                .Include(c=>c.CommentLikes)
                .Include(a=>a.Attachments)
                .FirstOrDefaultAsync(x=>x.CommentId ==id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            _context.CommentLikes.RemoveRange(comment.CommentLikes);
            _context.Attachments.RemoveRange(comment.Attachments);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
