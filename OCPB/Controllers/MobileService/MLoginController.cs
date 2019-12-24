using Newtonsoft.Json;
using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OCPB.Controllers.Service
{
    public class MLoginController : BaseServiceController
    {
        // GET api/login
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/login/5
        [Route("api/MLogin")]
        public void Get(string Username, string Password)
        {
            _resultLogin( Username,  Password); 
        } 
        // POST api/login
         

        [HttpPost()]
        [Route("api/MLogin")]
        public void Post([FromUri]_login value)
        {
            _resultLogin(value.Username, value.Password); 
        }

        private void _resultLogin(string Username, string Password)
        {
            try
            {
                result = falseresult("ข้อมูลไม่ถูกต้อง");
                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                {
                    CustomerMapDao map = new CustomerMapDao();
                    Username = MobileEncryption.Decrypt(Username.UrlDescriptHttp());
                    var _obj = map.FindAll().Where(o => o.IdentityID == Username && o.Active == true).ToList();
                    if (_obj.Count() != 0)
                    {
                        var PEncrypt = MobileEncryption.Decrypt(Password.UrlDescriptHttp());
                        var obj = _obj.Where(o => o.Password == Encryption.Encrypt(PEncrypt) && o.Active == true).FirstOrDefault();
                        if (obj != null)
                        {

                            string _ID = MobileEncryption.Encrypt(obj.ID.ToString()).UrlEnscriptHttp();
                            string UserKeys = MobileEncryption.Encrypt(obj.Keygen).UrlEnscriptHttp();
                            result = Trueresult(new _resultValue { Fullname = obj.FullNameStr, ID = _ID, UserKeys = UserKeys });
                        } 
                    } 
                }
                else
                {
                    result = falseresult("ข้อมูลไม่ถูกต้อง");
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

        private class _resultValue
        {
            public string Fullname { get; set; }
            public string ID { get; set; }
            public string UserKeys { get; set; }
        } 
        public class _login
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        //// PUT api/login/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/login/5
        //public void Delete(int id)
        //{
        //}
    }
}
