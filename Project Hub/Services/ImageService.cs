namespace Project_Hub.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly List<string> _allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".bmp" };
        private readonly long _maxFileSize = 100 * 1024 * 1024;

        public ImageService(IWebHostEnvironment environment)
        {

            _environment = environment;
        }
        public string UploadImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new Exception("No image file provided");
            }

            if (image.Length > _maxFileSize)
            {
                throw new Exception("File size exceeds the 20 MB limit");
            }

            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Invalid file type. Only JPG, JPEG, PNG, GIF, and BMP files are allowed.");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return $"https://localhost:7291/images/{uniqueFileName}";
        }

    }
}
