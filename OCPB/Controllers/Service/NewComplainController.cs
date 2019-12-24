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
    public class NewComplainController : BaseServiceController
    {
 //       [HttpGet]
 //       public void Get(string tokenId, string Identification_number, string Consumer_firstname, string Consumer_lastname, string Consumer_gender, string Consumer_Birth
 //, string Consumer_Address, string Consumer_ZipCode, string Consumer_Tel, string Consumer_Tel_Ex, string Consumer_Mobile, string Consumer_Fax, string Consumer_Email, string Complain_Subject
 //, string Complain_Details, string DefendentName, string DefendentDescription, int TYPE_0, int TYPE_1, int CASEID, int PurchaseID, int PaymentID, int MotiveID, string IsOversea, string OverseaAddress)
 //       {
 //           _AddComplain(tokenId, Identification_number, Consumer_firstname, Consumer_lastname, Consumer_gender, Consumer_Birth
 //, Consumer_Address, Consumer_ZipCode, Consumer_Tel, Consumer_Tel_Ex, Consumer_Mobile, Consumer_Fax, Consumer_Email, Complain_Subject
 //, Complain_Details, DefendentName, DefendentDescription, TYPE_0,TYPE_1,CASEID, PurchaseID,PaymentID,MotiveID, IsOversea, OverseaAddress);
 //       }

         [HttpPost()]
        [Route("api/NewComplain")]
        public void Post([FromBody]_AddComplainModel value)
        {
            _AddComplain(value.tokenId, value.Identification_number, value.Consumer_Title, value.Consumer_firstname, value.Consumer_lastname, value.Consumer_gender, value.Consumer_Birth
 , value.Consumer_Address, value.Consumer_Tumbol, value.Consumer_Amphur, value.Consumer_Tumbol, value.Consumer_Province, value.Consumer_Tel, value.Consumer_Tel_Ex, value.Consumer_Mobile, value.Consumer_Fax, value.Consumer_Email, value.Complain_Subject
 , value.Complain_Details, value.DefendentName, value.DefendentDescription,value.TYPE_0,value.TYPE_1,value.CASEID,value.PurchaseID,value.PaymentID,value.MotiveID, value.IsOversea, value.OverseaAddress);
        }

        public class _AddComplainModel
        {
            public string tokenId { get; set; }
            public string Identification_number { get; set; }
            public string Consumer_Title { get; set; }

            public string Consumer_firstname { get; set; }
            public string Consumer_lastname { get; set; }
            public string Consumer_gender { get; set; }
            public string Consumer_Birth { get; set; }

            public string Consumer_Address { get; set; }
            public string Consumer_Tumbol { get; set; }
            public string Consumer_Amphur { get; set; }
            public string Consumer_Province { get; set; }

            public string Consumer_ZipCode { get; set; }
            public string Consumer_Tel { get; set; }
            public string Consumer_Tel_Ex { get; set; }
            public string Consumer_Mobile { get; set; }
            public string Consumer_Fax { get; set; }
            public string Consumer_Email { get; set; }
            public string Complain_Subject { get; set; }
            public string Complain_Details { get; set; }
            public string DefendentName { get; set; }
            public string DefendentDescription { get; set; }
             

            public int TYPE_0 { get; set; }
            public int TYPE_1 { get; set; }
            public int CASEID { get; set; }
            public int PurchaseID { get; set; }
            public int PaymentID { get; set; }
             public int MotiveID { get; set; } 
            public string IsOversea { get; set; }
            public string OverseaAddress { get; set; }
        }


        public void _AddComplain(string tokenId, string Identification_number, string Consumer_Title, string Consumer_firstname, string Consumer_lastname, string Consumer_gender, string Consumer_Birth
, string Consumer_Address, string Consumer_Tumbol, string Consumer_Amphur, string Consumer_Province, string Consumer_ZipCode, string Consumer_Tel, string Consumer_Tel_Ex, string Consumer_Mobile, string Consumer_Fax, string Consumer_Email, string Complain_Subject
, string Complain_Details, string DefendentName, string DefendentDescription, int TYPE_0, int TYPE_1, int CASEID, int PurchaseID, int PaymentID, int MotiveID, string IsOversea, string OverseaAddress)
        {
            try
                {
            if (string.IsNullOrEmpty(tokenId))
            { result = falseresult("UnAuthorized."); }
            else if (string.IsNullOrEmpty(Identification_number))
            { result = falseresult("Please provide Identification number."); }

            //string key = Encryption.Decrypt(tokenId);

            //if (!IsValidateToken(key))
            //    result = falseresult("UnAuthorized.");
            if (!IsValidateToken(tokenId))
            { result = falseresult("UnAuthorized."); }
            else
            {
                Department_ExMapDao _departMap = new Department_ExMapDao();
                AuthenticateTokenMapDao _authenMap = new AuthenticateTokenMapDao();
                CustomerMapDao _cusMap = new CustomerMapDao();
                CustomerVerifyMapDao _mapVer = new CustomerVerifyMapDao();
                ComplainsMapDao _Map = new ComplainsMapDao();
                Complains_WebService_logMapDao _logMap = new Complains_WebService_logMapDao();
                
                    Complains _Item = new Complains();
                    //var objToken = _authenMap.FindByKeygen(key).FirstOrDefault();
                    //if (objToken != null)
                    //{

                    string Complain_Code_ID = null;
                    var Dep_ex = _departMap.FindByKeygen(TokenValid.ApiKey).FirstOrDefault();
                    int? ChannelID = Dep_ex.ChanelID;
                    int? Title = null;
                    int? Ampher = null;
                    int? Tumbon = null;
                    int? Province = null;

                    if (!string.IsNullOrEmpty(Consumer_Title))
                    {
                        Title = Consumer_Title.Toint();
                    }
                    if (!string.IsNullOrEmpty(Consumer_Amphur))
                    {
                        Ampher = Consumer_Amphur.Toint();
                    }
                    if (!string.IsNullOrEmpty(Consumer_Tumbol))
                    {
                        Tumbon = Consumer_Tumbol.Toint();
                    }
                    if (!string.IsNullOrEmpty(Consumer_Province))
                    {
                        Province = Consumer_Province.Toint();
                    }


                    int? CusID = SaveAccount.CheckUserAndNewregis(Title, Identification_number, Consumer_firstname, Consumer_lastname, Consumer_gender, Consumer_Birth, Consumer_Address, Tumbon, Ampher, Province, Consumer_ZipCode, Consumer_Tel, Consumer_Tel_Ex, Consumer_Mobile, Consumer_Fax, Consumer_Email, false,null,null);
                    if (IsOversea.ToUpper().Trim() == "TRUE")
                    {
                        var CusObj = _cusMap.FindById(CusID.Toint());
                        CusObj.IsOversea = true;
                        _cusMap.AddOrUpdate(CusObj);
                        _cusMap.CommitChange();
                        Customer_OverseaMapDao OverSeaMap = new Customer_OverseaMapDao();
                        if (OverSeaMap.FindAll().Where(o => o.CustomerID == CusID).ToList().Count() == 0)
                        {
                            Customer_Oversea SMapObj = new Customer_Oversea();
                            SMapObj.CustomerID = CusID;
                            SMapObj.address_oversea = OverseaAddress;
                            OverSeaMap.Add(SMapObj);
                            OverSeaMap.CommitChange();
                        }
                    }

                    int Id = SaveComplain.AddnewComplain(Complain_Subject, ChannelID, CusID, DefendentName, DefendentDescription, Complain_Details
                        , TYPE_0, TYPE_1, CASEID, PurchaseID, null, PaymentID, null, MotiveID, null, null, null, ref Complain_Code_ID);
                    SaveComplain.StartTrack(Id, 3, Dep_ex.Keygen,null);

                    _Item = _Map.FindById(Id);

                    //*********************Save Complain Service Log  

                    Complains_WebService_log _log = new Complains_WebService_log();
                    _log.IdentityID = Identification_number;
                    _log.Sex = Consumer_gender != null ? Consumer_gender.ToLower() : "";
                    _log.ApiKey = TokenValid.ApiKey;
                    _log.CreateDate = DateTime.Now;
                    _log.FullName = Consumer_firstname + " " + Consumer_lastname;
                    // _log.TitleID = TitleID;
                    _log.Fname = Consumer_firstname;
                    _log.Lname = Consumer_lastname;


                    _log.DateOfBirth = Consumer_Birth;

                    _log.Address = Consumer_Address;
                    _log.ZipCode = Consumer_ZipCode;
                    _log.Tel = Consumer_Tel;
                    _log.Tel_ext = Consumer_Tel_Ex;
                    _log.Mobile = Consumer_Mobile;
                    _log.Fax = Consumer_Fax;
                    _log.Email = Consumer_Email;
                    _log.Complain_Subject = Complain_Subject;
                    _log.Complain_Details = Complain_Details;
                    _log.CompanyName = DefendentName;
                    _log.CompanyDescription = DefendentDescription;
                    _log.PaymentID = PaymentID;
                    _log.PlacePurchaseID = PurchaseID;
                    _log.MotiveID = MotiveID;
                    //_log.Payment_Text = Payment;
                    //_log.PlacePurchase_Text = PlacePurchase;
                    //_log.Motive_Text = Motive;
                    _log.IsOversea = (IsOversea.Trim().ToUpper() == "TRUE") ? true : false;
                    _log.OverseaAddress = OverseaAddress;
                    _logMap.AddOrUpdate(_log);
                    _logMap.CommitChange();
                    Complains_DepartmentMapDao DepTMapDao = new Complains_DepartmentMapDao();
                    Complains_Department _Dept = new Complains_Department();
                    _Dept.ComplainID = Id;
                    _Dept.DepartmentID = Dep_ex.ID;
                    DepTMapDao.Add(_Dept);
                    DepTMapDao.CommitChange();
                    SaveUtility.SaveTransactionLog(_Item.Keygen, "Add Complain", SaveUtility.TransStatus.Create, TokenValid.Keygen, IPAddress, "s"); //s: service
                    result = Trueresult(new OCPB.Service.Model.Complain(_Item.Complain_Code_ID, _Item.Complain_Date.ToThaiFormate(), _Item.Complain_Time, _Item.Complain_Subject));
                    //}
                    //else
                    //{
                    //    result = falseresult("UnAuthorized.");
                    //}
               
            }
                }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
                result = falseresult(ex.Message);
            }
            //finally
            //{
            //    _mapVer = null;
            //    _Map = null;
            //    _logMap = null;
            //    _departMap = null;
            //    _authenMap = null;
            //    _cusMap = null;
            //}
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(result));
            HttpContext.Current.Response.End();
        }

    }
}
