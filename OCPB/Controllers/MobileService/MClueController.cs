using Newtonsoft.Json;
using OCPB.Model;
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
    public class MClueController : BaseServiceController
    {
        //// GET api/mclue
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/mclue/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/mclue
        [Route("api/MClue")]
        public void Post([FromBody]MClueModel value)
        {
            result = falseresult("ข้อมูลไม่ถูกต้อง");
            try
            {
                Clue obj = new Clue();
                ClueMapDao clueMap = new ClueMapDao();
                if (obj.ID == 0)
                {
                    obj.Active = true;
                    obj.CreateDate = DateTime.Now;
                    obj.Keygen = Guid.NewGuid().ToString();
                }
                obj.Address = value.Address;
                obj.Description = value.Description;
                obj.Title = value.Title;
                obj.Email = value.Email;
                obj.Fname = value.Fname;
                obj.Lname = value.Lname;
                obj.Mobile = value.Mobile;
                obj.Url = value.Url;
                obj.PrefectureID = value.PrefectureID;
                obj.ProvinceID = value.ProvinceID;
                obj.Complain_Channel_id = 1;//เรื่องร้องทุกข์ Online
                clueMap.Add(obj);
                clueMap.CommitChange();
                result = Trueresult("ได้รับเรื่องร้องเรียนของท่านเรียบร้อยแล้ว");
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

        public class MClueModel
        {
            public virtual string Address { get; set; }
            public virtual string Description { get; set; }
            public virtual string Title { get; set; }
            public virtual string Email { get; set; }
            public virtual string Fname { get; set; }
            public virtual string Lname { get; set; }
            public virtual string Mobile { get; set; }
            public virtual string Url { get; set; }
            public virtual int PrefectureID { get; set; }
            public virtual int ProvinceID { get; set; }
        }
        //// PUT api/mclue/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/mclue/5
        //public void Delete(int id)
        //{
        //}
    }
}
