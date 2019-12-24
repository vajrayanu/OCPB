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
    public class GetTimelineHeaderController : BaseServiceController
    {
        // GET api/gettimelineheader/5
        [Route("api/GetTimelineHeader")]
        public void Get(string tokenId, int skip, int take, string IsCloseJob)
        {
            _GetTimelineHeader(tokenId, skip, take, IsCloseJob);
        }
        // POST api/gettimelineheader
        [Route("api/GetTimelineHeader")]
        public void Post([FromBody]_TimelineHeader value)
        {
            _GetTimelineHeader(value.tokenId, value.skip, value.take, value.IsCloseJob);
        }
        public class _TimelineHeader
        {
            public string tokenId { get; set; } public int skip { get; set; } public int take { get; set; } public string IsCloseJob { get; set; }
        }
        protected void _GetTimelineHeader(string tokenId, int skip, int take, string IsCloseJob)
        {
            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");
            //string key = Encryption.Decrypt(tokenId);
            //if (!IsValidateToken(key))
            //    result = falseresult("UnAuthorized."); 

            if (skip < 1)
                skip = 1; 
            if (take < 1)
                take = 20; 
            try
            {
                AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
                // var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                //if (objToken != null)
                //{
                    var comList = ComplainData.GetComplainTimeLine(TokenValid.ApiKey, skip, take, 0,null);
                    SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Get Time line Header", SaveUtility.TransStatus.Create, TokenValid.Keygen, IPAddress, "s"); //s: service
                    result = Trueresult(comList);
                //}
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

            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }


    

        // PUT api/gettimelineheader/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/gettimelineheader/5
        public void Delete(int id)
        {
        }
    }
}
