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

namespace OCPB.Controllers
{
    public class OfficerController : BaseServiceController
    {
        //// GET api/officer
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/officer/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/officer
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/officer/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/officer/5
        //public void Delete(int id)
        //{
        //} 
        
        //private Object result = new Object();
        //private static JsonResult _Noresult()
        //{
        //    return new JsonResult() { Result = false, Message = "No result..." };
        //}
        //private static JsonResult falseresult(Object message)
        //{
        //    return new JsonResult() { Result = false, Message = message };
        //}
        //private static JsonResult Trueresult(Object message)
        //{
        //    return new JsonResult() { Result = true, Message = message };
        //}
        //private class JsonResult
        //{
        //    public bool Result { get; set; }
        //    public Object Message { get; set; }
        //}

        #region Token
        [WebMethod(Description = "Get Token เป็นเมธอดที่ใช้กำหนดค่า Token ของ User ที่ใช้อ้างอิงในการดึงข้อมูลจากระบบ"
+ "<br />APIKey เป็นคีย์เข้าใช้งานระบบที่ได้รับจากทาง สคบ"
+ "<br />Password รหัสผ่านเข้าใช้งานระบบ")]
        //[HttpGet]
        //public void GetToken(string APIKey, string Password)
        //{
        //    string IPAddress = HttpContext.Current.Request.UserHostAddress;
        //    if (string.IsNullOrEmpty(APIKey))
        //        result = falseresult("UnAuthorized...");

        //    if (string.IsNullOrEmpty(Password))
        //        result = falseresult("Please provide Password...");

        //    if (string.IsNullOrEmpty(IPAddress))
        //        result = falseresult("Please provide IP Address...");

        //    //if (!CheckPassEnscrypt(Password, ref Password))
        //    //{ result = falseresult("UnAuthorized..."); }
        //    else
        //    {

        //        Department_ExMapDao map = new Department_ExMapDao();
        //        try
        //        {
        //            OfficerToken _token = new OfficerToken();
        //            string newToken = Guid.NewGuid().ToString();
        //            var obj = map.FindByKeygen(APIKey).Where(o => o.Password == Encryption.Encrypt(Password)).FirstOrDefault();
        //            if (obj != null)
        //            {
        //                GetAuthToken(_token, APIKey, newToken, IPAddress);
        //                //_token.Username = "";
        //                //_token.Org_id = obj.ID.ToString();
        //                _token.Org_name = obj.External_dept_Name;
        //                _token.Office_name = obj.External_dept_Name;
        //                SaveUtility.SaveTransactionLog(newToken, "GetToken", SaveUtility.TransStatus.Create, APIKey, IPAddress, "s"); //s: service
        //                result = Trueresult(_token);
        //            }
        //            else
        //            {
        //                result = falseresult("Invalid Token and Password...");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            SaveUtility.logError(ex);
        //            result = falseresult(ex.Message.ToString());
        //        }
        //        finally
        //        {
        //            map = null;
        //        }
        //    } 
        //    HttpContext.Current.Response.ContentType = "application/json";
        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
        //    HttpContext.Current.Response.End();
        //}
        //[HttpPost]
        //public void GetToken([FromBody]string value)
        //{
        //    //string IPAddress = HttpContext.Current.Request.UserHostAddress;
        //    //if (string.IsNullOrEmpty(APIKey))
        //    //    result = falseresult("UnAuthorized...");

        //    //if (string.IsNullOrEmpty(Password))
        //    //    result = falseresult("Please provide Password...");

        //    //if (string.IsNullOrEmpty(IPAddress))
        //    //    result = falseresult("Please provide IP Address...");

        //    ////if (!CheckPassEnscrypt(Password, ref Password))
        //    ////{ result = falseresult("UnAuthorized..."); }
        //    //else
        //    //{

        //    //    Department_ExMapDao map = new Department_ExMapDao();
        //    //    try
        //    //    {
        //    //        OfficerToken _token = new OfficerToken();
        //    //        string newToken = Guid.NewGuid().ToString();
        //    //        var obj = map.FindByKeygen(APIKey).Where(o => o.Password == Encryption.Encrypt(Password)).FirstOrDefault();
        //    //        if (obj != null)
        //    //        {
        //    //            GetAuthToken(_token, APIKey, newToken, IPAddress);
        //    //            //_token.Username = "";
        //    //            //_token.Org_id = obj.ID.ToString();
        //    //            _token.Org_name = obj.External_dept_Name;
        //    //            _token.Office_name = obj.External_dept_Name;
        //    //            SaveUtility.SaveTransactionLog(newToken, "GetToken", SaveUtility.TransStatus.Create, APIKey, IPAddress, "s"); //s: service
        //    //            result = Trueresult(_token);
        //    //        }
        //    //        else
        //    //        {
        //    //            result = falseresult("Invalid Token and Password...");
        //    //        }
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        SaveUtility.logError(ex);
        //    //        result = falseresult(ex.Message.ToString());
        //    //    }
        //    //    finally
        //    //    {
        //    //        map = null;
        //    //    }
        //    //}
        //    HttpContext.Current.Response.ContentType = "application/json";
        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
        //    HttpContext.Current.Response.End();
        //}


        #endregion




