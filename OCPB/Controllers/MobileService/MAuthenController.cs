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
    public class MAuthenController : BaseServiceController
    {
        // GET api/login
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/login/5
        [Route("api/MAuthen")]
        public void Get(string tokenId, string Identity)
        {
            _resultLogin(tokenId, Identity ); 
        } 
        // POST api/login
           
        private void _resultLogin(string tokenId, string Identity )
        {
            try
            {
                if (string.IsNullOrEmpty(tokenId))
                    result = falseresult("UnAuthorized.");

                if (string.IsNullOrEmpty(Identity))
                    result = falseresult("Please provide citizen id.");

                //if (!IsValidateToken(key))
                //    result = falseresult("UnAuthorized.");
                if (!IsValidateToken(tokenId))
                    result = falseresult("UnAuthorized.");

                result = falseresult("ข้อมูลไม่ถูกต้อง");
                if (!string.IsNullOrEmpty(Identity) )
                {
                    CustomerMapDao map = new CustomerMapDao(); 
                    var _obj = map.FindAll().Where(o => o.IdentityID == Identity && o.Active == true).FirstOrDefault();
                    if (_obj != null)
                    { 
                            string _ID = MobileEncryption.Encrypt(_obj.ID.ToString()).UrlEnscriptHttp();
                            string UserKeys = MobileEncryption.Encrypt(_obj.Keygen).UrlEnscriptHttp();
                            result = Trueresult(new _resultValue { Fullname = _obj.FullNameStr, ID = _ID, UserKeys = UserKeys });
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
        
    }
}
