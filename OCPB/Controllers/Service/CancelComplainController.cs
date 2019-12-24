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
    public class CancelComplainController : BaseServiceController
    {
 //       [HttpGet]
 //       public void Get(string tokenId, string Identification_number, string Consumer_firstname, string Consumer_lastname, string Consumer_gender, string Consumer_Birth
 //, string Consumer_Address, string Consumer_ZipCode, string Consumer_Tel, string Consumer_Tel_Ex, string Consumer_Mobile, string Consumer_Fax, string Consumer_Email, string Complain_Subject
 //, string Complain_Details, string DefendentName, string DefendentDescription, int TYPE_0, int TYPE_1, int CASEID, int PurchaseID, int PaymentID, int MotiveID, string IsOversea, string OverseaAddress)
 //       {
 //           _AddComplain(tokenId, Identification_number, Consumer_firstname, Consumer_lastname, Consumer_gender, Consumer_Birth
 //, Consumer_Address, Consumer_ZipCode, Consumer_Tel, Consumer_Tel_Ex, Consumer_Mobile, Consumer_Fax, Consumer_Email, Complain_Subject
 //, Complain_Details, DefendentName, DefendentDescription, TYPE_0,TYPE_1,CASEID, PurchaseID,PaymentID,MotiveID, IsOversea, OverseaAddress);
 //       }
         [HttpPost()]
        [Route("api/CancelComplain")]
        public void Post([FromBody]Complain value)
        {
            _CancelStatus(value.tokenId , value.Case_Id);
        }

        public class Complain
        {
            public string tokenId { get; set; }
            public string Case_Id { get; set; }
        }


        public void _CancelStatus(string tokenId ,string Case_Id)
        {
            try
            {
                if (string.IsNullOrEmpty(tokenId))
                { result = falseresult("UnAuthorized."); }

                { result = falseresult("Please provide Identification number."); }

                //string key = Encryption.Decrypt(tokenId);

                //if (!IsValidateToken(key))
                //    result = falseresult("UnAuthorized.");
                if (!IsValidateToken(tokenId))
                { result = falseresult("UnAuthorized."); }
                else
                {

                    var comList = ComplainData.GetComplainLogByCaseId(TokenValid.ApiKey, Case_Id);

                    if (comList.Count > 0)
                    {

                        ComplainsMapDao Map = new ComplainsMapDao();

                        var Temp = Map.FindAll().Where(o=>o.Complain_Code_ID == Case_Id).FirstOrDefault();
                        if (SaveComplain.Cancel(Temp.Keygen, "", 228))
                        {
                            result = Trueresult("ยกเลิกข้อมูลสำเร็จ.");
                        }
                        else
                        {
                            result = falseresult("การยกเลิกข้อมูลมีปัญหา.");
                        }
                    }
                    else
                    {
                        result = falseresult("การยกเลิกข้อมูลมีปัญหา.");
                    }
                }
            }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
                result = falseresult(ex.Message);
            }
            //finally
            //{
            //    _mapVer = null;
            //    _Map = null;
            //    _logMap = null;
            //    _departMap = null;
            //    _authenMap = null;
            //    _cusMap = null;
            //}
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }

    }
}
