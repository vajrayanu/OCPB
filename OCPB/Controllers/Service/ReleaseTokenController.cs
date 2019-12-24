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
    public class ReleaseTokenController : BaseServiceController
    {
        // GET api/releasetoken
        // GET api/releasetoken/5
        [Route("api/ReleaseToken")]
        public void Get(string tokenId)
        {
            _ReleaseToken(tokenId);
        }
        // POST api/releasetoken
        [Route("api/ReleaseToken")]
        public void Post([FromBody]_Release value)
        {
            _ReleaseToken(value.tokenId);
        }
        public class _Release
        {
            public string tokenId { get; set; }
        }
        private void _ReleaseToken(string tokenId)
        {

            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized...");

          // string key = Encryption.Decrypt(tokenId); //new key
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();

            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");

            try
            {
                var objToken = _authenMap.FindByKeygen(TokenValid.Keygen).ToList();
                if (objToken.Count() > 0)
                {
                    //disable token
                    foreach (var item in objToken)
                    {
                        item.Active = false;
                        _authenMap.AddOrUpdate(item);
                        _authenMap.CommitChange();

                        SaveUtility.SaveTransactionLog(item.Keygen, "Release Token", SaveUtility.TransStatus.Create, tokenId, item.Owner_ip, "s");
                    }
                }
            }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
                result = falseresult(ex.Message);
            }
            finally
            {
                _authenMap = null;
            }

            result = Trueresult("OK");
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        } 
    }
}
