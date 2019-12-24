using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using OCPB.Models;
using System.Web.Services;
using OCPB.Model;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;


namespace OCPB.Controllers.Service
{
    public class SaveFileUploadController : BaseServiceController
    {
        [HttpPost]
        [Route("api/SaveFileUpload")]
        public void Post([FromUri]SavefileModel Value )
        {
             
            if (string.IsNullOrEmpty(Value.tokenId))
                result = falseresult("UnAuthorized.");
            if (!IsValidateToken(Value.tokenId))
                result = falseresult("UnAuthorized.");
            //ComplainsMapDao ComMApDao = new ComplainsMapDao();
            //var CompTemp = ComMApDao.FindByComplain_Code_ID(Value.Complain_Code).FirstOrDefault();

            //var Obj = Value.File.fileupload(CompTemp.Keygen, Value.Description, Value.Filename, Value.TypeFile);


            //Complains_FileUploadMapDao FileMap = new Complains_FileUploadMapDao();
            //Complains_FileUpload Fileobj = new Complains_FileUpload();
            //Fileobj.ComplainID = CompTemp.ID;
            //Fileobj.TypeAtID = 11;
            //Fileobj.Description = Value.Description;
            //Fileobj.Qty = 1;
            //Fileobj.Keygen = Guid.NewGuid().ToString();
            //Fileobj.Active = true;
            //Fileobj.CreateDate = DateTime.Now;
            //FileMap.Add(Fileobj);
            //FileMap.CommitChange();
            //foreach (var items in Obj)
            //{
            //    AttachFileMapDao AttachMap = new AttachFileMapDao();
            //    AttachFile _fileObj = new AttachFile();
            //    _fileObj.GUID = Fileobj.Keygen;
            //    _fileObj.path = items.path;
            //    _fileObj.Folder = items.folder;
            //    //_fileObj.Title = File.file_name;
            //    _fileObj.FileName = items.file_name;
            //    _fileObj.FileType = items.file_Type;
            //    _fileObj.CreateDate = DateTime.Now;
            //    _fileObj.Active = true;
            //    AttachMap.Add(_fileObj);
            //    AttachMap.CommitChange();
            //} 

            try
            { 
                var queryString = this.Request.GetQueryNameValuePairs();
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        HttpPostedFile postedFile = httpRequest.Files[file];
                        ComplainsMapDao ComMApDao = new ComplainsMapDao();
                        var CompTemp = ComMApDao.FindByComplain_Code_ID(Value.Complain_Code).FirstOrDefault();
                        var Obj = postedFile.fileupload(CompTemp.Keygen, Value.Description);
                        Complains_FileUploadMapDao FileMap = new Complains_FileUploadMapDao();
                        Complains_FileUpload Fileobj = new Complains_FileUpload();
                        Fileobj.ComplainID = CompTemp.ID;
                        Fileobj.TypeAtID = 11;
                        Fileobj.Description = Value.Description;
                        Fileobj.Qty = 1;
                        Fileobj.Keygen = Guid.NewGuid().ToString();
                        Fileobj.Active = true;
                        Fileobj.CreateDate = DateTime.Now;
                        FileMap.Add(Fileobj);
                        FileMap.CommitChange();
                        foreach (var items in Obj)
                        {
                            AttachFileMapDao AttachMap = new AttachFileMapDao();
                            AttachFile _fileObj = new AttachFile();
                            _fileObj.GUID = Fileobj.Keygen;
                            _fileObj.path = items.path;
                            _fileObj.Folder = items.folder;
                            //_fileObj.Title = File.file_name;
                            _fileObj.FileName = items.file_name;
                            _fileObj.FileType = items.file_Type;
                            _fileObj.CreateDate = DateTime.Now;
                            _fileObj.Active = true;
                            AttachMap.Add(_fileObj);
                            AttachMap.CommitChange();
                        }
                    }
                }
            }
            catch (IOException)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

             
            HttpResponseMessage response = new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Created;
            //return response;
            result = Trueresult(response);
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }

        public class SavefileModel
        {
            public virtual string tokenId { get; set; }
            public virtual string Complain_Code { get; set; }
            public virtual string Description { get; set; }
            public virtual string Filename { get; set; }
            public virtual string TypeFile { get; set; }
            //public virtual byte[] File { get; set; }
            //public virtual string FullFule { get; set; }
        }
    }
}