      //  [WebMethod(Description = "Get Timeline Header เป็นเมธอดสำหรับใช้ดึงข้อมูล ที่เกียวข้องกับหน่วยงานนั้นๆ <br />"
      //+ "Skip: ลำดับข้อมูลที่ต้องการเริ่มต้น เช่น เริ่มแถวที่ 1 <br />"
      //+ "take: จำนวนข้อมูลที่ต้องการ เช่น 10 ,20 แถว<br />"
      //+ "IsCloseJob: สถานะเรืองร้องทุกข์ (null=ทั้งหมด ,0=กำลังดำเนินการ ,1=ยุติการดำเนินการ)")]
        public void GetTimelineHeader(string tokenId, int skip, int take, string IsCloseJob)
        {
            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");
            string key = Encryption.Decrypt(tokenId);
            if (!IsValidateToken(key))
                result = falseresult("UnAuthorized.");



            if (skip < 1)
                skip = 1;

            if (take < 1)
                take = 20;

            try
            {
                AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();

                var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                if (objToken != null)
                {
                    var comList = ComplainData.GetComplainTimeLine(objToken.ApiKey, skip, take, 0,null);
                    SaveUtility.SaveTransactionLog(objToken.ApiKey, "Get Time line Header", SaveUtility.TransStatus.Create, key, objToken.Owner_ip, "s"); //s: service
                    result = Trueresult(comList);
                }
                else
                {
                    result = falseresult("UnAuthorized.");
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
    public class ComplainData
    {
        public static List<OCPB.Service.Model.GetComplainTimeLineResult> GetTimelineHeader_From_Dep(int CompID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@CompID", CompID);
            return DirectSqlDao.GetAllStored<OCPB.Service.Model.GetComplainTimeLineResult>("GetTimelineHeader_From_Dep", param);
        } 
        public static List<OCPB.Service.Model.GetComplainTimeLineResult> GetComplainTimeLine(string ApiKey, int? Skip, int? take, int? IsCloseJob, int? @CusID)
        {
            SqlParameter[] param = new SqlParameter[5];
            param[0] = new SqlParameter("@RefKey", !String.IsNullOrEmpty(ApiKey) ? ApiKey : (Object)DBNull.Value);
            param[1] = new SqlParameter("@Skip", Skip != null ? Skip : (Object)DBNull.Value);
            param[2] = new SqlParameter("@take", take != null ? take : (Object)DBNull.Value);
            param[3] = new SqlParameter("@IsCloseJob", IsCloseJob != null ? IsCloseJob : (Object)DBNull.Value);
            param[4] = new SqlParameter("@CusID", CusID != null ? CusID : (Object)DBNull.Value);
             return DirectSqlDao.GetAllStored<OCPB.Service.Model.GetComplainTimeLineResult>("GetComplainTimeLine", param);
        }
        //public static List<OCPB.Service.Model.ComplainlistDetail> GetComplainTimeLine(string ApiKey, int? Skip, int? take, int? IsCloseJob)
        //{
        //    SqlParameter[] param = new SqlParameter[4];
        //    param[0] = new SqlParameter("@RefKey", !String.IsNullOrEmpty(ApiKey) ? ApiKey : (Object)DBNull.Value);
        //    param[1] = new SqlParameter("@Skip", Skip != null ? Skip : (Object)DBNull.Value);
        //    param[2] = new SqlParameter("@take", take != null ? take : (Object)DBNull.Value);
        //    param[3] = new SqlParameter("@IsCloseJob", IsCloseJob != null ? IsCloseJob : (Object)DBNull.Value);
        //    return DirectSqlDao.GetAllStored<OCPB.Service.Model.ComplainlistDetail>("GetComplainTimeLine", param);
        //}

        public static List<OCPB.Service.Model.ComplainlistDetail> GetComplainByCaseId(string ApiKey, string Case_Id)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@RefKey", !String.IsNullOrEmpty(ApiKey) ? ApiKey : (Object)DBNull.Value);
            param[1] = new SqlParameter("@Case_Id", !String.IsNullOrEmpty(Case_Id) ? Case_Id.Trim() : (Object)DBNull.Value);
             return DirectSqlDao.GetAllStored<OCPB.Service.Model.ComplainlistDetail>("GetComplainByCaseId", param);
        }

        public static List<OCPB.Service.Model.ComplainLog> GetComplainLogByCaseId(string ApiKey, string Case_Id)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@RefKey", !String.IsNullOrEmpty(ApiKey) ? ApiKey : (Object)DBNull.Value);
            param[1] = new SqlParameter("@Case_Id", !String.IsNullOrEmpty(Case_Id) ? Case_Id.Trim() : (Object)DBNull.Value);

            return DirectSqlDao.GetAllStored<OCPB.Service.Model.ComplainLog>("GetComplainLogByCaseId", param);
        }


        public static List<ComplainGetFileModel> GetfileUpload(int ComplainID)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@ComplainID", ComplainID);
            return DirectSqlDao.GetAllStored<ComplainGetFileModel>("GetComplainFileUpload", param);
        }

        public static List<ComplainGetFileModel> GetfileUploadByCaseId(string ApiKey, string Case_Id)
        {
            SqlParameter[] param = new SqlParameter[2];
            param[0] = new SqlParameter("@RefKey", !String.IsNullOrEmpty(ApiKey) ? ApiKey : (Object)DBNull.Value);
            param[1] = new SqlParameter("@Case_Id", Case_Id);
            return DirectSqlDao.GetAllStored<ComplainGetFileModel>("GetComplainFileUploadByCaseId", param);
        }
    }

    public class CustomerModel
    {
        public CustomerModel() { }


        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }

    }

}
