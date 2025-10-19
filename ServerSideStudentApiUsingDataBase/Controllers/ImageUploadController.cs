using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ServerSideStudentApiUsingDataBase.Controllers
{
    [Route("api/ImageUpload")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {

        [HttpPost("ImageUpload")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(IFormFile ImageFile)
        {
            if(ImageFile==null || ImageFile.Length == 0)
            {
                return BadRequest("No File Uploaded..");
            }

            var Direct = @"C:\Images";

            var FileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            var FilePath = Path.Combine (Direct, FileName);

            if (!Directory.Exists(Direct))
            {
                Directory.CreateDirectory(Direct);
            }

            using (var stream = new FileStream(FilePath,FileMode.Create))
            {
               await ImageFile.CopyToAsync(stream);
            }

            return Ok(new { FilePath });
        }


        [HttpGet("GetImage")]
        public IActionResult GetImage(string FileName)
        {
            var Direct = @"C:\Images";
            var FilePath = Path.Combine(Direct, FileName);

            if (!System.IO.File.Exists(FilePath))
            {
                return NotFound("No File Found..");
            } 

            var Image = System.IO.File.OpenRead(FilePath);
            var mimtype = GetMimeType(FilePath);

            return File(Image, mimtype);
        }

        string GetMimeType(string FilePath)
        {
            var extention = Path.GetExtension(FilePath).ToLowerInvariant();
            return extention switch
            {
                ".jpg" or ".jepg" => "Image/jpeg",
                ".png" => "Image/png",
                ".gif" => "Image/gif",
                _ => "application/octet-stream",
            };
        }
    }
}
