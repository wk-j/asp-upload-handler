using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspUploadHandler {
    public class HelloController: Controller {
        [HttpPost]
        public async Task<object> Hello() {
            await Task.Run(() => {});
            return new {
                A = "Hello"
            };
        }
    }
}