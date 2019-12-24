using OCPB.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using OCPB.Model;
using OCPB.Repository.Repositories;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OCPB.Models;
using System.IO;
using System.Web.Routing;
using System.Text;
using System.Web.UI;
using OCPB.Library;
using System.Globalization;

namespace OCPB.Controllers
{
    public class BaseController : Controller
    {

        //protected void Application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    string culture = "en-US";
        //    if (Request.UserLanguages != null)
        //    {
        //        culture = Request.UserLanguages[0];
        //    }
        //    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
        //    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
        //}


        public class CustomAuthorize : AuthorizeAttribute
        {
            public int MenuID { get; set; }
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                //var obj = GetUtility.GetMenu_userID(MYSession.Current.User_Id).Where(o => o.MenuID == MenuID).FirstOrDefault();
                //var Url = new UrlHelper(filterContext.RequestContext); 
                //if (obj == null)
                //{ 
                //    var url = Url.Action("Index", "Index");
                //    filterContext.Result = new RedirectResult(url);
                //}
                //else if (MYSession.Current.User_Id == null)
                //{
                //    var url = Url.Action("Index", "Login");
                //    filterContext.Result = new RedirectResult(url);
                //}
                //else
                //{
                //    BaseController.PermissionMenu = obj;
                //} 
                var Url = new UrlHelper(filterContext.RequestContext);
                if (MYSession.Current.UserId == null)
                {
                    var url = Url.Action("Index", "Home");
                    filterContext.Result = new RedirectResult(url);
                }
                base.OnAuthorization(filterContext);
            }
        }

        public bool Isthai()
        { 
            return Cookie() == "th-TH"?true:false;
        }

