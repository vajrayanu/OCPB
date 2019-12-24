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
    public class UpDateCaseController : BaseServiceController
    {
        // GET api/updatecase


        // GET api/updatecase/5
        [Route("api/UpDateCase")]
        public void Get(string tokenId, string Case_Id, string Status_Date, string Status_Detail, bool Clase_Status)
        {
            _UpDateCase(tokenId, Case_Id, Status_Date, Status_Detail, Clase_Status);
        }

        // POST api/updatecase
        [Route("api/UpDateCase")]
        public void Post([FromBody]_Case value)
        {
            _UpDateCase(value.tokenId, value.Case_Id, value.Status_Date, value.Status_Detail, value.Clase_Status);
        }

        public class _Case
        {
            public string tokenId { get; set; } public string Case_Id { get; set; } public string Status_Date { get; set; } public string Status_Detail { get; set; }public bool Clase_Status { get; set; }
        }

        public void _UpDateCase(string tokenId, string Case_Id, string Status_Date, string Status_Detail, bool Clase_Status)
        {

            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");

            if (string.IsNullOrEmpty(Case_Id))
                result = falseresult("Please provide Case Id.");


            //string key = Encryption.Decrypt(tokenId);

            //if (!IsValidateToken(key))
            //    result = falseresult("UnAuthorized.");
            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");

            // Department_ExMapDao _departMap = new Department_ExMapDao();
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            ComplainsMapDao _Map = new ComplainsMapDao();
            ComplianTrackingMapDao _track = new ComplianTrackingMapDao();

            try
            {
                //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                //if (objToken != null)
                //{
                    // var objDepart = _departMap.FindByKeygen(objToken.ApiKey).FirstOrDefault();
                    var comp = _Map.FindByComplain_Code_ID(Case_Id).FirstOrDefault();
                    if (comp != null)
                    {
                        var obj = _track.FindByComplain_CodeWithRefKeygen(comp.ID, TokenValid.ApiKey);
                        if (obj != null)
                        {

                            ComplainTrackExLog.AddComplainTrackExLog(obj.ID, Status_Date, Status_Detail, Clase_Status, TokenValid.ApiKey);


                            //update ComplainTracking
                            if (Clase_Status)
                            {
                                //ถ้ามีการ close job ให้ update tracking ใน isclosejob,isclosedate ด้วย
                                obj.IsCloseJob = Clase_Status;
                                obj.IsCloseDate = DateTime.Now;
                                _track.AddOrUpdate(obj);
                                _track.CommitChange();
                            }


                            SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Update Case", SaveUtility.TransStatus.Create, TokenValid.Keygen,IPAddress, "s"); //s: service
                            result = Trueresult("OK");
                        }
                        else
                        {
                            result = falseresult("Data not found.");
                        }
                    }
                    else
                    {
                        result = falseresult("Data not found.");
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

                _Map = null;
                _track = null;
                // _trackEx = null;
                _authenMap = null;

            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }
    }
}
