using Newtonsoft.Json;
using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OCPB.Controllers.MobileService
{
    public class MComplainController : BaseServiceController
    {
        // GET api/mcomplain
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/mcomplain/5
        [Route("api/MComplain")]
        public void Get(string ID, string UserKeys, string Case_id)
        {
            result = falseresult("ข้อมูลไม่ถูกต้อง");
            try
            {
                CustomerMapDao map = new CustomerMapDao();
                    
                int _id = MobileEncryption.Decrypt(ID.UrlDescriptHttp()).Toint();  
                string Key = MobileEncryption.Decrypt(UserKeys.UrlDescriptHttp());  

                var Obj = map.FindByActive().Where(o => o.ID == _id && o.Keygen == Key).FirstOrDefault();
                if (Obj != null)
                { 
                    var comList = ComplainData.GetComplainByCaseId(null, Case_id); 
                    foreach (var i in comList)
                    {
                        i.AttachmentFiles = OCPB.Controllers.Service.GetCaseController.GetfileUpload(i.ID);
                    } 
                    if (comList.Count > 0)
                    {
                        var selected = from c in comList
                                       select new
                                       {
                                           Complain_Code_ID = c.Complain_Code_ID,
                                           Complain_Date = c.Complain_Date,
                                           Complain_Time = c.Complain_Time,
                                           Complain_Subject = c.Complain_Subject,
                                           Complain_Details = c.Complain_Details,
                                           Complain_Channel_id = c.Complain_Channel_id,
                                           Complain_Channel_Text = c.Complain_Channel_Text,
                                           Consumer_Citizen_id = c.CusIden,
                                           Consumer_Name = c.Cusname,
                                           Case_id = c.Complain_Cause_id,
                                           Defendent_Name = c.CompanyName,
                                           Defendent_Detail = c.Complain_Details,
                                           PaymentID = c.PaymentID,
                                           PaymentText = c.PaymentText,
                                           Complain_TypeID = c.Complain_TypeID,
                                           Complain_Type_Text = c.Complain_Type_Text,
                                           Complain_Type_Sub_ID = c.Complain_Type_Sub_ID,
                                           Complain_Type_Sub_Text = c.Complain_Type_Sub_Text,
                                           Complain_Cause_ID = c.Complain_Cause_id,
                                           Complain_Cause_Text = c.Complain_Cause_Text,
                                           PlacePurchaseID = c.PlacePurchaseID,
                                           PlacePurchase_Text = c.PlacePurchase_Text,
                                           MotiveID = c.MotiveID,
                                           Motive_Text = c.Motive_Text,
                                           Complain_Status_text = c.Complain_Status_text,
                                           AttachmentFiles = c.AttachmentFiles
                                       };
                        result = Trueresult(selected);
                    }
                }
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
        [Route("api/MComplain")]
        public void Get(string ID, string UserKeys)
        {
            result = falseresult("ข้อมูลไม่ถูกต้อง");
            try
            {
                CustomerMapDao map = new CustomerMapDao();
                int _id = MobileEncryption.Decrypt(ID.UrlDescriptHttp()).Toint();
                string Key = MobileEncryption.Decrypt(UserKeys.UrlDescriptHttp());  
                var Obj = map.FindByActive().Where(o => o.ID == _id && o.Keygen == Key).FirstOrDefault();
                if (Obj != null)
                {
                    result = Trueresult(ComplainData.GetComplainTimeLine(null, null, null, null, Obj.ID));
                }
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
        // POST api/mcomplain
         [HttpPost()]
        [Route("api/MComplain")]
        public void Post([FromUri]OCPB.Controllers.Service.NewComplainController._AddComplainModel value)
        {
            OCPB.Controllers.Service.NewComplainController Meth = new Service.NewComplainController();
             Meth._AddComplain(value.tokenId, value.Identification_number, value.Consumer_Title, value.Consumer_firstname, value.Consumer_lastname, value.Consumer_gender, value.Consumer_Birth
, value.Consumer_Address,null,null,null, value.Consumer_ZipCode, value.Consumer_Tel, value.Consumer_Tel_Ex, value.Consumer_Mobile, value.Consumer_Fax, value.Consumer_Email, value.Complain_Subject
, value.Complain_Details, value.DefendentName, value.DefendentDescription, value.TYPE_0, value.TYPE_1, value.CASEID, value.PurchaseID, value.PaymentID, value.MotiveID, value.IsOversea, value.OverseaAddress);
        }
         
 
        // PUT api/mcomplain/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/mcomplain/5
        public void Delete(string ID, string UserKeys, string Description, string Case_id, int CancelID)
        {
            result = falseresult("ข้อมูลไม่ถูกต้อง");
            try
            {
                CustomerMapDao CusMap = new CustomerMapDao();
                int _id = MobileEncryption.Decrypt(ID.UrlDescriptHttp()).Toint();
                string Key = MobileEncryption.Decrypt(UserKeys.UrlDescriptHttp());
                var Cus = CusMap.FindByActive().Where(o => o.ID == _id && o.Keygen == Key).FirstOrDefault();
                if (Cus != null)
                {
                    ComplainsMapDao Map = new ComplainsMapDao();
                    var CompObj = Map.FindByCustomerID(Cus.ID).Where(o => o.Complain_Code_ID == Case_id).FirstOrDefault();
                    if (CompObj != null)
                    {
                        SaveComplain.Cancel(CompObj.Keygen, Description, CancelID);
                        result = Trueresult("ยกเลิกข้อมูลสำเร็จ");
                    }
                    else
                    {
                        result = falseresult("ไม่พบข้อมูล");
                    }
                }
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
    }
}