        protected string RenderPartialViewToString2(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("Home");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
        public JsonResult UpdateCodeResult(string Status, string Message, string RegisterNo, string RegisterDateTime, int Key)
        {
            try
            {
                if (Status == "successful")
                {
                    ComplainsMapDao Map = new ComplainsMapDao();
                    var Obj = Map.FindById(Key);
                    Obj.Complain_Code = DateTime.Now.ToString("yyyy") + "/" + RegisterNo;
                    Map.AddOrUpdate(Obj);
                    Map.CommitChange();
                    Log_sarabunMapDap LMap = new Log_sarabunMapDap();
                    Log_sarabun _lObj = new Log_sarabun();
                    _lObj.Status = Status;
                    _lObj.Message = Message;
                    _lObj.RegisterNo = RegisterNo;
                    _lObj.RegisterDateTime = RegisterDateTime;
                    _lObj.GUID = Obj.Keygen;
                    _lObj.CreateDate = DateTime.Now;
                    LMap.AddOrUpdate(_lObj);
                    LMap.CommitChange();
                    return Json(new { success = true, Code = Obj.Complain_Code }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, Code = Status }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                SaveUtility.logError(ex);
                return Json(new { success = false, Code = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Ampher(string ProvnceID)
        {
            Tm_prefectureMapDao map = new Tm_prefectureMapDao();
            List<Tm_prefecture> obj = new List<Tm_prefecture>();
            if (!string.IsNullOrEmpty(ProvnceID))
            {
                BaseController BaseObj = new BaseController();
                var _obj = map.FindAll().Where(o => o.Province_ID == ProvnceID.Toint())
                           .Select(o => new { o.ID ,  label =
                            (BaseObj.Cookie() == "th-TH") ? o.Prefecture_NameTH : o.Prefecture_NameEN
                            }).ToList();
                return Json(_obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
         public static SelectList Title()
        {
            User_TitleMapDao map = new User_TitleMapDao();
            List<User_Title> obj = new List<User_Title>();
            BaseController BaseObj = new BaseController();
            if (BaseObj.Cookie() == "th-TH")
            {
                return new SelectList(map.FindByActiveAndTH(), "ID", "Description");
            }
            else
            {
                return new SelectList(map.FindByActiveAndEN(), "ID", "Description");
            }

        }
        public static SelectList ContinentsItems()
        {
            ContinentsMapDao map = new ContinentsMapDao();
            List<Continents> obj = new List<Continents>();
            return new SelectList(map.FindAll(), "code", "name");
        }
        public static SelectList CountriesItems()
        {
            CountriesMapDao map = new CountriesMapDao();
            List<Countries> obj = new List<Countries>();
            return new SelectList(map.FindAll(), "continent_code", "full_name");
        }
        public static SelectList Occupation()
        {
            Tm_OccupationMapDao map = new Tm_OccupationMapDao();
            List<Tm_Occupation> obj = new List<Tm_Occupation>();
            return new SelectList(map.FindByActive(), "ID", LangSelect("Occupation_Name","occupation_name_en"));
         }

         public static SelectList Salary()
        {
            Tm_SalaryMapDao map = new Tm_SalaryMapDao();
            List<Tm_Salary> obj = new List<Tm_Salary>();
            return new SelectList(map.FindByActive(), "ID", LangSelect("SalaryName", "SalaryNameEN"));
        }
        public static SelectList Ampher(int? ProvnceID)
        {
            Tm_prefectureMapDao map = new Tm_prefectureMapDao();
            List<Tm_prefecture> obj = new List<Tm_prefecture>();
            if (ProvnceID != null)
            {
                return new SelectList(map.FindAll().Where(o => o.Province_ID == ProvnceID).ToList(), "ID",LangSelect( "Prefecture_NameTH", "Prefecture_NameEN"));
            }
            else
            {
                return new SelectList(obj, "ID", LangSelect("Prefecture_NameTH", "Prefecture_NameEN"));
            }
        }
        public JsonResult Tumbon(string amphur_id)
        {
            Tm_districtMapDao map = new Tm_districtMapDao();
            List<Tm_district> obj = new List<Tm_district>();

            if (!string.IsNullOrEmpty(amphur_id))
            {
                BaseController BaseObj = new BaseController();
                //var _obj = map.FindAll().Where(o => o.Prefecture_ID == amphur_id.Toint()).ToList();
                var _obj = map.FindAll().Where(o => o.Prefecture_ID == amphur_id.Toint())
                        .Select(o => new
                        {
                            o.ID,
                            label =
                                (BaseObj.Cookie() == "th-TH") ? o.District_NameTH : o.District_NameEN
                        }).ToList();
                return Json(_obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public static SelectList Tumbon(int? amphur_id)
        {
            Tm_districtMapDao map = new Tm_districtMapDao();
            List<Tm_district> obj = new List<Tm_district>();

            if (amphur_id != null)
            {
                return new SelectList(map.FindAll().Where(o => o.Prefecture_ID == amphur_id).ToList(), "ID", LangSelect("District_NameTH", "District_NameEN"));
            }
            else
            {
                return new SelectList(obj, "ID", LangSelect("District_NameTH", "District_NameEN"));
            }
        }
        public JsonResult Zip(int district_id)
        {
            Tm_districtMapDao map = new Tm_districtMapDao();
            return Json(map.FindById(district_id).Zipcode, JsonRequestBehavior.AllowGet);
        }

        public static SelectList ProvinceSe()
        {
            return new SelectList(FindProvinceTHList(), "ID", LangSelect("Province_NameTH", "Province_NameEN"));
        }
        private static List<Tm_province> FindProvinceTHList()
        {
            Tm_provinceMapDao _pro = new Tm_provinceMapDao();
            return _pro.FindByActive().ToList();

        }
        private static List<Tm_prefecture> FindprefectureList()
        {
            Tm_prefectureMapDao _city = new Tm_prefectureMapDao(); ;
            List<Tm_prefecture> objcity = _city.FindByActive();
            return objcity;
        }
        private static List<Tm_district> FinddistrictList()
        {
            Tm_districtMapDao _dis = new Tm_districtMapDao();
            List<Tm_district> objDis = _dis.FindByActive();
            return objDis;
        }

        public static List<Tm_PlaceOfPurchase> Tm_PlaceOfPurchaseLsit()
        {
            Tm_PlaceOfPurchaseMapDao map = new Tm_PlaceOfPurchaseMapDao();
            return map.FindByActive();
        }

        public static List<Tm_Purpose> Get_Tm_Purpose_Lsit()
        {
            Tm_PurposeMapDao map = new Tm_PurposeMapDao();
            return map.FindByActive();
        }

        public static List<Tm_purepose> Get_Tm_purepose_Lsit()
        {
            Tm_pureposeMapDao map = new Tm_pureposeMapDao();
            return map.FindByActive();
        }
         
        public static List<Tm_Motivate> Tm_Motivatesit()
        {
            Tm_MotivateMapDao map = new Tm_MotivateMapDao();
            return map.FindByActive();
        }
        public static List<Tm_Payment> Tm_PaymentList()
        {
            Tm_PaymentMapDao map = new Tm_PaymentMapDao();
            return map.FindByActive();
        }
        public static List<Tm_Type_Attach> Tm_Type_AttachList()
        {
            Tm_Type_AttachMapDao map = new Tm_Type_AttachMapDao();
            return map.FindByActive();
        }

        
        public static List<Tm_Complain_Type> Tm_Complain_Type0()
        { 
            Tm_Complain_TypeMapDao map = new Tm_Complain_TypeMapDao();
            return map.FindByLevel0();
        }

        public static List<Tm_Complain_Type> Tm_Complain_Type1(int refID)
        {
            Tm_Complain_TypeMapDao map = new Tm_Complain_TypeMapDao();
            return map.FindByLevel1(refID);
        }

        private static string LangSelect(string ValueTH, string ValueEN)
        {
            BaseController obj = new BaseController();
            return (obj.Cookie() == "th-TH") ? ValueTH : ValueEN;
        }
        #region SetUp language
        //
        // GET: /Base/
        private static string defaultculture = ConfigurationManager.AppSettings["CultureTH"]; 
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string lang = Request.QueryString["lang"];
            //if (Url.RequestContext.RouteData.Values["lang"] != null)
            if ( !(string.IsNullOrEmpty(lang)) )
            {
                //string Curture = Url.RequestContext.RouteData.Values["lang"].ToString();
                string Curture = lang;
                Curture = (Curture.ToUpper() == "TH") ? "th-TH" : "en-GB";
                CultureHelper.WriteCookie("_culture", Curture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Curture);
            }
            Cookie();
            var routeValueDictionary = Url.RequestContext.RouteData.Values;
            if (routeValueDictionary["id"] == null)
            {
                ViewBag.thurl = Url.Action(routeValueDictionary["action"].ToString(), routeValueDictionary["controller"].ToString(), new { lang = "TH" });
                ViewBag.enurl = Url.Action(routeValueDictionary["action"].ToString(), routeValueDictionary["controller"].ToString(), new { lang = "EN" });
            }
            else
            {
                ViewBag.thurl = Url.Action("Index", "Home", new { lang = "TH" });
                ViewBag.enurl = Url.Action("Index", "Home", new { lang = "EN" });
            }
            //DatetimeSet(); 
            return base.BeginExecuteCore(callback, state);
        }
        public string Cookie()
        {
            string _Cookie = CultureHelper.ReadCookie("_culture");
            if (_Cookie == null)
            {
                CultureHelper.WriteCookie("_culture", defaultculture);
                _Cookie = CultureHelper.ReadCookie("_culture");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(_Cookie);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(_Cookie);

            return _Cookie;
        }
        #endregion
  
            protected void updatePassAndEnscrypt()
            {
                CustomerMapDao map = new CustomerMapDao();
                foreach (var I in map.FindAll().Where(o => o.IdentityID != null || o.IdentityID != "").ToList())
                {
                    I.Password = Encryption.Encrypt(I.IdentityID);
                    map.AddOrUpdate(I); 
                }
                map.CommitChange();
            }
        #region Utility แสดงข้อมูล
        [EncryptedActionParameter]
            public PartialViewResult ShowLog(string ID)
            {
                ComplainsMapDao map = new ComplainsMapDao();
                var obj = map.FindByKeygen(ID).FirstOrDefault();
                return PartialView("Utility/_ShowLog", obj); 
            }
            public PartialViewResult PopupDetailSub(int trackID)
            {
                List<OCPB.ModelsComplain.Get_log_History_DetailModel> Obj = new List<ModelsComplain.Get_log_History_DetailModel>();
                Obj = GetComplain.Get_log_History_Detail(trackID);
                if (Obj.Count() == 0)
                {
                    Obj.Add(new OCPB.ModelsComplain.Get_log_History_DetailModel { Send_Date = null, StatusTH = "รอตรวจสอบเรื่องร้องทุกข์" });
                }
                return PartialView("Utility/_ShowLogSub", Obj);
            }

            // Show ข้อมูล รายละเอียดเรื่องร้องทุกข์
            public PartialViewResult PopupDetailComplains(string ID, String Key)
            {
                ComplainsMapDao Map = new ComplainsMapDao(); 
                var obj = Map.FindByIDAndKeygen(int.Parse(ID.UrlDescriptHttp()), Key.UrlDescriptHttp());
                if (obj != null)
                {
                    return PartialView("Utility/_details", obj);
                }
                else
                {
                    return PartialView("Utility/Noresult", obj);
                }
            }
        [EncryptedActionParameter]
        public PartialViewResult VwCancle(string Keygen)
            {
                ComplainsMapDao map = new ComplainsMapDao(); 
                var obj = map.FindByKeygen(Keygen).FirstOrDefault();


                CustomerMapDao Cusmap = new CustomerMapDao();
                var Cusobj = Cusmap.FindById(obj.CustomerID.Toint());
                ViewBag.Customer = Cusobj;

                return PartialView("_VwCancle", obj);
            }

        #endregion
            #region prints reports
            private PdfPTable GetReports()
            {
                var rows1 = ItextCharpLib.Col1Config();
                string picpath = HttpContext.Server.MapPath("~/images/logo58-Red(500x500).png");
                var ImgHeader = ItextCharpLib.ImgConfig_center(60, 60, picpath);
                rows1.AddCell(ImgHeader);
                return rows1;
            }
            private PdfPTable GetHeaderDetail(string Complain_Code_ID, string Description, string Complain_Date, string Complain_Subject, string FullNameStr)
            {
                string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
                string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
                BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font h1 = new Font(bf_bold, 18);
                Font bold = new Font(bf_bold, 14);
                Font smallBold = new Font(bf_bold, 14);
                BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


                Font normal = new Font(bf_normal, 14);
                Font smallNormal = new Font(bf_normal, 14);

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = 530f;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 10;
                table.SpacingAfter = 10;

                float[] tableWidths = new float[3];
                tableWidths[0] = 150f;
                tableWidths[1] = 400f;
                tableWidths[2] = 500f;

                table.SetWidths(tableWidths);
                table.LockedWidth = true;


                Chunk blank = new Chunk(" ", normal);

                #region Rows1

                Phrase p = new Phrase();
                p.Add(new Chunk(blank));
                PdfPCell cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(blank));
                PdfPCell cell1 = new PdfPCell(p);
                cell1.Border = Rectangle.NO_BORDER;
                //  cell1.FixedHeight = 30f;
                table.AddCell(cell1);

                p = new Phrase();
                p.Add(new Chunk("เลขรับแจ้ง", bold));
                p.Add(new Chunk(blank));
                p.Add(new Chunk(Complain_Code_ID, normal));
                PdfPCell cell2 = new PdfPCell(p);
                cell2.Border = Rectangle.NO_BORDER;
                cell2.HorizontalAlignment = Element.ALIGN_RIGHT;

                table.AddCell(cell2);
                #endregion
                #region Rows2


                p = new Phrase();
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(blank));
                cell1 = new PdfPCell(p);
                cell1.Border = Rectangle.NO_BORDER;
                // cell1.FixedHeight = 30f;
                table.AddCell(cell1);

                p = new Phrase();
                p.Add(new Chunk("ช่องทางเรื่องร้องทุกข์", bold));
                p.Add(new Chunk(blank));
                p.Add(new Chunk(Description, normal));
                cell1 = new PdfPCell(p);
                cell1.Border = Rectangle.NO_BORDER;
                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell1.PaddingTop = 5;
                //  cell1.FixedHeight = 30f;
                table.AddCell(cell1);
                #endregion
                #region Rows3
                p = new Phrase();
                p.Add(new Chunk("วันที่บันทึก", bold));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //   cell0.FixedHeight = 30f;
                table.AddCell(cell0);




                p = new Phrase();
                p.Add(new Chunk(Complain_Date, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = 2;
                //cell0.PaddingTop = 5;
                //   cell0.FixedHeight = 30f;
                table.AddCell(cell0);


                #endregion
                #region Rows4
                p = new Phrase();
                p.Add(new Chunk("วันที่รับเรื่อง", bold));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                // cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(Complain_Date, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = 2;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);
                #endregion
                #region Rows5
                p = new Phrase();
                p.Add(new Chunk("หัวข้อเรื่องร้องทุกข์", bold));
                p.Add(new Chunk(blank));
                p.Add(new Chunk(Complain_Subject, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = 3;
                //    cell0.FixedHeight = 30f;
                table.AddCell(cell0);
                #endregion
                #region Rows6
                p = new Phrase();
                p.Add(new Chunk("ผู้ร้องเรียน", bold));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                // cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(FullNameStr, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = 2;
                cell0.PaddingTop = 5;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);
                #endregion








                PdfPTable tableSub = new PdfPTable(3);
                tableSub.TotalWidth = 530f;
                tableSub.HorizontalAlignment = 0;
                tableSub.SpacingAfter = 10;
                tableWidths = new float[3];
                tableWidths[0] = 500f;
                tableWidths[1] = 500f;
                tableWidths[2] = 500f;
                tableSub.SetWidths(tableWidths);
                tableSub.LockedWidth = true;



                p = new Phrase();
                p.Add(new Chunk("ตำบล", bold));
                p.Add(new Chunk(blank));
                p.Add(new Chunk("บางยี่เรือ", normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                // cell0.Colspan = 2; 
                tableSub.AddCell(cell0);


                p = new Phrase();
                p.Add(new Chunk("อำเภอ", bold));
                p.Add(new Chunk(blank));
                p.Add(new Chunk("เขตธนบุรี", normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                tableSub.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk("จังหวัด", bold));
                p.Add(new Chunk(blank));
                p.Add(new Chunk("กรุงเทพมหานคร", normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                tableSub.AddCell(cell0);


                table.AddCell(tableSub);


                //p = new Phrase();
                //p.Add(new Chunk("ที่อยู่ที่ติดต่อได้", bold));
                //p.Add(new Chunk(blank));
                //p.Add(new Chunk("100 ริมทางรถไฟ", normal));
                //cell0 = new PdfPCell(p);
                //cell0.Border = Rectangle.NO_BORDER;
                //cell0.Colspan = 2;
                //table.AddCell(cell0);



                return table;


            }
            private PdfPTable GetAddressDetail(string Address, string District_NameTH, string Prefecture_NameTH, string Province_NameTH)
            {
                string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
                string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
                BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font h1 = new Font(bf_bold, 18);
                Font bold = new Font(bf_bold, 14);
                Font smallBold = new Font(bf_bold, 14);
                BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


                Font normal = new Font(bf_normal, 14);
                Font smallNormal = new Font(bf_normal, 14);

                PdfPTable table = new PdfPTable(6);
                table.TotalWidth = 530f;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 10;
                table.SpacingAfter = 10;
                #region Rows8 new table

                float[] columnWidths = new float[6];
                columnWidths[0] = 88f;
                columnWidths[1] = 88f;
                columnWidths[2] = 88f;
                columnWidths[3] = 88f;
                columnWidths[4] = 88f;
                columnWidths[5] = 88f;
                table.SetWidths(columnWidths);
                table.LockedWidth = true;

                #region Rows7


                PdfPCell cell0 = new PdfPCell(new Phrase("ผู้ที่อยู่ที่ติดต่อได้", bold));
                cell0.HorizontalAlignment = Element.ALIGN_LEFT;
                cell0.Border = Rectangle.NO_BORDER;
                //   cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                PdfPCell cell1 = new PdfPCell(new Phrase(Address, normal));
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1.Border = Rectangle.NO_BORDER;
                cell1.Colspan = 5;
                // cell1.FixedHeight = 30f;
                table.AddCell(cell1);


                #endregion



                cell0 = new PdfPCell(new Phrase("ตำบล", bold));
                cell0.HorizontalAlignment = Element.ALIGN_LEFT;
                cell0.Border = Rectangle.NO_BORDER;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                cell1 = new PdfPCell(new Phrase(District_NameTH, normal));
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                cell1.Border = Rectangle.NO_BORDER;
                //  cell1.FixedHeight = 30f;
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase("อำเภอ", bold));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                //   cell2.FixedHeight = 30f;
                table.AddCell(cell2);


                cell2 = new PdfPCell(new Phrase(Prefecture_NameTH, normal));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                table.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase("จังหวัด", bold));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                table.AddCell(cell2);

                cell2 = new PdfPCell(new Phrase(Province_NameTH, normal));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                cell2.Border = Rectangle.NO_BORDER;
                table.AddCell(cell2);
                #endregion

                return table;
            }
            private PdfPTable GetContackDetail(string Tel, string Mobile, string Fax, string Email)
            {
                string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
                string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
                BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font h1 = new Font(bf_bold, 18);
                Font bold = new Font(bf_bold, 14);
                Font smallBold = new Font(bf_bold, 14);
                BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


                Font normal = new Font(bf_normal, 14);
                Font smallNormal = new Font(bf_normal, 14);

                PdfPTable table = new PdfPTable(4);
                table.TotalWidth = 530f;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 10;
                table.SpacingAfter = 10;
                float[] tableWidths = new float[4];
                tableWidths[0] = 130f;
                tableWidths[1] = 100f;
                tableWidths[2] = 100f;
                tableWidths[3] = 100f;
                table.SetWidths(tableWidths);
                table.LockedWidth = true;


                Chunk blank = new Chunk(" ", normal);

                #region Rows1
                Phrase p = new Phrase();
                p.Add(new Chunk("เบอร์โทรศัพท์", bold));
                PdfPCell cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(Tel, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                // cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk("มือถือ", bold));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                // cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(Mobile, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);
                #endregion
                #region Rows1
                p = new Phrase();
                p.Add(new Chunk("แฟกซ์", bold));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //   cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(Fax, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                // cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(Resources.OCPB.Email, bold));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);

                p = new Phrase();
                p.Add(new Chunk(Email, normal));
                cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);
                #endregion
                return table;


            }
            private enum FonSize
            {
                h1,
                bold,
                smallBold,
                normal,
                smallNormal
            }
            private Font SizeFont(FonSize obj)
            {
                string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
                string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
                BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font _font = new Font(bf_bold, 18);
                // Font h1 = new Font(bf_bold, 18);
                //Font bold = new Font(bf_bold, 16);
                //Font smallBold = new Font(bf_bold, 14);

                //Font normal = new Font(bf_normal, 16);
                //Font smallNormal = new Font(bf_normal, 14);
                switch (obj.ToString())
                {
                    case "h1": _font = new Font(bf_bold, 18); break;
                    case "bold": _font = new Font(bf_bold, 14); break;
                    case "smallBold": _font = new Font(bf_bold, 14); break;
                    case "normal": _font = new Font(bf_normal, 14); break;
                    case "smallNormal": _font = new Font(bf_normal, 14); break;
                }
                return _font;
            }
            private PdfPCell Cellnormal(string _value, FonSize _font)
            {
                Phrase p = new Phrase();
                p.Add(new Chunk(_value, SizeFont(_font)));
                PdfPCell cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                //  cell0.FixedHeight = 30f;
                return cell0;
            }
            private PdfPTable GetDetail(string Complain_Type_Name, string Complain_Type_Name_Sub, string CasenameTH, string Complain_Details, string PlaceNameThai, string MotiveNameThai, string PaymentNameThai)
            {
                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 500f;
                table.HorizontalAlignment = 0;
                table.SpacingAfter = 10;
                float[] tableWidths = new float[2];
                tableWidths[0] = 140f;
                tableWidths[1] = 360f;
                table.SetWidths(tableWidths);
                table.LockedWidth = true;


                Chunk blank = new Chunk(" ", SizeFont(FonSize.bold));

                #region Rows1
                table.AddCell(CellnormalColspan_text_left("เรื่องร้องทุกข์", FonSize.bold, 1));
                table.AddCell(Cellnormal(Complain_Type_Name, FonSize.normal));
                #endregion
                #region Rows2
                table.AddCell(CellnormalColspan_text_left("ประเภทร้องทุกข์", FonSize.bold, 1));
                table.AddCell(Cellnormal(Complain_Type_Name_Sub, FonSize.normal));

                #endregion
                #region Rows3
                table.AddCell(CellnormalColspan_text_left("สาเหตุเรื่องร้องทุกข์", FonSize.bold, 1));
                table.AddCell(Cellnormal(CasenameTH, FonSize.normal));
                #endregion
                #region Rows4
                table.AddCell(CellnormalColspan_text_left("สถานที่ซื้อ หรือรับบริการ", FonSize.bold, 1));
                table.AddCell(Cellnormal(PlaceNameThai, FonSize.normal));
                #endregion
                #region Rows5
                table.AddCell(CellnormalColspan_text_left("มูลเหตุจูงใจที่ซื้อ หรือรับบริการ", FonSize.bold, 1));
                table.AddCell(Cellnormal(MotiveNameThai, FonSize.normal));
                #endregion
                #region Rows6
                table.AddCell(CellnormalColspan_text_left("วิธีการชำระเงิน", FonSize.bold, 1));
                table.AddCell(Cellnormal(PaymentNameThai, FonSize.normal));
                #endregion
                #region Rows7
                table.AddCell(CellnormalColspan_text_left("รายละเอียดเรื่องร้องเรียน", FonSize.bold, 1));
                table.AddCell(Cellnormal(Complain_Details, FonSize.normal));

                #endregion


                return table;
            }
            private PdfPTable GetAttachDetail()
            {

                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 530f;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 10;
                table.SpacingAfter = 10;


                float[] columnWidths = new float[2];
                columnWidths[0] = 88f;
                columnWidths[1] = 442f;
                table.SetWidths(columnWidths);
                table.LockedWidth = true;

                Chunk blank = new Chunk(" ", SizeFont(FonSize.bold));

                #region Rows1
                table.AddCell(Cellnormal(" ", FonSize.bold));
                table.AddCell(Cellnormal("1.บัตรประชาชน", FonSize.normal));
                #endregion

                #region Rows1
                table.AddCell(Cellnormal(" ", FonSize.bold));
                table.AddCell(Cellnormal("1.บัตรประชาชน", FonSize.normal));
                #endregion

                #region Rows1
                table.AddCell(Cellnormal(" ", FonSize.bold));
                table.AddCell(Cellnormal("1.บัตรประชาชน", FonSize.normal));
                #endregion

                return table;
            }
            private PdfPTable GetBottoms()
            {

                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 530f;
                table.HorizontalAlignment = 0;
                table.SpacingBefore = 10;
                table.SpacingAfter = 10;


                float[] columnWidths = new float[2];
                columnWidths[0] = 250f;
                columnWidths[1] = 250f;
                table.SetWidths(columnWidths);
                table.LockedWidth = true;

                Chunk blank = new Chunk(" ", SizeFont(FonSize.bold));
                #region Rows4


                Phrase p = new Phrase();
                p.Add(new Chunk("     ข้าพเจ้าขอรับรองว่าข้อเท็จจริงที่ได้ยื่นเรื่องร้องทุกข์ต่อสำนักงานคณะกรรมการคุ้มครองผู้บริโภคเป็นเรื่องที่ เกิดขึ้นจริงทั้งหมดและขอรับผิดชอบต่อข้อเท็จจริง ดังกล่าวข้างต้นทุกประการ", SizeFont(FonSize.h1)));
                PdfPCell cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = 2;
                //  cell0.FixedHeight = 30f;
                table.AddCell(cell0);
                #endregion



                return table;
            }
            private PdfPCell CellnormalColspan(string _value, FonSize _font, int span)
            {
                Phrase p = new Phrase();
                p.Add(new Chunk(_value, SizeFont(_font)));
                PdfPCell cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = span;
                cell0.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell0.FixedHeight = 30f;
                return cell0;
            }
            private PdfPCell CellnormalColspan_text_RIGHT(string _value, FonSize _font, int span)
            {
                Phrase p = new Phrase();
                p.Add(new Chunk(_value, SizeFont(_font)));
                PdfPCell cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = span;
                cell0.HorizontalAlignment = Element.ALIGN_RIGHT;
                //cell0.FixedHeight = 30f;
                return cell0;
            }
            private PdfPCell CellnormalColspan_text_left(string _value, FonSize _font, int span)
            {
                Phrase p = new Phrase();
                p.Add(new Chunk(_value, SizeFont(_font)));
                PdfPCell cell0 = new PdfPCell(p);
                cell0.Border = Rectangle.NO_BORDER;
                cell0.Colspan = span;
                cell0.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell0.FixedHeight = 30f;
                return cell0;
            }

            private PdfPTable GetHeaderSignature2(string CustomerName)
            {

                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 530f;
                table.HorizontalAlignment = 0;
                table.SpacingAfter = 10;
                float[] tableWidths = new float[2];
                tableWidths[0] = 200f;
                tableWidths[1] = 250f;
                table.SetWidths(tableWidths);
                table.LockedWidth = true;
                table.AddCell(CellnormalColspan(" ", FonSize.normal, 2));

                var Cell1 = CellnormalColspan("ลงชื่อ.........................................ผู้ร้องทุกข์", FonSize.normal, 1);
                Cell1.Top = 35f;
                table.AddCell(Cell1);
                table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

                Cell1 = CellnormalColspan("(   " + CustomerName + "  )", FonSize.normal, 1);
                table.AddCell(Cell1);
                table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

                table.AddCell(CellnormalColspan(" ", FonSize.normal, 2));

                Cell1 = CellnormalColspan("       ลงชื่อ...........................................เจ้าหน้าที่เจ้าของเรื่อง", FonSize.normal, 1);
                table.AddCell(Cell1);
                table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

                Cell1 = CellnormalColspan("(..................................................)", FonSize.normal, 1);
                table.AddCell(Cell1);
                table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));
                return table;
            }

            public ActionResult Prints(string ID, string Key)
            {
                GetComplain_reports_DetailModel obj = GetUtility.GetComplain_reports_Detail(ID.UrlDescriptHttp().Toint(), Key.UrlDescriptHttp());
                GetCustomer_Reports_DetailModel objCustomer = GetUtility.GetCustomer_Reports_Detail(obj.CustomerID);
                // Create PDF document
                Document pdfDoc = new Document(PageSize.A4, 30, 30, 20, 20);
                PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();
                pdfDoc.Add(GetReports());
                pdfDoc.Add(GetHeaderDetail(obj.Complain_Code_ID, obj.Description, obj.Complain_Date_str, obj.Complain_Subject, objCustomer.FullNameStr));
                pdfDoc.Add(GetAddressDetail(objCustomer.Address, objCustomer.District_NameTH, objCustomer.Prefecture_NameTH, objCustomer.Province_NameTH));
                pdfDoc.Add(GetContackDetail(objCustomer.Tel, objCustomer.Mobile, objCustomer.Fax, objCustomer.Email));
                pdfDoc.Add(GetDetail(obj.Complain_Type_Name, obj.Complain_Type_Name_Sub, obj.CasenameTH, obj.Complain_Details, obj.PlaceNameThai, obj.MotiveNameThai, obj.PaymentNameThai));
                // pdfDoc.Add(GetAttachDetail()); 
                pdfDoc.Add(GetBottoms());
                pdfDoc.Add(GetHeaderSignature2(objCustomer.FullNameStr));

                //LineSeparator line = new LineSeparator();

                //pdfDoc.Add(line);

                pdfWriter.CloseStream = false;
                pdfDoc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + obj.Complain_Code_ID + ".pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();

                //เลข 4 ตัวหลัง layout ของกระดาษคือระยะขอบครับ
                //Document pdfDoc = new Document(PageSize.A4, 30, 30, 20, 20);
                //PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //pdfDoc.Open();

                // Bold
                //BaseFont bf_bold = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //h1 = new Font(bf_bold, 18);
                //bold = new Font(bf_bold, 16);
                //smallBold = new Font(bf_bold, 14);

                // Normal
                //BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                //normal = new Font(bf_normal, 16);
                //smallNormal = new Font(bf_normal, 14);





                //var obj = GetUtility.Getreports_SummaryData();
                //obj.WriteXSD("Tab1");
                //ExportsDoc.FileExportsName = "รายงานการดำเนินการเรื่องร้องทุกข์";
                //ExportsDoc.CreateReportForm("reports_Tab1", ExportsDoc.TypeExports.EXCEL, obj);  
                return View();
            }


            #endregion


            //#region prints reports
            //private PdfPTable GetReports()
            //{
            //    var rows1 = ItextCharpLib.Col1Config();
            //    string picpath = HttpContext.Server.MapPath("~/images/logo58-Red(500x500).png");
            //    var ImgHeader = ItextCharpLib.ImgConfig_center(  60, 60, picpath);
            //    rows1.AddCell(ImgHeader);
            //    return rows1;
            //}
            //private PdfPTable GetHeaderDetail(string Complain_Code_ID, string Description, string Complain_Date, string Complain_Subject, string FullNameStr)
            //{
            //    string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
            //    string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
            //    BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //    Font h1 = new Font(bf_bold, 18);
            //    Font bold = new Font(bf_bold, 16);
            //    Font smallBold = new Font(bf_bold, 14);
            //    BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


            //    Font normal = new Font(bf_normal, 16);
            //    Font smallNormal = new Font(bf_normal, 14);

            //    PdfPTable table = new PdfPTable(3);
            //    table.TotalWidth = 530f;
            //    table.HorizontalAlignment = 0;
            //    table.SpacingBefore = 10;
            //    table.SpacingAfter = 10;

            //    float[] tableWidths = new float[3];
            //    tableWidths[0] = 150f;
            //    tableWidths[1] = 400f;
            //    tableWidths[2] = 500f;

            //    table.SetWidths(tableWidths);
            //    table.LockedWidth = true;


            //    Chunk blank = new Chunk(" ", normal);

            //    #region Rows1

            //    Phrase p = new Phrase();
            //    p.Add(new Chunk(blank));
            //    PdfPCell cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(blank));
            //    PdfPCell cell1 = new PdfPCell(p);
            //    cell1.Border = Rectangle.NO_BORDER;
            //    cell1.FixedHeight = 30f;
            //    table.AddCell(cell1);

            //    p = new Phrase();
            //    p.Add(new Chunk("เลขที่ร้องเรียน", bold));
            //    p.Add(new Chunk(blank));
            //    p.Add(new Chunk(Complain_Code_ID, normal));
            //    PdfPCell cell2 = new PdfPCell(p);
            //    cell2.Border = Rectangle.NO_BORDER;
            //    cell2.HorizontalAlignment = Element.ALIGN_RIGHT;

            //    table.AddCell(cell2);
            //    #endregion
            //    #region Rows2


            //    p = new Phrase();
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(blank));
            //    cell1 = new PdfPCell(p);
            //    cell1.Border = Rectangle.NO_BORDER;
            //    cell1.FixedHeight = 30f;
            //    table.AddCell(cell1);

            //    p = new Phrase();
            //    p.Add(new Chunk("ช่องทางเรื่องร้องทุกข์", bold));
            //    p.Add(new Chunk(blank));
            //    p.Add(new Chunk(Description, normal));
            //    cell1 = new PdfPCell(p);
            //    cell1.Border = Rectangle.NO_BORDER;
            //    cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    //cell1.PaddingTop = 5;
            //    cell1.FixedHeight = 30f;
            //    table.AddCell(cell1);
            //    #endregion
            //    #region Rows3
            //    p = new Phrase();
            //    p.Add(new Chunk("วันที่บันทึก", bold));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);




            //    p = new Phrase();
            //    p.Add(new Chunk(Complain_Date, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.Colspan = 2;
            //    //cell0.PaddingTop = 5;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);


            //    #endregion
            //    #region Rows4
            //    p = new Phrase();
            //    p.Add(new Chunk("วันที่รับเรื่อง", bold));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(Complain_Date, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.Colspan = 2;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);
            //    #endregion
            //    #region Rows5
            //    p = new Phrase();
            //    p.Add(new Chunk("หัวข้อเรื่องร้องทุกข์", bold));
            //    p.Add(new Chunk(blank));
            //    p.Add(new Chunk(Complain_Subject, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.Colspan = 3;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);
            //    #endregion
            //    #region Rows6
            //    p = new Phrase();
            //    p.Add(new Chunk("ผู้ร้องเรียน", bold));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(FullNameStr, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.Colspan = 2;
            //    cell0.PaddingTop = 5;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);
            //    #endregion








            //    PdfPTable tableSub = new PdfPTable(3);
            //    tableSub.TotalWidth = 530f;
            //    tableSub.HorizontalAlignment = 0;
            //    tableSub.SpacingAfter = 10;
            //    tableWidths = new float[3];
            //    tableWidths[0] = 500f;
            //    tableWidths[1] = 500f;
            //    tableWidths[2] = 500f;
            //    tableSub.SetWidths(tableWidths);
            //    tableSub.LockedWidth = true;



            //    p = new Phrase();
            //    p.Add(new Chunk("ตำบล", bold));
            //    p.Add(new Chunk(blank));
            //    p.Add(new Chunk("บางยี่เรือ", normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    // cell0.Colspan = 2; 
            //    tableSub.AddCell(cell0);


            //    p = new Phrase();
            //    p.Add(new Chunk("อำเภอ", bold));
            //    p.Add(new Chunk(blank));
            //    p.Add(new Chunk("เขตธนบุรี", normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    tableSub.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk("จังหวัด", bold));
            //    p.Add(new Chunk(blank));
            //    p.Add(new Chunk("กรุงเทพมหานคร", normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    tableSub.AddCell(cell0);


            //    table.AddCell(tableSub);


            //    //p = new Phrase();
            //    //p.Add(new Chunk("ที่อยู่ที่ติดต่อได้", bold));
            //    //p.Add(new Chunk(blank));
            //    //p.Add(new Chunk("100 ริมทางรถไฟ", normal));
            //    //cell0 = new PdfPCell(p);
            //    //cell0.Border = Rectangle.NO_BORDER;
            //    //cell0.Colspan = 2;
            //    //table.AddCell(cell0);



            //    return table;


            //}
            //private PdfPTable GetAddressDetail(string Address, string District_NameTH, string Prefecture_NameTH, string Province_NameTH)
            //{
            //    string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
            //    string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
            //    BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //    Font h1 = new Font(bf_bold, 18);
            //    Font bold = new Font(bf_bold, 16);
            //    Font smallBold = new Font(bf_bold, 14);
            //    BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


            //    Font normal = new Font(bf_normal, 16);
            //    Font smallNormal = new Font(bf_normal, 14);

            //    PdfPTable table = new PdfPTable(6);
            //    table.TotalWidth = 530f;
            //    table.HorizontalAlignment = 0;
            //    table.SpacingBefore = 10;
            //    table.SpacingAfter = 10;
            //    #region Rows8 new table

            //    float[] columnWidths = new float[6];
            //    columnWidths[0] = 88f;
            //    columnWidths[1] = 88f;
            //    columnWidths[2] = 88f;
            //    columnWidths[3] = 88f;
            //    columnWidths[4] = 88f;
            //    columnWidths[5] = 88f;
            //    table.SetWidths(columnWidths);
            //    table.LockedWidth = true;

            //    #region Rows7


            //    PdfPCell cell0 = new PdfPCell(new Phrase("ผู้ที่อยู่ที่ติดต่อได้", bold));
            //    cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    PdfPCell cell1 = new PdfPCell(new Phrase(Address, normal));
            //    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell1.Border = Rectangle.NO_BORDER;
            //    cell1.Colspan = 5;
            //    cell1.FixedHeight = 30f;
            //    table.AddCell(cell1);


            //    #endregion



            //    cell0 = new PdfPCell(new Phrase("ตำบล", bold));
            //    cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    cell1 = new PdfPCell(new Phrase(District_NameTH, normal));
            //    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell1.Border = Rectangle.NO_BORDER;
            //    cell1.FixedHeight = 30f;
            //    table.AddCell(cell1);

            //    PdfPCell cell2 = new PdfPCell(new Phrase("อำเภอ", bold));
            //    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell2.Border = Rectangle.NO_BORDER;
            //    cell2.FixedHeight = 30f;
            //    table.AddCell(cell2);


            //    cell2 = new PdfPCell(new Phrase(Prefecture_NameTH, normal));
            //    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell2.Border = Rectangle.NO_BORDER;
            //    table.AddCell(cell2);

            //    cell2 = new PdfPCell(new Phrase("จังหวัด", bold));
            //    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell2.Border = Rectangle.NO_BORDER;
            //    table.AddCell(cell2);

            //    cell2 = new PdfPCell(new Phrase(Province_NameTH, normal));
            //    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cell2.Border = Rectangle.NO_BORDER;
            //    table.AddCell(cell2);
            //    #endregion

            //    return table;
            //}
            //private PdfPTable GetContackDetail(string Tel, string Mobile, string Fax, string Email)
            //{
            //    string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
            //    string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
            //    BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //    Font h1 = new Font(bf_bold, 18);
            //    Font bold = new Font(bf_bold, 16);
            //    Font smallBold = new Font(bf_bold, 14);
            //    BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);


            //    Font normal = new Font(bf_normal, 16);
            //    Font smallNormal = new Font(bf_normal, 14);

            //    PdfPTable table = new PdfPTable(4);
            //    table.TotalWidth = 530f;
            //    table.HorizontalAlignment = 0;
            //    table.SpacingBefore = 10;
            //    table.SpacingAfter = 10;
            //    float[] tableWidths = new float[4];
            //    tableWidths[0] = 130f;
            //    tableWidths[1] = 100f;
            //    tableWidths[2] = 100f;
            //    tableWidths[3] = 100f;
            //    table.SetWidths(tableWidths);
            //    table.LockedWidth = true;


            //    Chunk blank = new Chunk(" ", normal);

            //    #region Rows1
            //    Phrase p = new Phrase();
            //    p.Add(new Chunk("เบอร์โทรศัพท์", bold));
            //    PdfPCell cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(Tel, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk("มือถือ", bold));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(Mobile, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);
            //    #endregion
            //    #region Rows1
            //    p = new Phrase();
            //    p.Add(new Chunk("แฟกซ์", bold));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(Fax, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk("อีเมล", bold));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);

            //    p = new Phrase();
            //    p.Add(new Chunk(Email, normal));
            //    cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);
            //    #endregion
            //    return table;


            //}
            //private enum FonSize
            //{
            //    h1,
            //    bold,
            //    smallBold,
            //    normal,
            //    smallNormal
            //}
            //private Font SizeFont(FonSize obj)
            //{
            //    string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
            //    string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
            //    BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //    BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //    Font _font = new Font(bf_bold, 18);
            //    // Font h1 = new Font(bf_bold, 18);
            //    //Font bold = new Font(bf_bold, 16);
            //    //Font smallBold = new Font(bf_bold, 14);

            //    //Font normal = new Font(bf_normal, 16);
            //    //Font smallNormal = new Font(bf_normal, 14);
            //    switch (obj.ToString())
            //    {
            //        case "h1": _font = new Font(bf_bold, 18); break;
            //        case "bold": _font = new Font(bf_bold, 16); break;
            //        case "smallBold": _font = new Font(bf_bold, 14); break;
            //        case "normal": _font = new Font(bf_normal, 16); break;
            //        case "smallNormal": _font = new Font(bf_normal, 14); break;
            //    }
            //    return _font;
            //}
            //private PdfPCell Cellnormal(string _value, FonSize _font)
            //{
            //    Phrase p = new Phrase();
            //    p.Add(new Chunk(_value, SizeFont(_font)));
            //    PdfPCell cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.FixedHeight = 30f;
            //    return cell0;
            //}
            //private PdfPTable GetDetail(string Complain_Type_Name, string Complain_Type_Name_Sub, string CasenameTH, string Complain_Details, string Sue_case_Name)
            //{
            //    PdfPTable table = new PdfPTable(2);
            //    table.TotalWidth = 530f;
            //    table.HorizontalAlignment = 0;
            //    table.SpacingAfter = 10;
            //    float[] tableWidths = new float[2];
            //    tableWidths[0] = 180f;
            //    tableWidths[1] = 350f;
            //    table.SetWidths(tableWidths);
            //    table.LockedWidth = true;


            //    Chunk blank = new Chunk(" ", SizeFont(FonSize.bold));

            //    #region Rows1
            //    table.AddCell(Cellnormal("ประเภทการร้องเรียน", FonSize.bold));
            //    table.AddCell(Cellnormal(Complain_Type_Name, FonSize.normal));
            //    #endregion
            //    #region Rows2
            //    table.AddCell(Cellnormal("ประเภทเรื่องที่ร้องเรียน", FonSize.bold));
            //    table.AddCell(Cellnormal(Complain_Type_Name_Sub, FonSize.normal));

            //    #endregion
            //    #region Rows3
            //    table.AddCell(Cellnormal("สาเหตุเรื่องร้องเรียน", FonSize.bold));
            //    table.AddCell(Cellnormal(CasenameTH, FonSize.normal));
            //    #endregion
            //    #region Rows4
            //    table.AddCell(Cellnormal("รายละเอียดเรื่องร้องเรียน", FonSize.bold));
            //    table.AddCell(Cellnormal(Complain_Details, FonSize.normal));

            //    #endregion
            //    #region Rows5
            //    table.AddCell(Cellnormal("ผู้ร้องมีความประสงค์ ให้ดำเนินการ", FonSize.bold));
            //    table.AddCell(Cellnormal(Sue_case_Name, FonSize.normal));

            //    #endregion
            //    #region Rows6
            //    table.AddCell(Cellnormal("ความประสงค์ให้ดำเนินการ", FonSize.bold));
            //    table.AddCell(Cellnormal("", FonSize.normal));

            //    #endregion
            //    #region Rows7
            //    table.AddCell(Cellnormal("หลักฐานการประกอบเรื่องร้องทุกข์", FonSize.bold));
            //    table.AddCell(Cellnormal("", FonSize.normal));

            //    #endregion

            //    return table;
            //}
            //private PdfPTable GetAttachDetail()
            //{

            //    PdfPTable table = new PdfPTable(2);
            //    table.TotalWidth = 530f;
            //    table.HorizontalAlignment = 0;
            //    table.SpacingBefore = 10;
            //    table.SpacingAfter = 10;


            //    float[] columnWidths = new float[2];
            //    columnWidths[0] = 88f;
            //    columnWidths[1] = 442f;
            //    table.SetWidths(columnWidths);
            //    table.LockedWidth = true;

            //    Chunk blank = new Chunk(" ", SizeFont(FonSize.bold));

            //    #region Rows1
            //    table.AddCell(Cellnormal(" ", FonSize.bold));
            //    table.AddCell(Cellnormal("1.บัตรประชาชน", FonSize.normal));
            //    #endregion

            //    #region Rows1
            //    table.AddCell(Cellnormal(" ", FonSize.bold));
            //    table.AddCell(Cellnormal("1.บัตรประชาชน", FonSize.normal));
            //    #endregion

            //    #region Rows1
            //    table.AddCell(Cellnormal(" ", FonSize.bold));
            //    table.AddCell(Cellnormal("1.บัตรประชาชน", FonSize.normal));
            //    #endregion

            //    return table;
            //}
            //private PdfPTable GetBottoms()
            //{

            //    PdfPTable table = new PdfPTable(2);
            //    table.TotalWidth = 530f;
            //    table.HorizontalAlignment = 0;
            //    table.SpacingBefore = 10;
            //    table.SpacingAfter = 10;


            //    float[] columnWidths = new float[2];
            //    columnWidths[0] = 250f;
            //    columnWidths[1] = 250f;
            //    table.SetWidths(columnWidths);
            //    table.LockedWidth = true;

            //    Chunk blank = new Chunk(" ", SizeFont(FonSize.bold));
            //    #region Rows4


            //    Phrase p = new Phrase();
            //    p.Add(new Chunk("ข้าพเจ้าขอรับรองว่าข้อเท็จจริงที่ได้ยื่นเรื่องร้องทุกข์ต่อสำนักงานคณะกรรมการคุ้มครองผู้บริโภคเป็นเรื่องที่เกิดขึ้นจริงทั้งหมดและขอรับผิดชอบต่อข้อเท็จจริงดังกล่าวข้างต้นทุกประการ", SizeFont(FonSize.bold)));
            //    PdfPCell cell0 = new PdfPCell(p);
            //    cell0.Border = Rectangle.NO_BORDER;
            //    cell0.Colspan = 2;
            //    cell0.FixedHeight = 30f;
            //    table.AddCell(cell0);
            //    #endregion

            //    //#region Rows1
            //    //table.AddCell(Cellnormal(" ", FonSize.bold));

            //    //Phrase p = new Phrase();
            //    //p.Add(new Chunk("ข้าพเจ้าขอรับรองว่าข้อเท็จจริงที่ได้ยื่นเรื่องร้องทุกข์ต่อสำนักงานคณะกรรมการคุ้มครองผู้บริโภค เป็นเรื่องที่เกิดขึ้นจริงทั้งหมดและขอรับผิดชอบต่อข้อเท็จจริงดังกล่าวข้างต้นทุกประการ", SizeFont(FonSize.bold)));
            //    //PdfPCell cell0 = new PdfPCell(p);
            //    //cell0.Border = Rectangle.NO_BORDER;
            //    //cell0.FixedHeight = 30f;
            //    //cell0.Colspan = 2;
            //    //table.AddCell(cell0);
            //    //#endregion

            //    //#region Rows2
            //    //p = new Phrase();
            //    //p.Add(new Chunk(" sdsdvd ", SizeFont(FonSize.bold)));
            //    //cell0 = new PdfPCell(p);
            //    //cell0.Border = Rectangle.NO_BORDER;
            //    //cell0.FixedHeight = 30f;
            //    //cell0.Colspan = 2;
            //    //table.AddCell(cell0); 
            //    ////table.AddCell(Cellnormal("ลงชื่อ " + " นายชาญวิทย์ แฝงคด /r/n วันที่ xx/xx/xxxx", FonSize.normal));
            //    //#endregion



            //    return table;
            //}
            //public ActionResult Prints(string ID, string Key)
            //{
            //    GetComplain_reports_DetailModel obj = GetUtility.GetComplain_reports_Detail(ID.UrlDescriptHttp().Toint(), Key.UrlDescriptHttp());
            //    GetCustomer_Reports_DetailModel objCustomer = GetUtility.GetCustomer_Reports_Detail(obj.CustomerID);
            //    // Create PDF document
            //    Document pdfDoc = new Document(PageSize.A4, 30, 30, 20, 20);
            //    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //    pdfDoc.Open();
            //    pdfDoc.Add(GetReports());
            //    pdfDoc.Add(GetHeaderDetail(obj.Complain_Code_ID, obj.Description, obj.Complain_Date_str, obj.Complain_Subject, objCustomer.FullNameStr));
            //    pdfDoc.Add(GetAddressDetail(objCustomer.Address, objCustomer.District_NameTH, objCustomer.Prefecture_NameTH, objCustomer.Province_NameTH));
            //    pdfDoc.Add(GetContackDetail(objCustomer.Tel, objCustomer.Mobile, objCustomer.Fax, objCustomer.Email));
            //    pdfDoc.Add(GetDetail(obj.Complain_Type_Name, obj.Complain_Type_Name_Sub, obj.CasenameTH, obj.Complain_Details, obj.Sue_case_Name));
            //    pdfDoc.Add(GetAttachDetail());
            //    pdfDoc.Add(GetBottoms());

            //    //LineSeparator line = new LineSeparator();

            //    //pdfDoc.Add(line);

            //    pdfWriter.CloseStream = false;
            //    pdfDoc.Close();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("content-disposition", "attachment;filename=" + obj.Complain_Code_ID + ".pdf");
            //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    Response.Write(pdfDoc);
            //    Response.End();

            //    //เลข 4 ตัวหลัง layout ของกระดาษคือระยะขอบครับ
            //    //Document pdfDoc = new Document(PageSize.A4, 30, 30, 20, 20);
            //    //PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //    //pdfDoc.Open();

            //    // Bold
            //    //BaseFont bf_bold = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //    //h1 = new Font(bf_bold, 18);
            //    //bold = new Font(bf_bold, 16);
            //    //smallBold = new Font(bf_bold, 14);

            //    // Normal
            //    //BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //    //normal = new Font(bf_normal, 16);
            //    //smallNormal = new Font(bf_normal, 14);





            //    //var obj = GetUtility.Getreports_SummaryData();
            //    //obj.WriteXSD("Tab1");
            //    //ExportsDoc.FileExportsName = "รายงานการดำเนินการเรื่องร้องทุกข์";
            //    //ExportsDoc.CreateReportForm("reports_Tab1", ExportsDoc.TypeExports.EXCEL, obj);  
            //    return View();
            //}


            //#endregion

    }
    public class ReportsController : Controller {
        #region prints reports
        private PdfPTable GetReports()
        {
            var rows1 = ItextCharpLib.Col1Config();

            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 530f;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 10;
            table.SpacingAfter = 10;

            float[] tableWidths = new float[1];
            tableWidths[0] = 530f;

            table.SetWidths(tableWidths);
            table.LockedWidth = true;  
            string picpath = HttpContext.Server.MapPath("~/images/logo58-Red(500x500).png");
            var ImgHeader = ItextCharpLib.ImgConfig_center(80, 80, picpath);
            rows1.AddCell(ImgHeader);
            return rows1;
        }
        private PdfPTable GetHeaderDetail(string Complains_Details,DateTime? CancelDate)
        { 

            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 530f;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 10;
            table.SpacingAfter = 10;

            float[] tableWidths = new float[1];
            tableWidths[0] = 530f; 

            table.SetWidths(tableWidths);
            table.LockedWidth = true;  
            table.AddCell(CellnormalColspan("บันทึกขอยุติเรื่องร้องทุกข์", FonSize.bold,2));
            table.AddCell(CellnormalColspan_text_RIGHT("วันที่ " + CancelDate.ToThaiFormateFull(), FonSize.normal, 2));
            table.AddCell(CellnormalColspan_text_left(Complains_Details, FonSize.normal, 2));
            table.AddCell(CellnormalColspan_text_left("บัดนี้ข้าพเจ้าต้องการยกเลิกเรื่องร้องทุกข์เนื่องจาก", FonSize.bold, 2));

             

            return table; 
        }
        private PdfPTable GetCheckbox(_statusSelect Obj)
        {
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 530f;
            table.HorizontalAlignment = 0;
            table.SpacingAfter = 10;
            float[] tableWidths = new float[2];
            tableWidths[0] = 80f;
            tableWidths[1] = 450f;
            table.SetWidths(tableWidths);
            table.LockedWidth = true;
             
            string picpath = "";
            if (Obj.check)
            {
                picpath = HttpContext.Server.MapPath("~/images/Very-Basic-Checked-checkbox-icon.png");
            }
            else
            {
                picpath = HttpContext.Server.MapPath("~/images/yTogj4zEc.png");
            }
            var ImgHeader = ItextCharpLib.ImgConfig_center(12, 12, picpath); 
            ImgHeader.HorizontalAlignment = Element.ALIGN_RIGHT;
            ImgHeader.VerticalAlignment = Element.ALIGN_BOTTOM;
            table.AddCell(ImgHeader);
            table.AddCell(Cellnormal("   " +Obj.Name, FonSize.normal));

            return table;

            // add a image 
        }
        private PdfPTable GetHeaderSignature()
        {

            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 10;
            table.AddCell(CellnormalColspan_text_left("ดังนั้นข้าพเจ้าฯ จึงขอยุติเรื่องร้องทุกข์ดังกล่าว ทั้งนี้ข้าพเจ้าขอรับรองว่าข้อความข้างต้นเป็นความจริง", FonSize.bold, 2));
           table.AddCell(CellnormalColspan("ลงชื่อ.................................ผู้ร้องทุกข์", FonSize.bold, 1));
            //table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

            //table.AddCell(CellnormalColspan(  "(   "+ CustomerName +"  )", FonSize.normal, 1));
            //table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));


            //table.AddCell(CellnormalColspan("ลงชื่อ.................................เจ้าหน้าที่เจ้าของเรื่อง", FonSize.bold, 1));
            //table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

            //table.AddCell(CellnormalColspan("(......................................)", FonSize.normal, 1));
            //table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));
            return table;
        }

        private PdfPTable GetHeaderSignature2(string CustomerName)
        {

            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = 530f;
            table.HorizontalAlignment = 0;
            table.SpacingAfter = 10;
            float[] tableWidths = new float[2];
            tableWidths[0] = 200f;
            tableWidths[1] = 250f;
            table.SetWidths(tableWidths);
            table.LockedWidth = true;
            table.AddCell(CellnormalColspan(" ", FonSize.normal, 2));

            var Cell1 = CellnormalColspan("ลงชื่อ.................................ผู้ร้องทุกข์", FonSize.normal, 1);
            Cell1.Top = 35f;
            table.AddCell(Cell1);
            table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

            Cell1 = CellnormalColspan("(   " + CustomerName + "  )", FonSize.normal, 1);
            table.AddCell(Cell1);
            table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

            table.AddCell(CellnormalColspan(" ", FonSize.normal, 2));

            Cell1 = CellnormalColspan("       ลงชื่อ.................................เจ้าหน้าที่เจ้าของเรื่อง", FonSize.normal, 1);
            table.AddCell(Cell1);
             table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));

            Cell1 = CellnormalColspan("(......................................)", FonSize.normal, 1);
            table.AddCell(Cell1);
            table.AddCell(CellnormalColspan(" ", FonSize.normal, 1));
            return table;
        }

        private PdfPTable GetBottoms()
        {
            PdfPTable table = new PdfPTable(1);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 10;
            table.AddCell(Cellnormal("สำนักงานคณะกรรมการคุ้มครองผู้บริโภค อาคารรัฐประศาสนภักดี (อาคารบี) ชั้น 5 ศูนย์ราชการเฉลิมพระเกียรติ 80 พรรษา 5 ธันวาคม 2550 ถนนแจ้งวัฒนะ เขตหลักสี่ กทม. 10210 หรือส่งทางโทรสารหมายเลข 02 143 9774 และสามารถสอบถามได้ที่ โทรศัพท์หมายเลข 02 141 3487 หรือ 02 141 3490", FonSize.normal));
            return table;
        }
        private enum FonSize
        {
            h1,
            bold,
            smallBold,
            normal,
            smallNormal
        }
        private Font SizeFont(FonSize obj)
        {
            string pathFont = HttpContext.Server.MapPath("~/fonts/THSarabun.ttf");
            string pathFontB = HttpContext.Server.MapPath("~/fonts/THSarabun Bold.ttf");
            BaseFont bf_bold = BaseFont.CreateFont(pathFontB, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            BaseFont bf_normal = BaseFont.CreateFont(pathFont, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font _font = new Font(bf_bold, 18);
             
            switch (obj.ToString())
            {
                case "h1": _font = new Font(bf_bold, 18); break;
                case "bold": _font = new Font(bf_bold, 16); break;
                case "smallBold": _font = new Font(bf_bold, 14); break;
                case "normal": _font = new Font(bf_normal, 16); break;
                case "smallNormal": _font = new Font(bf_normal, 14); break;
            }
            return _font;
        }
        private PdfPCell Cellnormal(string _value, FonSize _font)
        {
            Phrase p = new Phrase();
            p.Add(new Chunk(_value, SizeFont(_font)));
            PdfPCell cell0 = new PdfPCell(p);
            cell0.Border = Rectangle.NO_BORDER;
            cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            cell0.VerticalAlignment = Element.ALIGN_MIDDLE;
            //cell0.FixedHeight = 30f;
            return cell0;
        }
        private PdfPCell CellnormalColspan(string _value, FonSize _font,int span)
        {
            Phrase p = new Phrase();
            p.Add(new Chunk(_value, SizeFont(_font)));
            PdfPCell cell0 = new PdfPCell(p);
            cell0.Border = Rectangle.NO_BORDER;
            cell0.Colspan = span;
            cell0.HorizontalAlignment = Element.ALIGN_CENTER;
            //cell0.FixedHeight = 30f;
            return cell0;
        }
        private PdfPCell CellnormalColspan_text_RIGHT(string _value, FonSize _font, int span)
        {
            Phrase p = new Phrase();
            p.Add(new Chunk(_value, SizeFont(_font)));
            PdfPCell cell0 = new PdfPCell(p);
            cell0.Border = Rectangle.NO_BORDER;
            cell0.Colspan = span;
            cell0.HorizontalAlignment = Element.ALIGN_RIGHT;
            //cell0.FixedHeight = 30f;
            return cell0;
        }
        private PdfPCell CellnormalColspan_text_left(string _value, FonSize _font, int span )
        {
            Phrase p = new Phrase();
            p.Add(new Chunk(_value, SizeFont(_font)));
            PdfPCell cell0 = new PdfPCell(p);
            cell0.Border = Rectangle.NO_BORDER;
            cell0.Colspan = span;
            cell0.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell0.FixedHeight = 30f;
            return cell0;
        }
        private class _statusSelect
        {
            public string Name { get; set; }
            public bool check { get; set; }
        }
         public ActionResult Prints(string ID, string Key)
        {
            GetComplain_reports_DetailModel obj = GetUtility.GetComplain_reports_Detail(ID.UrlDescriptHttp().Toint(), Key.UrlDescriptHttp());
            GetCustomer_Reports_DetailModel objCustomer = GetUtility.GetCustomer_Reports_Detail(obj.CustomerID);

            Complains_CancelMapDao MapCancel = new Complains_CancelMapDao();
            var _Cancle = MapCancel.FindAll().Where(o => o.ComplainID == obj.ID);
            var _Status = (from T in GetUtility.Complain_Cancel()
                            join o in _Cancle on T.ID equals o.CancelID into Temp
                            from _Temp in Temp.DefaultIfEmpty()
                            select new _statusSelect {
                                check = (_Temp != null)?true:false,
                                 Name = T.StatusTH 
                            }); 
             // Create PDF document
            Document pdfDoc = new Document(PageSize.A4, 30, 30, 20, 20);
            PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            pdfDoc.Add(GetReports());
            pdfDoc.Add(GetHeaderDetail(obj.Complain_Details, _Cancle.FirstOrDefault().Cancel_Date));
              
            foreach (var Items in _Status)
            {
                pdfDoc.Add(GetCheckbox(Items));
            }
            CustomerMapDao Cusmap = new CustomerMapDao();
            var Cusobj = Cusmap.FindById(obj.CustomerID.Toint()); 
            pdfDoc.Add(GetHeaderSignature( )); 
            pdfDoc.Add(GetHeaderSignature2(Cusobj.FullNameStr));
            pdfDoc.Add(GetBottoms());
          
            //LineSeparator line = new LineSeparator();

            //pdfDoc.Add(line);

            pdfWriter.CloseStream = false;
            pdfDoc.Close();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=บันทึกขอยุติเรื่องร้องทุกข์.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
             
            return View();
        }


        #endregion

    }

}
