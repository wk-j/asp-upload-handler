using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xunit;
using System.IO;
using System;
using System.Net;

namespace AspUploadHandler.Tests
{
    public class UploadSpec
    {
        [Fact]
        public void ShouldUploadFile()
        {
            var values = new {
                A = 100,
                B = 200
            };

            using (var formDataContent = new MultipartFormDataContent()) 
            {
                var content = File.ReadAllBytes("/Users/wk/Source/github/asp-upload-handler/AspUploadHandler.sln");

                formDataContent.Add(new ByteArrayContent(content), "files", "Hello.pdf");
                formDataContent.Add(new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json"), "MyJsonObject");

                var api = "http://localhost:5000/api/uploadApi/upload";
                using (HttpClient httpClient = new HttpClient())
                {
                     var rs = httpClient.PostAsync(api, formDataContent).Result;
                     Console.WriteLine(rs.Content.ReadAsStringAsync().Result);
                     Console.WriteLine(rs.StatusCode == HttpStatusCode.OK);
                }
            }
        }
    }
}
