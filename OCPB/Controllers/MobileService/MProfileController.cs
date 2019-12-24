using Newtonsoft.Json;
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
    public class MProfileController : BaseServiceController
    {
        //// GET api/mprofile
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/mprofile/5
        [Route("api/MProfile")]
        public void Get(string ID, string UserKeys)
        {
            result = falseresult("ข้อมูลไม่ถูกต้อง");
            try
            {
                CustomerMapDao map = new CustomerMapDao();
                int _id = MobileEncryption.Decrypt(ID.UrlDescriptHttp()).Toint();
                string Key = MobileEncryption.Decrypt(UserKeys.UrlDescriptHttp());
                var Obj = map.FindByActive().Where(o => o.ID == _id && o.Keygen == Key).ToList();
                if (Obj.Count() > 0)
                {
                    result = Trueresult(Obj.Select(o => new { o.Address, o.ContinentsID, o.CountriesID, o.DateOfBirthStr, o.DistrictID, o.Email, o.Fax, o.Fname, o.IdentityID, o.Lname, o.Mobile, o.OccupationID, o.PrefectureID, o.ProvinceID, o.RegisterAddress, o.SalaryID, o.Sex, o.Tel, o.Tel_ext, o.TitleID, o.ZipCode }).FirstOrDefault());
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

        // POST api/mprofile
        [Route("api/MProfile")]
        public void Post([FromUri]Pfile items)
        {

            //    Encryption.Encrypt
            try
            {
                result = falseresult("ข้อมูลไม่ถูกต้อง");
                CustomerMapDao map = new CustomerMapDao(); 
                int _id = MobileEncryption.Decrypt(items.ID.UrlDescriptHttp()).Toint();
                string Key = MobileEncryption.Decrypt(items.UserKeys.UrlDescriptHttp());
                var Obj = map.FindByActive().Where(o => o.ID == _id && o.Keygen == Key).FirstOrDefault();
                if (Obj != null)
                {
                    SaveAccount.UpdateUser(Obj.ID, items.TitleID, items.Fname, items.Lname, items.DateOfBirth, items.OccupationID, items.SalaryID, items.Address, items.ProvinceID, items.PrefectureID, items.DistrictID, items.ZipCode,
                              items.Tel, items.Tel_ext, items.Mobile, items.Fax, items.Email);
                    result = Trueresult("แก้ไขข้อมูลเรียบร้อย");
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
        public class Pfile
        {
            public string ID { get; set; }
            public string UserKeys { get; set; }
            public int TitleID { get; set; }
            public string Fname { get; set; }
            public string Lname { get; set; }
            public string DateOfBirth { get; set; }
            public int OccupationID { get; set; }
            public int SalaryID { get; set; }
            public string Address { get; set; }
            public int ProvinceID { get; set; }
            public int PrefectureID { get; set; }
            public int DistrictID { get; set; }
            public string ZipCode { get; set; }
            public string Tel { get; set; }
            public string Tel_ext { get; set; }
            public string Mobile { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; } 
        }
        //// PUT api/mprofile/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/mprofile/5
        //public void Delete(int id)
        //{
        //}
    }
}
