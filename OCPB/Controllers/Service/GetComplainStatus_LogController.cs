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
    public class GetComplainStatus_LogController : BaseServiceController
    {
        // GET api/getcomplainstatus_log
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/getcomplainstatus_log/5

        [Route("api/GetComplain")]
        public void Get(string tokenId, string Case_Id)
        {
            GetComplainStatus_Log(tokenId, Case_Id);
        }

        // POST api/getcomplainstatus_log
        [Route("api/GetComplain")]
        public void Post([FromBody]Complain value )
        {
            GetComplainStatus_Log(value.tokenId, value.Case_Id);
        }
        public class Complain {
            public string tokenId { get; set; }
            public string Case_Id { get; set; }
        }

        public void GetComplainStatus_Log(string tokenId, string Case_Id)
        {

            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");


            if (string.IsNullOrEmpty(Case_Id))
                result = falseresult("Please provide Case Id.");


            ////string key = Encryption.Decrypt(tokenId);

            ////if (!IsValidateToken(key))
            ////    result = falseresult("UnAuthorized.");

            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");

            // Department_ExMapDao _departMap = new Department_ExMapDao();
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            //ComplainsMapDao _Map = new ComplainsMapDao();

            try
            {


                //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                //if (objToken != null)
                //{
                    //  var objDepart = _departMap.FindByKeygen(objToken.ApiKey).FirstOrDefault();

                    //********************* Get Complain list*********************
                    //CaseID= Complain_Code_id                  
                    var comList = ComplainData.GetComplainLogByCaseId(TokenValid.ApiKey, Case_Id);

                    if (comList.Count > 0)
                    {
                        SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Get ComplainStatus_Log", SaveUtility.TransStatus.Create, TokenValid.Keygen, IPAddress, "s"); //s: service
                        result = Trueresult(comList);
                    }
                    else
                    {
                        SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Get ComplainStatus_Log", SaveUtility.TransStatus.Create, TokenValid.Keygen, IPAddress, "s"); //s: service
                        result = falseresult(new List<OCPB.Service.Model.ComplainLog> { new OCPB.Service.Model.ComplainLog() });
                    }

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
            finally
            {

                //_Map = null;
                //_departMap = null;
                _authenMap = null;
            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();

        }

        //// PUT api/getcomplainstatus_log/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/getcomplainstatus_log/5
        //public void Delete(int id)
        //{
        //}
    }
}
