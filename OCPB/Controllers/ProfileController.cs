using OCPB.Model;
using OCPB.Models;
using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OCPB.Controllers
{
    [CustomAuthorize]
    public class ProfileController : BaseController
    {
        //
        // GET: /Profile/ 
        public ActionResult Index()
        {
            CustomerMapDao map = new CustomerMapDao(); 
            return View(map.FindById(Helplibery.GetUserID()));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Customer items)
        {
            ModelState.Remove("IdentityID"); 
            ModelState.Remove("Sex"); 

            if (ModelState.IsValid)
            {
                if (SaveAccount.UpdateUser(items.ID, items.TitleID, items.Fname, items.Lname, items.DateOfBirthStr, items.OccupationID, items.SalaryID, items.Address, items.ProvinceID, items.PrefectureID, items.DistrictID, items.ZipCode,
                      items.Tel, items.Tel_ext, items.Mobile, items.Fax, items.Email))
                {
                   // return Json(new { RedirectUrl = Url.Action("Index", "Manage") });

                    return Json(new ResultData() { Status = true, text = Url.Action("Index", "Manage") }, JsonRequestBehavior.AllowGet);

                }
            }
            return View(items); 
        }
    }
}
