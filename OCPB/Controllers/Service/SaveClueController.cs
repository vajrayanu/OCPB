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
    public class SaveClueController : BaseServiceController
    {
        // GET api/saveclue
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/saveclue/5
        [Route("api/SaveClue")]
        public void Get( string tokenId, string title, string description, string Consumer_firstname, string Consumer_lastname, string Consumer_Email, string Consumer_Mobile, string address, string Amphur_text, string Province_text)
        {
            _SaveClue(tokenId, title, description, Consumer_firstname, Consumer_lastname, Consumer_Email, Consumer_Mobile, address, Amphur_text, Province_text);
        }

        // POST api/saveclue
        [Route("api/SaveClue")]
        public void Post([FromBody]_Clue value)
        {
            _SaveClue(value.tokenId, value.title, value.description, value.Consumer_firstname, value.Consumer_lastname, value.Consumer_Email, value.Consumer_Mobile, value.address, value.Amphur_text, value.Province_text);
        }

        public class _Clue
        {
            public string tokenId { get; set; }public string title { get; set; }public string description { get; set; }public string Consumer_firstname { get; set; }public string Consumer_lastname { get; set; }public string Consumer_Email { get; set; }public string Consumer_Mobile { get; set; }public string address { get; set; }public string Amphur_text { get; set; }public string Province_text { get; set; }
        
        }

        public void _SaveClue(string tokenId, string title, string description, string Consumer_firstname, string Consumer_lastname, string Consumer_Email, string Consumer_Mobile, string address, string Amphur_text, string Province_text)
        {
            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");
            //string key = Encryption.Decrypt(tokenId);
            //if (!IsValidateToken(key))
            //    result = falseresult("UnAuthorized.");

            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");

            ClueMapDao clueMap = new ClueMapDao();
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
            Department_ExMapDao _departMap = new Department_ExMapDao();
            //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
            try
            {
                int? ChannelID = _departMap.FindByKeygen( TokenValid.ApiKey).FirstOrDefault().ChanelID;
                Clue obj = new Clue();
                obj.Active = true;
                obj.CreateDate = DateTime.Now;
                obj.Keygen = Guid.NewGuid().ToString();
                obj.Title = title;
                obj.Address = address;
                obj.Description = description;
                obj.Amphur_text = Amphur_text;
                obj.Province_text = Province_text;
                obj.Complain_Channel_id = ChannelID;
                obj.Fname = Consumer_firstname;
                obj.Lname = Consumer_lastname;
                obj.Mobile = Consumer_Mobile;
                obj.Email = Consumer_Email;
                clueMap.Add(obj);
                clueMap.CommitChange();
                SaveUtility.SaveTransactionLog(obj.Keygen, "Save Clue", SaveUtility.TransStatus.Create, TokenValid.Keygen, IPAddress, "s"); //s: service
                clueMap = null;
                result = Trueresult("ระบบได้รับคำแจ้งเบาแสเรียบร้อยแล้ว");
            }
            catch (Exception ex)
            {
                clueMap = null;
                _authenMap = null;
                SaveUtility.logError(ex);
                result = falseresult(ex.Message);
            }
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }

        //// PUT api/saveclue/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/saveclue/5
        //public void Delete(int id)
        //{
        //}
    }
}
