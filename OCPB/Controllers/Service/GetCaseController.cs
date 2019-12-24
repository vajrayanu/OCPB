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
    public class GetCaseController : BaseServiceController
    {
        //// GET api/getcase
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/getcase/5
        [Route("api/GetCase")]
        public void Get(string tokenId, string Case_Id)
        {
            _GetCase(tokenId, Case_Id);
        }

        // POST api/getcase
        [Route("api/GetCase")]
        public void Post([FromBody]_Case value)
        {
            _GetCase(value.tokenId, value.Case_Id);
        }
        public class _Case
        {
            public string tokenId { get; set; }
            public string Case_Id { get; set; }
        }

        public void _GetCase(string tokenId, string Case_Id)
        {
            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");

            if (string.IsNullOrEmpty(Case_Id))
                result = falseresult("Please provide Case Id.");

            //if (!IsValidateToken(key))
            //    result = falseresult("UnAuthorized.");
            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");
            Department_ExMapDao _departMap = new Department_ExMapDao();
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            ComplainsMapDao _Map = new ComplainsMapDao();
            try
            {
                //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                //if (objToken != null)
                //{
                var objDepart = _departMap.FindByKeygen(TokenValid.ApiKey).FirstOrDefault();

                //********************* Get Complain list*********************
                //CaseID= Complain_Code_id                  
                var comList = ComplainData.GetComplainByCaseId(TokenValid.Keygen, Case_Id).ToList();

                foreach (var i in comList)
                {
                    i.AttachmentFiles = GetfileUpload(i.ID);
                }

                if (comList.Count > 0)
                {
                     

                    // var Status = GetComplain.Get_log_all(obj.Complain_Code_ID, null, null, null, null, null, null, null);

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
                                       // Complain_Status_text = c.Complain_Status_text,
                                       Complain_Status_text = Liststatus(c.ID),
                                       AttachmentFiles = c.AttachmentFiles,
                                   };

                    SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Get Case", SaveUtility.TransStatus.Create, TokenValid.ApiKey, IPAddress, "s"); //s: service
                    result = Trueresult(selected);
                }
                else
                {
                    SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Get Case", SaveUtility.TransStatus.Create, TokenValid.ApiKey, IPAddress, "s"); //s: service
                                                                                                                                                      // result = Trueresult(new OCPB.Model.Complains());
                    result = _Noresult();
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
                _departMap = null;
                _authenMap = null;
            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }
        public List<Statuslist> Liststatus(int ID)
        {
            List<Statuslist> Obj = new List<Statuslist>();
            foreach (var Items in OCPB.GetComplain.Get_log_History(ID))
            {
                Statuslist _i = new Statuslist();
                _i.DepartName = Items.title;
                _i.StatusDetailObj = new List<StatusDetails>();
                foreach (var SubItems in GetComplain.Get_log_History_Detail(Items.Complain_Tracking_ID.Value))
                {
                    StatusDetails _s = new StatusDetails();
                    _s.Amount = SubItems.Amount.ToString();
                    _s.Comment = SubItems.Reason_Comment;
                    _s.StatusTH = SubItems.StatusTH;
                    _s.IsCloseFlage = SubItems.IsCloseFlage;
                    _s.Date = SubItems.Send_Date_str;
                    //_s.IsCloseFlage = SubItems. /// เพิ่ม IsCloseFlage ในสถานะส่งต่อ
                    _i.StatusDetailObj.Add(_s);
                }
                Obj.Add(_i);
            }
            return Obj;
        }

        public class Statuslist
        {
            public string DepartName { get; set; }
            public List<StatusDetails> StatusDetailObj { get; set; }
        }
        public class StatusDetails
        {
            public string Date { get; set; }
            public string Comment { get; set; }
            public string StatusTH { get; set; }
            public string Amount { get; set; }
            public bool? IsCloseFlage { get; set; }
        }

        public static List<OCPB.Service.Model.ComplainGetFileModel> GetfileUpload(int ComplainID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ComplainID", ComplainID);


            return DirectSqlDao.GetAllStored<OCPB.Service.Model.ComplainGetFileModel>("GetComplainFileUpload", param);
        }
    }
}
