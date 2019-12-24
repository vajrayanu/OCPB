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
    public class UserLoginController : BaseServiceController
    {
       

        // GET api/gettoken/5
        //public void Get( string APIKey, string Password)
        //{
        //      TokenData(APIKey, Password);
        //}
        [HttpGet]
        [Route("api/UserLogin")]
        public void Get(string username, string Password)
        {
            GetLogin(username, Password);
        }

        [HttpGet]
        [Route("api/UserLogin")]
        public void Get(string value )
        {
            GetSearch(value);
        }

        private void GetLogin(string username, string Password)
        {
            UsersMapDao map = new UsersMapDao();
            var obj = map.FindUserAndPass(username).Where(o => o.Password == Encryption.Encrypt(Password) && o.Active == true).ToList();
            if (obj.Count() > 0  )
            {
                result = Trueresult(obj.Select(o => new { o.ID, o.Keygen, o.LName, o.FName }));
            }
            else
            {
                result = falseresult("ไม่พบข้อมูล");
            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }

        private void GetSearch(string value)
        {
            var obj = GetUtility.GetUsers(value, null, null, true, null);
            if (obj != null)
            {
                result = Trueresult(obj.Select(o=>new {o.ID,o.Keygen,o.LName,o.FName }).ToList());
            }
            else
            {
                result = falseresult("ไม่พบข้อมูล");
            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }

        // POST api/gettoken
        //public void Post([FromBody]_Token value )
        //{
        //    TokenData(value.APIKey, value.Password); 
        //}

        //public void Post(string APIKey, string Password)
        //{
        //    TokenData( APIKey, Password);
        //}
       [HttpPost()]
        [Route("api/UserLogin")]
        public void Post([FromUri]_Token value, [FromBody]_Token Bvalue)
        {
            if (value.APIKey != null)
            {
                TokenData(value.APIKey, value.Password);
            }
            else 
            {
                TokenData(Bvalue.APIKey, Bvalue.Password);
            }
        }
        public class _Token 
        {
           public string APIKey{get;set;} 
           public string Password {get;set;} 
        }

        protected void TokenData(string APIKey, string Password)
        {
           
            if (string.IsNullOrEmpty(APIKey))
                result = falseresult("UnAuthorized...");

            else  if (string.IsNullOrEmpty(Password))
                result = falseresult("Please provide Password...");

            else  if (string.IsNullOrEmpty(IPAddress))
                result = falseresult("Please provide IP Address...");

            //if (!CheckPassEnscrypt(Password, ref Password))
            //{ result = falseresult("UnAuthorized..."); }
            else
            {

                Department_ExMapDao map = new Department_ExMapDao();
                try
                {
                    OfficerToken _token = new OfficerToken();
                    string newToken = Guid.NewGuid().ToString();
                    string _Pass = Encryption.Encrypt(Password);
                    var _obj = map.FindByKeygen(APIKey.Trim());

                  var  obj = _obj.Where(o => o.Password == _Pass).FirstOrDefault();
                    if (obj != null)
                    {
                        GetAuthToken(_token, APIKey, newToken, IPAddress);
                        //_token.Username = "";
                        //_token.Org_id = obj.ID.ToString();
                        _token.Org_name = obj.External_dept_Name;
                        _token.Office_name = obj.External_dept_Name;
                        SaveUtility.SaveTransactionLog(newToken, "GetToken", SaveUtility.TransStatus.Create, APIKey, IPAddress, "s"); //s: service
                        result = Trueresult(_token);
                    }
                    else
                    {
                        result = falseresult("Invalid Token and Password...");
                    }
                }
                catch (Exception ex)
                {
                    SaveUtility.logError(ex);
                    result = falseresult(ex.Message.ToString());
                }
                finally
                {
                    map = null;
                }
            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }
         
    }
}
