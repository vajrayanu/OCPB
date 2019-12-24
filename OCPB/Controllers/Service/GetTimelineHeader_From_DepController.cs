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
    public class GetTimelineHeader_From_DepController : BaseServiceController
    {
        // GET api/gettimelineheader_from_dep
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/gettimelineheader_from_dep/5
        [Route("api/GetTimelineHeader_From_Dep")]
        public void Get(string tokenId, int skip, int take)
        {
            _GetTimelineHeader_From_Dep( tokenId, skip, take);
        }

        // POST api/gettimelineheader_from_dep
        [Route("api/GetTimelineHeader_From_Dep")]
        public void Post([FromBody]_TimelineHeader value)
        {
            _GetTimelineHeader_From_Dep(value.tokenId, value.skip, value.take);
        }
        public class _TimelineHeader
        {
            public string tokenId { get; set; }
            public int skip { get; set; }
            public int take { get; set; }
        }
         
        public void _GetTimelineHeader_From_Dep(string tokenId, int skip, int take)
        {
            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");
            //string key = Encryption.Decrypt(tokenId);
            //if (!IsValidateToken(key))
            //    result = falseresult("UnAuthorized.");
            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");


            if (skip < 1)
                skip = 1;

            if (take < 1)
                take = 20;

            try
            {
                AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();

                //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();



                Department_ExMapDao _departMap = new Department_ExMapDao();

                //if (objToken != null)
                //{
                  
                    var Dep_ex = _departMap.FindByKeygen(TokenValid.ApiKey).FirstOrDefault();

                    var comList = ComplainData.GetTimelineHeader_From_Dep(Dep_ex.ID);
                    SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Get Time line Header", SaveUtility.TransStatus.Create, TokenValid.Keygen,IPAddress, "s"); //s: service
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

        // PUT api/gettimelineheader_from_dep/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/gettimelineheader_from_dep/5
        //public void Delete(int id)
        //{
        //}
    }
}
