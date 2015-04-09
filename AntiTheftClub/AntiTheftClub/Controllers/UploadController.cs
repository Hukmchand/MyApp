using AntiTheftClub.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace AntiTheftClub.Controllers
{
    public class UploadController : ApiController
    {
        // GET api/upload
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/upload/5
        public string Get(int id)
        {
            return "value";
        }

         [ResponseType(typeof(ResponseModel))]
        public async Task<IHttpActionResult> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                StringBuilder sb = new StringBuilder(); // Holds the response body
                
                // Read the form data and return an async task.
                await Request.Content.ReadAsMultipartAsync(provider);
                var result =   this.Request.GetQueryNameValuePairs();
                // This illustrates how to get the form data.
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        sb.Append(string.Format("{0}: {1}\n", key, val));
                    }
                }

                ResponseModel model = new ResponseModel();
                model.IsSuccess = true;
                // This illustrates how to get the file names for uploaded files.
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                    model.Message = fileInfo.Name;
                }
                
                //return new HttpResponseMessage()
                //{
                //    Content = new StringContent(sb.ToString())
                //};

                return this.Ok<ResponseModel>(model);
                //  return this.Ok<bool>(true);
            }
            catch (System.Exception e)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.InternalServerError,e));
            }
        }

        // PUT api/upload/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/upload/5
        public void Delete(int id)
        {
        }
    }
}
