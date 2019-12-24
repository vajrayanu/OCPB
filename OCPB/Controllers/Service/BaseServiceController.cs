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
using OCPB.Library;

namespace OCPB.Controllers
{
    public class BaseServiceController : ApiController
    {
        // GET api/config
        //public void Get()
        //{
        //    result = falseresult("รูปแบบข้อมูลไม่ถูกต้อง");
        //    HttpContext.Current.Response.ContentType = "application/json";
        //    HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
        //    HttpContext.Current.Response.End();
        //} 

        // GET api/baseservice
        protected Object result = new Object();
        protected static JsonResult _Noresult()
        {
            return new JsonResult() { Result = false, Message = "No result..." };
        }
        protected static JsonResult falseresult(Object message)
        {
            return new JsonResult() { Result = false, Message = message };
        }
        protected static JsonResult Trueresult(Object message)
        {
            return new JsonResult() { Result = true, Message = message };
        }
        protected class JsonResult
        {
            public bool Result { get; set; }
            public Object Message { get; set; }
        }
        protected void GetAuthToken(OfficerToken token, string key, string newKey, string ip)
        {
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            AuthenticateToken Obj = new AuthenticateToken();


            //_authenMap.AddOrUpdate(Obj);

            var objToken = _authenMap.FindActiveByAPIKey(key).ToList();
            foreach (var Items in objToken)
            {
                Items.Active = false;
                _authenMap.AddOrUpdate(Items);
                _authenMap.CommitChange();
            }
            //if (objToken.Count() > 0)
            //{
            //    //disable token
            //    foreach (var item in objToken)
            //    {
            //        if (item.Owner_ip == ip && Convert.ToDateTime(item.Process_date).AddMinutes(item.Process_time) > DateTime.Now)
            //        {
            //            token.Token_id = Encryption.Encrypt(item.Keygen);
            //        }
            //        else
            //        {
            //            item.Active = false;
            //            _authenMap.AddOrUpdate(item);
            //            _authenMap.CommitChange();
            //        }
            //    }
            //}

            //if (string.IsNullOrEmpty(token.Token_id))
            //{


             
                var authen = new AuthenticateToken(key, newKey, ip);
                _authenMap.AddOrUpdate(authen);
                _authenMap.CommitChange();
                token.Token_id = MyExtensions.ParamEncode(new { authen.ApiKey, Process_date = authen.Process_date.ToThaiFormateAndtime(), authen.Process_time, authen.Keygen });
              //token   = 
            //}

            //_authenMap = null;
        }
        protected class _TokenCheck
        {
            public string ApiKey { get; set; }
            public DateTime? Process_date { get; set; }
            public string Process_time { get; set; }
            public string Keygen { get; set; }
        }

        protected string IPAddress = HttpContext.Current.Request.UserHostAddress;
        protected _TokenCheck TokenValid = new _TokenCheck();
        protected bool IsValidateToken(string tokenId)
        {
            var Obj = MyExtensions.DecodeObj<_TokenCheck>(tokenId);
            TokenValid = Obj;
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            var _obj = _authenMap.FindByKeygenAndActive(Obj.Keygen).Where(o => o.ApiKey == Obj.ApiKey).FirstOrDefault();
            if (_obj == null)
            {
                return false;
            }
            else {

                var start = DateTime.Now;

                if (start.Subtract(Obj.Process_date.Value) <= TimeSpan.FromMinutes(20))
                {
                    //20 minutes were passed from start
                    return true;
                }
                else
                {
                    return false;
                }
            }


            //AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            //bool bResult = false;

            //var objToken = _authenMap.FindByKeygen(tokenId).ToList();

            //if (objToken.Count() > 0)
            //{
            //    //disable token
            //    foreach (var item in objToken)
            //    {
            //        if (Convert.ToDateTime(item.Process_date).AddMinutes(item.Process_time) > DateTime.Now)
            //        {
            //            bResult = true;
            //        }
            //        else
            //        {
            //            item.Active = false;
            //            _authenMap.AddOrUpdate(item);
            //            _authenMap.CommitChange();
            //        }
            //    }
            //}
            //_authenMap = null;
            //return bResult;
        }


    }
}
