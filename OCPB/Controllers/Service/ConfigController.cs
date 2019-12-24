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
using OCPB.Library;
namespace OCPB.Controllers.Service
{
    public class ConfigController : BaseServiceController
    {

        // GET api/config/5
        [Route("api/Config")]
        public void Get(string tokenId, string _Type, string refID)
        { 
              _Config(tokenId, _Type, refID);
        }

        private void _Config(string tokenId, string _Type, string refID)
        { 
            if (string.IsNullOrEmpty(tokenId))
                result = falseresult("UnAuthorized.");

            //if (string.IsNullOrEmpty(Case_Id))
            //    result = falseresult("Please provide Case Id.");

          
            //string key = Encryption.Decrypt(tokenId);

            if (!IsValidateToken(tokenId))
                result = falseresult("UnAuthorized.");

            Department_ExMapDao _departMap = new Department_ExMapDao();
            AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();

            try
            {
                //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                //if (objToken != null)
                //{
                //    var objDepart = _departMap.FindByKeygen(objToken.ApiKey).FirstOrDefault();

                //********************* Get Config*********************
                var _obj = new Object();

                switch (_Type.ToUpper())
                {
                    case "PROVINCE":
                        {
                            Tm_provinceMapDao Map = new Tm_provinceMapDao();
                            _obj = Map.FindByActive().Select(o => new { o.ID, o.Province_NameEN, o.Province_NameTH }).OrderBy(o => o.Province_NameTH).ToList();
                            break;
                        }

                    case "DISTRICT":
                        {
                            Tm_districtMapDao Map = new Tm_districtMapDao();
                         
                            if (!string.IsNullOrEmpty(refID))
                            {
                                _obj = Map.FindByActive().Where(o=>o.Prefecture_ID == refID.Toint()).Select(o => new { o.ID, o.Province_ID, o.Prefecture_ID, o.Zipcode, o.District_NameEN, o.District_NameTH }).OrderBy(o=>o.District_NameTH).ToList();
                            }
                            else
                            {
                                _obj = Map.FindByActive().Select(o => new { o.ID, o.Province_ID, o.Prefecture_ID, o.Zipcode, o.District_NameEN, o.District_NameTH }).OrderBy(o => o.District_NameTH).ToList();
                            }
                            break;
                        }
                    case "PREFECTURE":
                        {
                            Tm_prefectureMapDao Map = new Tm_prefectureMapDao();
                            if (!string.IsNullOrEmpty(refID))
                            {
                                _obj = Map.FindByActive().Where(o => o.Province_ID == refID.Toint()).Select(o => new { o.ID, o.Prefecture_ID, o.Prefecture_NameEN, o.Prefecture_NameTH, o.Province_ID }).OrderBy(o => o.Prefecture_NameTH).ToList();
                             }
                            else
                            {
                                _obj = Map.FindByActive().Select(o => new { o.ID, o.Prefecture_ID, o.Prefecture_NameEN, o.Prefecture_NameTH, o.Province_ID }).OrderBy(o => o.Prefecture_NameTH).ToList();
                            }
                            break;
                        }

                    case "TYPE_0":
                        {
                            Tm_Complain_TypeMapDao Map = new Tm_Complain_TypeMapDao();
                            _obj = Map.FindByLevel0().Select(o => new { o.Complain_Type_Class, o.Complain_Type_Code, o.Complain_Type_Level, o.Complain_Type_Name, o.complain_type_name_en, o.Complain_Type_Refer, o.Complain_Type_Sub_ID, o.Complain_Type_SubLevel, o.ID }).ToList();
                            break;
                        }

                    case "TYPE_1":
                        {
                            Tm_Complain_TypeMapDao Map = new Tm_Complain_TypeMapDao();
                            if (!string.IsNullOrEmpty(refID))
                            {
                                _obj = Map.FindByLevel1(refID.Toint()).Select(o => new { o.Complain_Type_Class, o.Complain_Type_Code, o.Complain_Type_Level, o.Complain_Type_Name, o.complain_type_name_en, o.Complain_Type_Refer, o.Complain_Type_Sub_ID, o.Complain_Type_SubLevel, o.ID }).ToList();
                            }
                            else
                            {
                                _obj = Map.FindAll().Select(o => new { o.Complain_Type_Class, o.Complain_Type_Code, o.Complain_Type_Level, o.Complain_Type_Name, o.complain_type_name_en, o.Complain_Type_Refer, o.Complain_Type_Sub_ID, o.Complain_Type_SubLevel, o.ID }).ToList();
                            } 
                            break;
                        }

                    case "CASE":
                        {
                            if (!string.IsNullOrEmpty(refID))
                            {
                                _obj = GetUtility.GetTm_case(refID.Toint()).Select(o => new { o.CaseID, o.CasenameEN, o.CasenameTH, o.Complain_Cause_id, o.Complain_Type_Name, o.complain_type_name_en, o.Tm_Cause_GruopID, o.Tm_CauseID, o.Tm_Complain_TypeID }).ToList();
                            }
                            else
                            {
                                _obj = GetUtility.GetTm_case().Select(o => new { o.CaseID, o.CasenameEN, o.CasenameTH, o.Complain_Cause_id, o.Complain_Type_Name, o.complain_type_name_en, o.Tm_Cause_GruopID, o.Tm_CauseID, o.Tm_Complain_TypeID }).ToList();
                            }
                            break;
                        }
                    case "PLACE":
                        {
                            Tm_PlaceOfPurchaseMapDao map = new Tm_PlaceOfPurchaseMapDao();
                            _obj = map.FindByActive().Select(o => new { o.ID, o.PlaceNameEng, o.PlaceNameThai }).ToList();
                            break;
                        }
                    case "MOV":
                        {
                            Tm_MotivateMapDao map = new Tm_MotivateMapDao();
                            _obj = map.FindByActive().Select(o => new { o.MotiveNameEng, o.MotiveNameThai, o.ID }).ToList();
                            break;
                        }
                    case "PAY":
                        {
                            Tm_PaymentMapDao map = new Tm_PaymentMapDao();
                            _obj = map.FindByActive().Select(o => new { o.ID, o.PaymentNameEng, o.PaymentNameThai });
                            break;
                        }
                    case "JOB":
                        {
                            Tm_OccupationMapDao map = new Tm_OccupationMapDao();
                            _obj = map.FindByActive().Select(o => new { o.ID, o.Occupation_Name, o.occupation_name_en });
                            break;
                        }
                    case "SALARY":
                        {
                            Tm_SalaryMapDao map = new Tm_SalaryMapDao();
                            _obj = map.FindByActive().Select(o => new { o.ID, o.SalaryName, o.SalaryNameEN });
                            break;
                        }
                    case "TITLE":
                        {
                            User_TitleMapDao map = new User_TitleMapDao();
                            _obj = map.FindByActive().Select(o => new { o.ID, o.Description, o.IsThai });
                            break;
                        } 
                        case "CANCEL":
                        {
                            Complain_Cancel_StatusMapDao map = new Complain_Cancel_StatusMapDao();
                            _obj = map.FindByActive().Select(o => new { o.ID, o.ComplainCancelStatusName });
                            break;
                        }

                        case "OCCUPATION"  :
                        {
                            Tm_OccupationMapDao map = new Tm_OccupationMapDao();
                            _obj = map.FindByActive().Select(o => new { o.ID, o.occupation_name_en, o.Occupation_Name });
                            break;
                        } 
                        
                }
                SaveUtility.SaveTransactionLog(TokenValid.ApiKey, "Config", SaveUtility.TransStatus.View, tokenId, IPAddress, "s"); //s: service
                result = Trueresult(_obj);
                //}
                //else
                //{
                //    result = falseresult("UnAuthorized.");
                //} 
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

        // POST api/config
        [Route("api/Config")]
        public void Post([FromUri]_ConfigModel value)
        {
            _Config(value.tokenId, value._Type, value.refID);
        }

        public class _ConfigModel
        {
            public string tokenId { get; set; }
            public string _Type { get; set; }
            public string refID { get; set; }
        }
    }
}
