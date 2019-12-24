using OCPB.Model;
using OCPB.Models;
using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
namespace OCPB.Controllers
{

    public class ManageController : BaseController
    {
        //
        // GET: /Manage/ 
        [CustomAuthorize]
        public ActionResult Index()
        {
            return View();
        } 
        public PartialViewResult ManagePartial(string page)
        {
            var obj = GetComplain.GetComplainlist_For_customer(Helplibery.GetUserID());
            return PartialView("_Managelist", obj);
        }
        public ActionResult Password()
        {
            return View(new LocalPasswordModel());
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Password(LocalPasswordModel obj)
        {
            if (ModelState.IsValid)
            {
                CustomerMapDao map = new CustomerMapDao();
                var _obj = map.FindById(Helplibery.GetUserID());
                if (_obj.Password == Encryption.Encrypt(obj.OldPassword))
                {
                    _obj.Password = Encryption.Encrypt(obj.NewPassword.Trim());
                    map.AddOrUpdate(_obj);
                    map.CommitChange();
                    if (!string.IsNullOrEmpty(_obj.Email))
                    {

                        //ChangePasswordViewModel newObj = new ChangePasswordViewModel();
                        //newObj.ConfirmPassword = obj.NewPassword;
                        //newObj.NewPassword = obj.NewPassword;
                        //newObj.OldPassword = obj.OldPassword;

                        SendEmail.SendMail(_obj.Email, "แก้ไขรหัสผ่านสํานักงานคณะกรรมการคุ้มครองผู้บริโภค", RenderPartialViewToString("ChangePasswordTemplate", _obj));
                        //SendMail.Send(obj.Email, "สํานักงานคณะกรรมการคุ้มครองผู้บริโภค ยินดีต้อนรับเข้าสู่การเป็นสมาชิก", RenderPartialViewToString("ChangePassword", obj));
                    }

                    return Json(new ResultData() { Status = true, text = Url.Action("Index", "Manage") }, JsonRequestBehavior.AllowGet);
                }
            }
            ModelState.AddModelError("", "ข้อมูลไม่ถูกต้อง");
            return Json(new ResultData() { Status = false, text = "ข้อมูลไม่ถูกต้อง" }, JsonRequestBehavior.AllowGet);
         }
         

        public ActionResult Complain(string id)
        {
            TempData.Clear();
            ComplainStep2 _ObjStep2 = new ComplainStep2();
            string Str = "";
            switch (id)
            {
                case "Product": { Str = "สินค้า"; _ObjStep2.ComplainID = 8; break; }
                case "Service": { Str = "บริการ"; _ObjStep2.ComplainID = 9; break; }
                case "Property": { Str = "อสังหาริมทรัพย์"; _ObjStep2.ComplainID = 1; break; }
                default: { return RedirectToAction("Index", "Manage"); }
            }
            ViewBag.TypeStr = Str;
            TempData["_ComplainStep2"] = _ObjStep2;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Complain(List<ComplainFileUploadModel> Step3ObjectData)
        {
            ComplainStep1 Step1 = (ComplainStep1)TempData["_ComplainStep1"];
            ComplainStep2 Step2 = (ComplainStep2)TempData["_ComplainStep2"];
            if (Step1 == null || Step2 == null)
            {
                return RedirectToAction("Complain", "Manage");
            } 
            string _refID = "";


            int CompId = SaveComplain.AddnewComplain(Step1.Complain_Subject, 1, Helplibery.GetUserID(), Step1.CompanyName, Step1.CompanyDescription,
                Step2.Description, Step2.ComplainID, Step2.lv1, Step2.Tm_CauseID, Step2.PurchaseID,null, Step2.PaymentID,null, Step2.MotiveID,null, Step1.attorneyLog, null, ref _refID);
            SaveComplain.StartTrack(CompId, 1,null,null);

            SaveUtility.SaveComplainFileUpload(Step3ObjectData, CompId);

            //SaveComplain.StartTrack(CompId);
            TempData.Clear();


            return RedirectToAction("Confirm", "Manage");
        }
        public ActionResult Confirm()
        {
            ComplainsMapDao Map = new ComplainsMapDao();
            var Obj = Map.FindByCustomerID(Helplibery.GetUserID()).LastOrDefault(); 
            CustomerMapDao CusMap = new CustomerMapDao(); 
            var Cmap = CusMap.FindById(Obj.CustomerID.Toint());
            ViewBag.fullname = Cmap.FullNameStr; 
            return View(Obj);
        }

        public PartialViewResult Step1Complain()
        {
            ComplainStep1 obj = new ComplainStep1();
            if (TempData["_ComplainStep1"] != null)
            {
                obj = (ComplainStep1)TempData["_ComplainStep1"];
            }
            if (TempData["_ComplainStep2"] != null)
            {
                TempData["_ComplainStep2"] = TempData["_ComplainStep2"];
            }
            return PartialView("_ComplainStep1", obj);
        }

        public PartialViewResult Step2Complain(ComplainStep1 obj)
        {
            if (ModelState.IsValid)
            {
                TempData["_ComplainStep1"] = obj;
                ComplainStep2 _ObjStep2 = new ComplainStep2();
                if (TempData["_ComplainStep2"] != null)
                {
                    _ObjStep2 = (ComplainStep2)TempData["_ComplainStep2"];
                }
                return PartialView("_ComplainStep2", _ObjStep2);
            }
            return PartialView("_ComplainStep1", obj);
        }


        public PartialViewResult Step2back()
        {
            TempData["_ComplainStep1"] = TempData["_ComplainStep1"];
            ComplainStep2 _ObjStep2 = new ComplainStep2();
            if (TempData["_ComplainStep2"] != null)
            {
                _ObjStep2 = (ComplainStep2)TempData["_ComplainStep2"];
            }
            return PartialView("_ComplainStep2", _ObjStep2);
        }

        public PartialViewResult Step3Complain(ComplainStep2 obj)
        {
            if (ModelState.IsValid)
            {
                TempData["_ComplainStep1"] = TempData["_ComplainStep1"];
                TempData["_ComplainStep2"] = obj;
                return PartialView("_ComplainStep3");
            }
            return PartialView("_ComplainStep2", obj);
        }


        public PartialViewResult CustomerDetial()
        {
            Customer obj = new Customer();
            CustomerMapDao map = new CustomerMapDao();
            obj = map.FindById(Helplibery.GetUserID());
            return PartialView("_CustomerDetail", obj);
        }
        public PartialViewResult AttorneyDateil()
        {
            return PartialView("_AttorneyDateil", new ComplainStep1() { attorneyLog = new Complains_attorney_log() });
        }
        public PartialViewResult Complain_Type_LV1(int refID)
        {
            List<Tm_Complain_Type> obj = Tm_Complain_Type1(refID);

            ComplainStep2 _ObjStep2 = new ComplainStep2();
            if (TempData["_ComplainStep2"] != null)
            {
                _ObjStep2 = (ComplainStep2)TempData["_ComplainStep2"];
            }

            ViewBag.select = _ObjStep2.lv1;
            return PartialView("_Complain_Type_LV1", obj);
        }
        public PartialViewResult SuecaseGroup(int refID)
        {
            var obj = GetUtility.GetTm_case(refID);

            ComplainStep2 _ObjStep2 = new ComplainStep2();
            if (TempData["_ComplainStep2"] != null)
            {
                _ObjStep2 = (ComplainStep2)TempData["_ComplainStep2"];
            }

            ViewBag.select = _ObjStep2.Tm_CauseID;
            return PartialView("_CauseDetail", obj);
        }
        public JsonResult CancelComplain(string Keygen, string Description, int? CancelID)
        {

            if (SaveComplain.Cancel(Keygen.UrlDescriptHttp(), Description, CancelID))
            { 
                return Json(new ResultData() { Status = true, text = "ยกเลิกข้อมูลสำเร็จ" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultData() { Status = false, text = "การยกเลิกข้อมูลมีปัญหา กรุณาติดต่อเจ้าหน้าที่" }, JsonRequestBehavior.AllowGet);
             }
        } 
    }
}
