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

namespace OCPB.Controllers.Service
{
    public class GetFileController : BaseServiceController
    {
        // GET api/getfile
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/getfile/5
        [Route("api/GetFile")]
        public void Get(string tokenId, int ID, string GUID)
        {
            GetFile(tokenId, ID, GUID);
        }

        // POST api/getfile
        [Route("api/GetFile")]
        public void Post([FromBody]_File value)
        {
            GetFile(value.tokenId, value.ID, value.GUID);
        }

        public class _File
        {
            public string tokenId { get; set; } public int ID { get; set; } public string GUID { get; set; }
        }
        public void GetFile(string tokenId, int ID, string GUID)
        {
            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");
            //string key = Encryption.Decrypt(tokenId);
            //if (!IsValidateToken(key))
            //    result = falseresult("UnAuthorized.");
            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");
            try
            {
                AttachFileMapDao _map = new AttachFileMapDao();
                var _obj = _map.FindById(ID);
                if (_obj != null)
                {
                    if (_obj.GUID.Trim() == GUID.Trim())
                    {
                        var obj = _map.FindById(ID);
                        string Filename = null;
                        byte[] fileBytes = GetUtility.LoadFile(ID, ref Filename);
                        string fileName = obj.FileName;
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ContentType = "application/force-download";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;    filename=" + fileName);
                        HttpContext.Current.Response.BinaryWrite(fileBytes);
                        HttpContext.Current.Response.End();
                    }
                }
                else
                {
                    result = falseresult("No file...");
                    HttpContext.Current.Response.ContentType = "application/json";
                    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
                result = falseresult(ex.Message);
                HttpContext.Current.Response.ContentType = "application/json";
                HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
                HttpContext.Current.Response.End();
            }
        }

        //// PUT api/getfile/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/getfile/5
        //public void Delete(int id)
        //{
        //}
    }
}
