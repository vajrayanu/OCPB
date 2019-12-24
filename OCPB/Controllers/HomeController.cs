using OCPB.Model;
using OCPB.Models;
using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Mvc.Html;
using System.IO;
using OCPB.GCCService;
using System.Threading;
using System.Globalization;

namespace OCPB.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        #region TestFileUpload
        public void Tetfuction()
        {
            byte[] file = System.IO.File.ReadAllBytes(@"C:\Users\arm\Desktop\5_4_60.rar");
            string s3 = Convert.ToBase64String(file);
            var message = new HttpRequestMessage();
            var content = new MultipartFormDataContent();
            var parameters = new Dictionary<string, object>() { { "tokenId", "nGmTzs0rB+uw9yInpecXiFDsghl1kvHPyyhEBhphYHx22gn1LShcjo/vLf7//dGVXm/i7Y2xCBTfo3jlYiE/s2Uk27OBqHJ8BEYwMFIuvZLwLmaYTICAdt0eP73a16PBr17qdPCGarjRbz1ouZBk4JM22wHeWCzTgpOyrzPAkWtpTX0qV8VrznX8N03X8q61" }, { "Complain_Code", "OCPB0160/0083811" }, { "Description", "ไฟล์อัพโหลด" }, { "Filename", "5_4_60.rar" }, { "TypeFile", "rar" } };
            var fileContent = new ByteArrayContent(file);
            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = "5_4_60.rar" };
            content.Add(fileContent);
            message.Method = HttpMethod.Post;
            message.Content = content;
            string testUrl = "http://complain.ocpb.go.th/Temp_Ocpb/api/SaveFileUpload?";
            //    string Url = "http://localhost:45930/api/SaveFileUpload?";
            message.RequestUri = new Uri(testUrl + QueryString(parameters));
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(60);

            var _return = client.SendAsync(message).ContinueWith(task =>
            {
                if (task.Result.IsSuccessStatusCode)
                {
                    var result = task.Result;
                    Console.WriteLine(result);

                }
            }).IsCompleted; 
        }

        //public void Tetfuction2()
        //{


        //    var nvc = new List<KeyValuePair<string, string>>();

        //    nvc.Add(new KeyValuePair<string, string>("tokenId", "ycGt2mTOPDM3xw3cMnjiRIA2O+Y65Hc9ZvkcJnqKJU6XbCqJDYMyUkLCt7lCicUz4AK744mNc8Cgp1rZtAWf1jOtuaxHD7yOxpdKnrNuiOLKoJaibijz5lNz7RGgK6VCzCKu04oKFwbMB2tnON2YimvC1mFjkakwTTEXViQW3kskGqaONYYsPreIsx4YpC1h"));
        //    nvc.Add(new KeyValuePair<string, string>("Identification_number", "1739900147610"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Title", "1"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_firstname", "ชาญวิทย์"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_lastname", "แฝงคด"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_gender", "m"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Birth", "16/04/2531"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Address", "11"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Tumbol", "1"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Amphur", "2"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Province", "1"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_ZipCode", "10600"));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Tel", ""));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Tel_Ex", ""));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Mobile", ""));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Fax", ""));
        //    nvc.Add(new KeyValuePair<string, string>("Consumer_Email", "stupidnez@hotmail.com"));
        //    nvc.Add(new KeyValuePair<string, string>("Complain_Subject", "Addservice"));
        //    nvc.Add(new KeyValuePair<string, string>("Complain_Details", "Addservice"));
        //    nvc.Add(new KeyValuePair<string, string>("DefendentName", "ตัวอย่าง"));
        //    nvc.Add(new KeyValuePair<string, string>("DefendentDescription", "ตัวอย่าง"));
        //    nvc.Add(new KeyValuePair<string, string>("TYPE_0", "4"));
        //    nvc.Add(new KeyValuePair<string, string>("TYPE_1", "3"));
        //    nvc.Add(new KeyValuePair<string, string>("CASEID", "2"));
        //    nvc.Add(new KeyValuePair<string, string>("PurchaseID", "1"));
        //    nvc.Add(new KeyValuePair<string, string>("PaymentID", "2"));
        //    nvc.Add(new KeyValuePair<string, string>("MotiveID", "3"));
        //    nvc.Add(new KeyValuePair<string, string>("IsOversea", "false"));
        //    nvc.Add(new KeyValuePair<string, string>("OverseaAddress", ""));

        //    var client = new HttpClient();
        //    //var req = new HttpRequestMessage(HttpMethod.Post, "http://localhost:45930/api/MComplain") { Content = new FormUrlEncodedContent(nvc) };
        //    ////var res = await client.SendAsync(req);

        //    //var _return = client.SendAsync(req).ContinueWith(task =>
        //    //{
        //    //    if (task.Result.IsSuccessStatusCode)
        //    //    {
        //    //        var result = task.Result;
        //    //        Console.WriteLine(result);

        //    //    }
        //    //}).IsCompleted;

        //    var request = (HttpWebRequest)WebRequest.Create(defaultUrl);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";

        //    request.ContentLength = data.Length;

        //    using (var stream = request.GetRequestStream())
        //    {
        //        stream.Write(data, 0, data.Length);
        //    }

        //    var response = (HttpWebResponse)request.GetResponse();

        //    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


        //}

        #endregion
        public ActionResult Index()
        {

            //Tetfuction();
            //Tetfuction2();

      //  var test =    OCPB.httpHelper.testFunctiondata("http://localhost:45930/api/MComplain", "ycGt2mTOPDM3xw3cMnjiRIA2O+Y65Hc9ZvkcJnqKJU6XbCqJDYMyUkLCt7lCicUz4AK744mNc8Cgp1rZtAWf1jA8npn0k8UMSdVlwmz8H7I7aGAut1QJBSfwfAtEHTFyK1kuN96Y9tpdALQvOM/eqUKfu0KgrtslGcMiLBBvcmoqeHUjdqtN871ZgGY9uEFG");
            if (MYSession.Current.UserId != null)
            {
                return RedirectToAction("Index", "Manage");
            }
            return View();
        }
        public static string QueryString(IDictionary<string, object> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value);
            }
            return string.Join("&", list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(loginModel lm)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    CustomerMapDao map = new CustomerMapDao();
                    var _obj = map.FindIdentityCard(lm.Username);
                    var PEncrypt = Encryption.Encrypt(lm.Passwordd);
                    var obj = _obj.Where(o => o.Password == Encryption.Encrypt(lm.Passwordd) && o.Active == true).FirstOrDefault();
                    if (obj != null)
                    {
                        FormsAuthentication.SetAuthCookie(obj.ID.ToString(), false);
                        MYSession.Current.UserId = obj.ID.ToString();
                        MYSession.Current.UserName = obj.Fname + " " + obj.Lname; 
                        // MYSession.Current.Role = obj.level_id.ToString();
                        //return Json(new { RedirectUrl = Url.Action("Index", "Manage") });
                        return Json(new ResultData() { Status = true, text = Url.Action("Index", "Manage") }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
                return Json(new ResultData() { Status = false, text = ex.Message }, JsonRequestBehavior.AllowGet);
             }
            //return View(lm);
            return Json(new ResultData() { Status = false, text ="ข้อมูลไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
         }


        public ActionResult condition()
        {
            return View();
        }

        public PartialViewResult condition_lang()
        {
            if (Isthai())
            {
                return PartialView("_ConditionTH");
            }
            else
            {
                return PartialView("_ConditionEN");
            }
        }
        public ActionResult Nation()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Nation(IdentityCheck obj, string mode)
        {
            try
            {
                if (ModelState.IsValid)
                {  
                    CustomerMapDao map = new CustomerMapDao();
                    if (map.FindIdentityCard(obj.IdentStr).Count == 0)
                    {
                        return Json(new ResultData() { Status = true, text = Url.Action("Register", "Home", new { mode = mode, key = Encryption.Encrypt(obj.IdentStr) }) }, JsonRequestBehavior.AllowGet);
                        //return Json(new { RedirectUrl =  });
                    }
                    else
                    {
                        //return Json("Is Dupplicate!!!",JsonRequestBehavior.AllowGet );
                        return Json(new ResultData() { Status = false, text = "Is Dupplicate!!!" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
            }
            return Json(new ResultData() { Status = false, text = "ข้อมูลไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
         }
    
        public JsonResult CheckIden(string IdentityID)
        {
            CustomerMapDao map = new CustomerMapDao();
            if (map.FindIdentityCard(IdentityID.Trim()).Count == 0)
            {
                TempData["identity"] = IdentityID.UrlEnscriptHttp(); 
                return Json(new ResultData() { Status = true, text = IdentityID.UrlEnscriptHttp() }, JsonRequestBehavior.AllowGet); 
            }
            else
            { 
                return Json(new ResultData() { Status = false, text = Resources.Message.MsgDuplicate }, JsonRequestBehavior.AllowGet);
            } 
         }




        public PartialViewResult partialIden(string Model)
        {
            switch (Model)
            {
                case "Identity": Model = "_Identity"; break;
                case "Passport": Model = "_Passports"; break;
                default: Model = "_Identity"; break;
            }
            return PartialView(Model, new IdentityCheck());
        }

        public ActionResult Register(string mode, string key)
        { 
            return View(new Customer() { IsOversea = false });

        }

        public PartialViewResult BoxRegis(int? Type)
        {
             Customer obj= new Customer();
                string _partial = "";
                switch (Type)
                {
                    case 1: _partial = "_regisTH";obj.IsOversea = false; break;
                    case 2: _partial = "_regisEN"; obj.IsOversea = true;break;
                    default: _partial = "_regisTH";obj.IsOversea = false; break;
                }

            string Curture = "";
            Curture = (_partial == "_regisTH") ? "th-TH" : "en-GB";
            OCPB.Helper.CultureHelper.WriteCookie("_culture", Curture);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Curture);

            return PartialView(_partial, obj);// View(_partial, new Customer() { IdentityID = Encryption.Decrypt(key) });
        }

        public ResultData _RegisterTH(Customer obj)
        {
            CustomerMapDao map = new CustomerMapDao();
            if (map.FindIdentityCard(obj.IdentityID).Count == 0)
            {
                //obj.IdentityID = obj.IdentValid.UrlDescriptHttp();
                int ID = SaveAccount.Register(obj.IdentityID, obj.TitleID, obj.Fname, obj.Lname, obj.Sex, obj.DateOfBirthStr, obj.Address, obj.DistrictID, obj.PrefectureID, obj.ProvinceID, obj.ZipCode, obj.Tel, obj.Tel_ext, obj.Mobile, obj.Fax, obj.Email, obj.OccupationID, obj.SalaryID, obj.TypeCustomer, obj.FromApp, obj.IsOversea);
                CustomerMapDao _CusMap = new CustomerMapDao();
                obj = _CusMap.FindById(ID);
                if (!string.IsNullOrEmpty(obj.Email))
                {
                    string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Templates/regisTemplate.cshtml");
                    string html = System.IO.File.ReadAllText(filePath);
                    SendEmail.SendMail(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", string.Format(html, obj.FullNameStr, obj.IdentityID, Encryption.Decrypt(obj.Password)));
                    if (!string.IsNullOrEmpty(obj.Mobile))
                    {
                        //SmsLibs.SendSMS(obj.Mobile, SmsLibs.TypeMessage.register);
                        SmsLibs.SendSMS(obj.Mobile, string.Format(SmsLibs.CallStr(SmsLibs.TypeMessage.register), obj.IdentityID, Encryption.Decrypt(obj.Password)));
                     }



                    //SendEmail.SendMail(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("regisTemplate", obj));
                }
                return new ResultData() { Status = true };
            }
            else
            {
                return new ResultData() { Status = false, text = Resources.Message.MsgDuplicate };
            } 
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterTH(Customer obj)
        {
            try
            {
                ModelState.Remove("IdentityID");
                if (ModelState.IsValid)
                { 
                    obj.IdentityID = obj.tempIden.UrlDescriptHttp();
                    /*
                    var rst = _RegisterTH(obj);
                    if(rst.Status == true)
                    {
                    rst.text = Url.Action("Index", "Home");
                    }
                    */

                    //return Json(rst,JsonRequestBehavior.AllowGet);

                    CustomerMapDao map = new CustomerMapDao();
                    if (map.FindIdentityCard(obj.IdentityID).Count == 0)
                    {
                        //obj.IdentityID = obj.IdentValid.UrlDescriptHttp();
                        int ID = SaveAccount.Register(obj.IdentityID, obj.TitleID, obj.Fname, obj.Lname, obj.Sex, obj.DateOfBirthStr, obj.Address, obj.DistrictID, obj.PrefectureID, obj.ProvinceID, obj.ZipCode, obj.Tel, obj.Tel_ext, obj.Mobile, obj.Fax, obj.Email, obj.OccupationID, obj.SalaryID, obj.TypeCustomer, obj.FromApp, obj.IsOversea);
                        CustomerMapDao _CusMap = new CustomerMapDao();
                        obj = _CusMap.FindById(ID);
                        if (!string.IsNullOrEmpty(obj.Email))
                        {
                            SendEmail.SendMail(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("regisTemplate", obj));
                            //SendMail.Send(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("ChangePassword", obj));
                        }

                        if (!string.IsNullOrEmpty(obj.Mobile))
                        {
                            //SmsLibs.SendSMS(obj.Mobile, SmsLibs.TypeMessage.register);
                            SmsLibs.SendSMS(obj.Mobile, string.Format(SmsLibs.CallStr(SmsLibs.TypeMessage.register), obj.IdentityID, Encryption.Decrypt(obj.Password)));
                        }

                        return Json(new ResultData() { Status = true, text = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new ResultData() { Status = false, text = Resources.Message.MsgDuplicate }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                SaveUtility.logError(ex);
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult blogupdate(Customer obj)
        {
            //if (ModelState.IsValid)
            //{
                obj.IdentityID = obj.IdentityIDTemp;
                TempData["Customer"] = obj;  
                return PartialView("_RegisterEnBlog2", new Customer_Oversea());
            //}
           // return PartialView("_regisEN", obj);
        }

        public PartialViewResult EnTeb1view()
        {
            Customer Tab1 = new Customer();
            if (TempData["Customer"] != null)
            {
                Tab1 = (Customer)TempData["Customer"];
            }
            return PartialView("_RegisterEnBlog1", Tab1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterEN(Customer_Oversea obj)
        {
            if (ModelState.IsValid)
            {
                if (TempData["Customer"] != null)
                {
                    CustomerMapDao Map = new CustomerMapDao();
                    Customer Cusobj = new Customer();

                    Cusobj = (Customer)TempData["Customer"];

                    Cusobj.Keygen = Guid.NewGuid().ToString();
                    Cusobj.CreateDate = DateTime.Now;
                    Cusobj.Active = true;
                    Cusobj.IsConfirmRegister = false;
                    Cusobj.IsOversea = true;
                    Cusobj.DateOfBirth = Cusobj.DateOfBirthStr.todate();
                    //obj.Password = Helplibery.CreatePassword(8);
                    //Cusobj.Password = Encryption.Encrypt("12345678");
                    Cusobj.Password = Encryption.Encrypt(Helplibery.CreatePassword(8));
                    //Cusobj.Password = Helplibery.CreatePassword(10);

                    Map.Add(Cusobj);
                    Map.CommitChange();

                    int ID = GetAccount.GetCustomerLastID();
                    Customer_OverseaMapDao OverMap = new Customer_OverseaMapDao();
                    obj.CustomerID = ID;

                    if (obj.PurposeIList != null)
                    {
                        obj.purpose_id =  Convert.ToInt32( string.Join(",", obj.PurposeIList));
                    }

                    OverMap.Add(obj);
                    OverMap.CommitChange();

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////
                    
                    CustomerMapDao map = new CustomerMapDao();
                    if (map.FindIdentityCard(Cusobj.IdentityID).Count == 0)
                    {
                        //obj.IdentityID = obj.IdentValid.UrlDescriptHttp();
                        int CusID = SaveAccount.Register(Cusobj.IdentityID, Cusobj.TitleID, Cusobj.Fname, Cusobj.Lname, Cusobj.Sex, Cusobj.DateOfBirthStr, Cusobj.Address, Cusobj.DistrictID, Cusobj.PrefectureID, Cusobj.ProvinceID, Cusobj.ZipCode, Cusobj.Tel, Cusobj.Tel_ext, Cusobj.Mobile, Cusobj.Fax, Cusobj.Email, Cusobj.OccupationID, Cusobj.SalaryID, Cusobj.TypeCustomer, Cusobj.FromApp, Cusobj.IsOversea);
                        CustomerMapDao _CusMap = new CustomerMapDao();
                        Cusobj = _CusMap.FindById(CusID);
                        if (!string.IsNullOrEmpty(Cusobj.Email))
                        {
                            SendEmail.SendMail(Cusobj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("regisTemplate", Cusobj));
                            //SendMail.Send(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("ChangePassword", obj));
                        }
                        return Json(new ResultData() { Status = true, text = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //return Json(new ResultData() { Status = false, text = Resources.Message.MsgDuplicate }, JsonRequestBehavior.AllowGet);
                        CustomerMapDao _CusMap = new CustomerMapDao();
                        int CusID  = GetAccount.GetCustomerLastID();
                        if (!string.IsNullOrEmpty(Cusobj.Email))
                        {
                            SendEmail.SendMail(Cusobj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("regisTemplate", Cusobj));
                            //SendMail.Send(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("ChangePassword", obj));
                        }
                        return Json(new ResultData() { Status = true, text = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);

                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////

                    return Json(new ResultData() { Status = true, text = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);

                    //return Json(new { RedirectUrl = Url.Action("Index", "Home") });
                }
            }
            return View(obj);
        }

        public PartialViewResult BackTo_regisEN()
        {
            Customer Customer = new Customer();
            string _partial = "";
            int Type = 2;
            switch (Type)
            {
                case 1: _partial = "_regisTH"; Customer.IsOversea = false; break;
                case 2: _partial = "_regisEN"; Customer.IsOversea = true; break;
                default: _partial = "_regisTH"; Customer.IsOversea = false; break;
            }

            if (TempData["Customer"] != null)
            {
                Customer = (Customer)TempData["Customer"];
            }


            return PartialView(_partial, Customer);
        }



        public ActionResult LogOff()
        {
            // WebSecurity.Logout();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public PartialViewResult PView(Customer Obj)
        {
            return PartialView("ChangePassword", Obj);
        }

        public ResultData _Forget(string Identity, string Email)
        {
            if (!string.IsNullOrEmpty(Identity) && !string.IsNullOrEmpty(Email))
            {
                CustomerMapDao map = new CustomerMapDao();
                var obj = map.FindByIdentityAndEmail(Identity.Trim(), Email.Trim());
                if (obj != null)
                {
                    String newPass = Helplibery.CreatePassword(10);
                    // Method ส่ง Email
                    obj.Password = Encryption.Encrypt(newPass);
                    map.AddOrUpdate(obj);
                    map.CommitChange();
                    Log_Customer_reset_passMapDao logmap = new Log_Customer_reset_passMapDao();
                    logmap.Add(new Log_Customer_reset_pass { CreateDate = DateTime.Now, EmailTo = Email, ErrorText = "", IPAddress = Extension.GetIPAddress(), Result = true });
                    logmap.CommitChange();
                    if (!string.IsNullOrEmpty(Email))
                    {
 
                      //  string ttt = System.Web.Mvc.Html.PartialExtensions.Partial("ChangePassword", obj);
                        string filePath = Path.Combine(HttpRuntime.AppDomainAppPath, "Templates/ChgPass.htm");
                        string html = System.IO.File.ReadAllText(filePath);
                        SendEmail.SendMail(Email, "แก้ไขรหัสผ่านสํานักงานคณะกรรมการคุ้มครองผู้บริโภค", string.Format(html,obj.FullNameStr,obj.IdentityID, Encryption.Decrypt(obj.Password),obj.Email));
                        //SendMail.Send(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("ChangePassword", obj));
                    }
                    return new ResultData() { Status = true, text = "รหัสผ่านใหม่ ถูกจัดส่งไปยังอีเมลของท่าน เรียบร้อยแล้ว" };
                }
                else
                {
                    return new ResultData() { Status = false, text = "ข้อมูลไม่ถูกต้อง" };
                }
            }
            else
            {
                return new ResultData() { Status = false, text = "กรุณากรอกข้อมูลให้ครบถ้วน" };
            }
        }
         

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ForgotPass(string Identity, string Email)
        {
            return Json(_Forget(Identity,Email));

            //if (!string.IsNullOrEmpty(Identity) && !string.IsNullOrEmpty(Email))
            //{
            //    CustomerMapDao map = new CustomerMapDao();
            //    var obj = map.FindByIdentityAndEmail(Identity.Trim(), Email.Trim());
            //    if (obj != null)
            //    {
            //        String newPass = Helplibery.CreatePassword(10);
            //        // Method ส่ง Email
            //        obj.Password = Encryption.Encrypt(newPass);
            //         map.AddOrUpdate(obj);
            //        map.CommitChange();
            //        Log_Customer_reset_passMapDao logmap = new Log_Customer_reset_passMapDao();
            //        logmap.Add(new Log_Customer_reset_pass { CreateDate = DateTime.Now, EmailTo = Email, ErrorText = "", IPAddress = Extension.GetIPAddress(), Result = true });
            //        logmap.CommitChange();
            //        if (!string.IsNullOrEmpty(Email))
            //        {
            //            SendEmail.SendMail(Email, "แก้ไขรหัสผ่านสํานักงานคณะกรรมการคุ้มครองผู้บริโภค", RenderPartialViewToString("ChangePassword", obj));
            //            //SendMail.Send(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("ChangePassword", obj));
            //        }
            //        return Json(new ResultData() { Status = true, text = "รหัสผ่านใหม่ ถูกจัดส่งไปยังอีเมลของท่าน เรียบร้อยแล้ว" });
            //    }
            //    else
            //    {
            //        return Json(new ResultData() { Status = false, text = "ข้อมูลไม่ถูกต้อง" });
            //    }
            //}
            //else
            //{

            //    return Json(new ResultData() { Status = false, text = "กรุณากรอกข้อมูลให้ครบถ้วน" });
            //}
        }
    }
}
