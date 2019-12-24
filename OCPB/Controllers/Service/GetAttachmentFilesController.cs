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
    public class GetAttachmentFilesController : BaseServiceController
    {
        // GET api/getattachmentfiles
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/getattachmentfiles/5
        [Route("api/GetAttachmentFiles")]
        public void Get(string tokenId, string Case_Id)
        {
            GetAttachmentFiles(tokenId, Case_Id);
        }
        // POST api/getattachmentfiles
        [Route("api/GetAttachmentFiles")]
        public void Post([FromBody]_Attachment value)
        {
            GetAttachmentFiles(value.tokenId, value.Case_Id);
        }

        public class _Attachment
        {
            public string tokenId { get; set; }
            public string Case_Id { get; set; }
        }
         public void GetAttachmentFiles(string tokenId, string Case_Id)
        {
            //if (string.IsNullOrEmpty(tokenId))
            //    result = falseresult("UnAuthorized.");

            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized."); 
            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");

            if (string.IsNullOrEmpty(Case_Id))
                result = falseresult("Please provide Case Id."); 
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            ComplainsMapDao _Map = new ComplainsMapDao();
            try
            {
                //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                //if (objToken != null)
                //{
                    //********************* Get Complain list*********************
                    //CaseID= Complain_Cause_id                  
                    var comList = ComplainData.GetfileUploadByCaseId(TokenValid.ApiKey, Case_Id).Select(o => new
                    {
                        o.Description,
                        o.Complain_file_ID,
                        o.FileName,
                        o.FileType,
                        o.CreateDate,
                        o.GUID
                    }).ToList();
                    SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Get Attachment Files", SaveUtility.TransStatus.Create, TokenValid.ApiKey,IPAddress, "s"); //s: service
                     result = Trueresult(comList);
                // }
                //else
                //{
                //    result = falseresult("UnAuthorized.");
                //}
            }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
                result = falseresult(ex.Message);
            }
            finally
            {
                _Map = null;
                _authenMap = null;
             }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
         }

        // DELETE api/getattachmentfiles/5
        //public void Delete(int id)
        //{
        //}
    }
}
