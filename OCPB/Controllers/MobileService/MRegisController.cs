using Newtonsoft.Json;
using OCPB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OCPB.Controllers.MobileService
{
    public class MRegisController : BaseServiceController
    {
        // GET api/mregis
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/mregis/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/mregis
        [Route("api/MRegis")]
        public void Post([FromUri]Customer value)
        {  
            value.FromApp = 1;
            value.TypeCustomer = null;
            value.IsOversea = false;
            OCPB.Controllers.HomeController Obj = new HomeController();
            
            var _resultRegis = Obj._RegisterTH(value);
            if (_resultRegis.Status == true)
            {
                _resultRegis.text = "success";
                result = Trueresult(_resultRegis);
            }
            else
            {
                result = falseresult(_resultRegis);
            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End(); 
        }

        // PUT api/mregis/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/mregis/5
        //public void Delete(int id)
        //{
        //}
    }
}
