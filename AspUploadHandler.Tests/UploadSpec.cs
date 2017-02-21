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
        public void ShouldUploadFileAndMetadata() {
            var values = new {
                A = 100,
                B = 200
            };
            using (var formDataContent = new MultipartFormDataContent()) 
            {
                var content = File.ReadAllBytes("AspUploadHandler.sln");

                formDataContent.Add(new ByteArrayContent(content), "files", "Hello.pdf");
                formDataContent.Add(new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json"), "json");

                var api = "http://localhost:5000/api/uploadApi/uploadFileAndMetadata";
                using (HttpClient httpClient = new HttpClient())
                {
                     var rs = httpClient.PostAsync(api, formDataContent).Result;
                     Console.WriteLine(rs.Content.ReadAsStringAsync().Result);
                     Assert.True(rs.StatusCode == HttpStatusCode.OK);
                }
            }
        }

        [Fact]
        public void ShouldUploadFile()
        {
            var values = new {
                A = 100,
                B = 200
            };

            using (var formDataContent = new MultipartFormDataContent()) 
            {
                var content = File.ReadAllBytes("AspUploadHandler.sln");
                var guid = Guid.NewGuid().ToString("N");

                formDataContent.Add(new ByteArrayContent(content), "files", $"{guid}-Hello.sln");
                formDataContent.Add(new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "application/json"), "MyJsonObject");

                var api = "http://localhost:5000/api/uploadApi/uploadFile";
                using (HttpClient httpClient = new HttpClient())
                {
                     var rs = httpClient.PostAsync(api, formDataContent).Result;
                     Console.WriteLine(rs.Content.ReadAsStringAsync().Result);
                     Assert.True(rs.StatusCode == HttpStatusCode.OK);
                }
            }
        }
    }
}
