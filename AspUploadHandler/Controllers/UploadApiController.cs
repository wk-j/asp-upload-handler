using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace AspUploadHandler.Controllers 
{
    public class UploadApiController : Controller
    {
        private IHostingEnvironment _environment;

        public UploadApiController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        [Route("api/uploadApi/hello")]
        public async Task<string> Hello() {
            return await Task.Run(() => "Hello, world!");
        }

        [HttpPost]
        [Route("api/uploadApi/upload")]
        public async Task<string> Upload(ICollection<IFormFile> files)
        {
            Console.WriteLine("Upload ...");
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                var json = file.ContentDisposition;
                Console.Write(json);

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return "Hello, world!";
        }
    }
}