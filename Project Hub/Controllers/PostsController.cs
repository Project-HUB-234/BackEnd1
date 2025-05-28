using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Project_Hub.Data;
using Project_Hub.DTOs;
using Project_Hub.Models;
using Project_Hub.Services;

namespace Project_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        private readonly EmailService _emailService;

        public PostsController(AppDbContext context, ImageService imageService, EmailService emailService)
        {
            _context = context;
            _imageService = imageService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            var t= await _context.Posts
                .Include(p => p.User)
                .Include(pl => pl.PostLikes)
             .Include(p => p.Attachments)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.User)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.CommentLikes)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.Attachments)
                 .OrderByDescending(x => x.DatePosted)
             .ToListAsync();
            return t;
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
        [HttpGet("ByUser")]
        public async Task<List<Post>> GetPostsByUser([FromQuery] int userId)
        {
            return await _context.Posts
                .Where(u=>u.UserId==userId)
                .Include(p => p.User)
                .Include(pl=>pl.PostLikes)
             .Include(p => p.Attachments)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.User)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.CommentLikes)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.Attachments)
                 .OrderByDescending(x => x.DatePosted)
             .ToListAsync();




        }

        [HttpPut("UpdatePost")]

        public async Task<IActionResult> UpdatePost([FromForm] UpdatePostDTO updatePostDTO)
        {

            var postToUpdate = await _context.Posts.FindAsync(updatePostDTO.PostId);

            postToUpdate.Content = updatePostDTO.Content;
            postToUpdate.CategoryId = updatePostDTO.PostCategory;
            _context.Posts.Update(postToUpdate);
            if (updatePostDTO.PostPictures is not null && updatePostDTO.PostPictures.Count() > 0)
            {
                string imagePath = "";
                var attachments = await _context.Attachments.Where(x => x.PostId == updatePostDTO.PostId).ToListAsync();
                if (attachments.Count() > 0)
                {
                    foreach (var picture in updatePostDTO.PostPictures)
                    {
                        imagePath = _imageService.UploadImage(picture);
                        attachments[0].AttachmentPath = imagePath;
                    }
                    _context.Attachments.UpdateRange(attachments);
                }
                else
                {
                    string newImagePath = "";
                    List<Attachment> attachment = new List<Attachment>();
                    foreach (var picture in updatePostDTO.PostPictures)
                    {
                        newImagePath = _imageService.UploadImage(picture);
                        var image = new Attachment()
                        {
                            AttachmentPath = newImagePath,
                            PostId = updatePostDTO.PostId,

                        };
                        attachment.Add(image);
                    }
                    _context.Attachments.AddRange(attachment);
                }
               
            }
            var userInfo = await _context.Users.FindAsync(postToUpdate.UserId);
            var email = new EmailDTO()
            {
                Receiver = userInfo.Email,
                Title = "Add New Post",
                Body = $"Dear {userInfo.FirstName} {userInfo.LastName}\n\nWe are happy to inform you that your post updated successfully.\n\nRegards,"
            };
            await _emailService.SendEmailAsync(email);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AddPost")]

        public async Task<ActionResult<Post>> AddPost([FromForm] AddPostDTO post)
        {
            var newPost = new Post()
            {
                UserId = post.UserId,
                CategoryId = post.PostCategory,
                Content = post.Content
            };
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            if (post.PostPictures.Count > 0)
            {
                string imagePath = "";
                List<Attachment> attachments = new List<Attachment>();
                foreach (var picture in post.PostPictures)
                {
                    imagePath = _imageService.UploadImage(picture);
                    var image = new Attachment()
                    {
                        AttachmentPath = imagePath,
                        PostId = newPost.PostId,

                    };
                    attachments.Add(image);
                }
                _context.Attachments.AddRange(attachments);
                await _context.SaveChangesAsync();

            }
            var userInfo = await _context.Users.FindAsync(post.UserId);
            var email = new EmailDTO()
            {
                Receiver = userInfo.Email,
                Title = "Add New Post",
                Body = $"Dear {userInfo.FirstName} {userInfo.LastName}\n\nWe are happy to inform you that your post added successfully.\n\nRegards,"
            };
            await _emailService.SendEmailAsync(email);
            return Ok();
        }

        [HttpDelete("DeletePost/{id}")]
        public async Task<IActionResult> DeletePost([FromRoute]int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(pl => pl.PostLikes)
             .Include(p => p.Attachments)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.User)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.CommentLikes)
             .Include(p => p.Comments)
                 .ThenInclude(c => c.Attachments)
                 .FirstOrDefaultAsync(u => u.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            _context.PostLikes.RemoveRange(post.PostLikes);
            _context.Comments.RemoveRange(post.Comments);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("getallImage")]
        public async Task<List<string>> GetAllPostImages([FromBody] List<int> ids)
        {
            var images = await _context.Attachments.Where(x => ids.Contains(x.PostId.Value)).Select(p=>p.AttachmentPath).ToListAsync();

            return images;

        }

        [HttpGet("postCount")]
        public async Task<int> PostCount()
        {
            return await _context.Posts.CountAsync();
        }

    }
}
