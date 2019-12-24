using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OCPB.Controllers.Service
{
    public class MEnCryptionController : BaseServiceController
    {
        // GET api/mencryption
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/mencryption/5
        [Route("api/MEnCryption")]
        public void Get(string value)
        {
            _result(value);
        }

        // POST api/mencryption
        [Route("api/MEnCryption")]
        public void Post(string value)
        {
            _result(value);

        }
        private void _result(string value)
        { 
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(Trueresult(MobileEncryption.Encrypt(value).UrlEnscriptHttp())));
            HttpContext.Current.Response.End();
        }
        public class _value
        {
            public string value { get; set; }
        }
        //// PUT api/mencryption/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/mencryption/5
        //public void Delete(int id)
        //{
        //}
    }
}
