using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ConcertTickets.Data
{
    public static class ImageService
    {
        public static async Task<string?> SaveResizedImageAsync(IFormFile? file, string webRootPath)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            string folder = Path.Combine(webRootPath, "images", "concerts");
            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(folder, fileName);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                if (image.Width > 1024)
                {
                    image.Mutate(x => x.Resize(1024, 0));
                }
                await image.SaveAsync(fullPath);
            }

            return "/images/concerts/" + fileName;
        }

        public static void DeleteImage(string? imagePath, string webRootPath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return;
            }

            if (string.IsNullOrEmpty(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            string fullPath = Path.Combine(webRootPath, imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}