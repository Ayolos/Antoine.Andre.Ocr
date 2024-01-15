using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Antoine.Andre.Ocr.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class OcrController : ControllerBase
    {
        private readonly Ocr.Ocr _ocr;

        public OcrController(Ocr.Ocr ocr)
        {
            _ocr = ocr;
        }

        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync([FromForm(Name = "files")] IList<IFormFile> files)
        {
            var images = new List<byte[]>();

            foreach (var formFile in files)
            {
                using var sourceStream = formFile.OpenReadStream();
                using var memoryStream = new MemoryStream();
                sourceStream.CopyTo(memoryStream);
                images.Add(memoryStream.ToArray());
            }

            try
            {
                // Process the images using OCR
                var ocrResults = _ocr.Read(images);

                return Ok(ocrResults);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erreur lors du traitement des images : {ex.Message}");
            }
        }
    }
}