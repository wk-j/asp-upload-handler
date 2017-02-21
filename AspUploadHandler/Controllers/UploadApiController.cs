using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Primitives;
using System.Linq;

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
        public async Task<string> Hello()
        {
            return await Task.Run(() => "Hello, world!");
        }

        private (bool, string) GetJsonString(HttpRequest request)
        {
            StringValues value = "";
            var success = request.Form.TryGetValue("json", out value);
            if (success)
            {
                var json = String.Join("", value);
                return (true, json);
            }
            return (false, string.Empty);
        }

        [HttpPost]
        public async Task<object> UploadFileAndMetadata()
        {
            Console.WriteLine("Upload file and metadata ...");

            var request = this.Request;
            var files = request.Form.Files;
            var (success, json) = GetJsonString(request);
            if (success)
            {
                Console.Write(json);
            }

            await Task.Run(() =>
            {
                var tasks = files.ToList().Select(async file =>
                {
                    var guid = Guid.NewGuid().ToString("N");
                    var target = Path.Combine(_environment.WebRootPath, "uploads", $"{guid}-{file.FileName}");
                    using (var stream = new FileStream(target, FileMode.Create, FileAccess.Write))
                    {
                        await file.CopyToAsync(stream);
                    }
                });
                Task.WaitAll(tasks.ToArray());
            });

            return new
            {
                Success = true
            };
        }

        [HttpPost]
        public async Task<object> UploadFile([FromForm] ICollection<IFormFile> files)
        {
            Console.WriteLine("Upload file ...");
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            return new
            {
                Success = true
            };
        }
    }
}