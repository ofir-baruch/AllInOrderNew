using AllinOrder_Shahaf_Ofir_Snir.Server.Data;
using AllinOrder_Shahaf_Ofir_Snir.Server.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllinOrder_Shahaf_Ofir_Snir.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly FileStorage _fileStorage;

        public PhotosController(DataContext context, FileStorage fileStorage)
        {
            _context = context;
           _fileStorage = fileStorage;
        }
        
        //העלאת תמונה
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromBody] string imageBase64)
        {
            byte[] picture = Convert.FromBase64String(imageBase64);
            string url = await _fileStorage.SaveFile(picture, "png", "uploadedFiles");
            return Ok(url);
        }

        //מחיקת תמונה
        [HttpPost("deleteImages")]
        public async Task<IActionResult> DeleteImages([FromBody] List<string> images)
        {
            foreach (string img in images)
            {
                await _fileStorage.DeleteFile(img, "uploadedFiles");
            }
            return Ok("deleted");
        }

    }
}
