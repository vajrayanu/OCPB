using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OCPB.Repository.Repositories;
namespace OCPB.Controllers.MobileService
{
    public class MForgetPassController : BaseServiceController
    {
        // GET api/mchangpass
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //} 
        // GET api/mchangpass/5
        [Route("api/MForgetPass")]
        public void Get(string Email, string Identity)
        {
            _MChange(Identity, Email);
        }

        [Route("api/MForgetPass")]
        public void Get(string ID, string UserKeys, string Oldpass, string newPass, string PassCompare)
        {
            //_MChange(Identity, Email);

            int _id = MobileEncryption.Decrypt(ID.UrlDescriptHttp()).Toint();
            string Key = MobileEncryption.Decrypt(UserKeys.UrlDescriptHttp());
            CustomerMapDao map = new CustomerMapDao();
            var _obj = map.FindByActive().Where(o => o.ID == _id && o.Keygen == Key).FirstOrDefault();
            result = falseresult("ข้อมูลไม่ถูกต้อง"); 
            if (newPass == PassCompare || _obj != null)
            {
                if (_obj.Password == Encryption.Encrypt(Oldpass))
                {
                    _obj.Password = Encryption.Encrypt(newPass);
                    map.AddOrUpdate(_obj);
                    map.CommitChange();
                    result = Trueresult("แก้ไขข้อมูลเรียบร้อยแล้ว");
                }
            }
            
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End(); 
        }




        // POST api/mchangpass
        [Route("api/MForgetPass")]
        public void Post([FromUri]_PassModel value)
        {
            if (!string.IsNullOrEmpty(value.Email))
            {
                _MChange(value.Identity, value.Email);
            } 
        }

        public void _MChange(string Identity, string Email)
        {
            result = falseresult("ข้อมูลไม่ถูกต้อง");
            try
            {
                OCPB.Controllers.HomeController obj = new HomeController();
                var _resultMChange = obj._Forget(MobileEncryption.Decrypt(Identity.UrlDescriptHttp()), MobileEncryption.Decrypt(Email.UrlDescriptHttp()));
                if (_resultMChange.Status == true)
                {
                    result = Trueresult(_resultMChange);
                }
                else
                {
                    result = falseresult(_resultMChange);
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

        // PUT api/mchangpass/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/mchangpass/5
        //public void Delete(int id)
        //{
        //}
        public class _PassModel
        {
            public string Email { get; set; }
            public string Identity { get; set; } 
        }

    }
}
