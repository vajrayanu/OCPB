using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCPB.Library;
using OCPB.Model;
using OCPB.Models;

namespace OCPB.Controllers
{
    public class CerIdentController : Controller
    {
        //
        // GET: /CerIdent/
        [EncryptedActionParameter]
        public ActionResult Index(string Mode, string Compname, int ID, string Guid, string Code)
        {

            ViewBag.typeBagID = Mode;
            ViewBag.DirecsaleID = ID;
            ViewBag.Keygen = Guid;
            ViewBag.Code = Code;
            ViewBag.HeaderStr = "ข้อมูลการจดทะเบียนการประกอบธุรกิจตลาดแบบตรง";

            if (ID > 0)
            {
                if ("ระบบขายตรง" == Mode)
                {
                    var Sale = OCPB.GetCompany.GetSaleByID(ID);
                    //var _User = OCPBLIb.OCPBlib.CompanyMethod.Get.GetCompany_user(keygen);
                    var _User = OCPB.GetCompany.CompanyMethod.Get.GetCompany_user(Sale.Keygen);
                    ViewBag.license = Sale.license;
                    ViewBag.Citizen_Users = Sale.license;
                }
                {
                    var Market = OCPB.GetCompany.GetMarketByID(ID);
                    var _User = OCPB.GetCompany.CompanyMethod.Get.GetCompany_user(Market.Keygen);
                    ViewBag.license = Market.license;
                }
            }

            int Temp = OCPB.GetCompany.GetByID(ID, Mode);
            var Obj = OCPB.GetCompany.CompanyMethod.Get.GetCompany(Temp);

            return View(Obj);

        }

        public PartialViewResult _Address2(int ID)
        {
            var Obj = OCPB.GetCompany.CompanyMethod.Get.GetCompany_Address(ID);
            return PartialView("View/_Address2", Obj);
        }

        public PartialViewResult CommitteeInformations2(int ID)
        {
            CommitteeModel Obj = new CommitteeModel();
            Obj.Committee = OCPB.GetCompany.CompanyMethod.Get.GetCompany_CommitteeInformations(ID);
            Obj.Authorize = OCPB.GetCompany.CompanyMethod.Get.GetCompany_AuthorizeDescriptions(ID);
            return PartialView("View/Committee2", Obj);
        }

        public PartialViewResult GetCompany_user(int ID,string Mode)
        {
            List<Citizen_Users> _User = new List<Citizen_Users>();

            if ("ระบบขายตรง" == Mode)
            {
                var Sale = OCPB.GetCompany.GetSaleByID(ID);
                //var _User = OCPBLIb.OCPBlib.CompanyMethod.Get.GetCompany_user(keygen);
                _User = OCPB.GetCompany.CompanyMethod.Get.GetCompany_user(Sale.Keygen);
                ViewBag.license = Sale.license;
                ViewBag.Citizen_Users = Sale.license;
            }
            {
                var Market = OCPB.GetCompany.GetMarketByID(ID);
                _User = OCPB.GetCompany.CompanyMethod.Get.GetCompany_user(Market.Keygen);
                ViewBag.license = Market.license;
            }

            return PartialView("View/Citizen_Users", _User);
        }

        public PartialViewResult _CerDirecMarget(int ID)
        {
            var Obj = OCPB.GetCompany.GetMarketByID(ID);

            if (Obj.DirecMarketTypeID.Equals(2))
            {
                Obj.DirecMarketTypeText = "B2B2C";
            }
            else if (Obj.DirecMarketTypeID.Equals(1))
            {
                Obj.DirecMarketTypeText = "B2B";

            }
            else
            {
                Obj.DirecMarketTypeText = "";
            }

            return PartialView("Content/_CerMarketDirecsale", Obj);

        }

        public PartialViewResult _CerDirecsale(int ID)
        {
            var Obj = OCPB.GetCompany.GetSaleByID(ID);
            return PartialView("Content/_CerDirecsale", Obj);
        }
    }
}