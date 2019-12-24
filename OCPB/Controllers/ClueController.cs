using OCPB.Model;
using OCPB.Models;
using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BotDetect.Web.Mvc;
namespace OCPB.Controllers
{
    public class ClueController : BaseController
    {
        //
        // GET: /Clue/
        public ActionResult Index()
        {          
            return View(new Clue());         
        }
            
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "ClueCaptcha", "Incorrect!")]
        public ActionResult Index(Clue obj, bool captchaValid, List<HttpPostedFileBase> Filename)
        {
            if (captchaValid)
            {
                if (ModelState.IsValid)
                {
                    ClueMapDao clueMap = new ClueMapDao();
                    if (obj.ID == 0)
                    {
                        obj.Active = true;
                        obj.CreateDate = DateTime.Now;                       
                        obj.Keygen = Guid.NewGuid().ToString(); 
                    }
                    obj.Complain_Channel_id = 1;//เรื่องร้องทุกข์ Online
                    clueMap.Add(obj);
                    clueMap.CommitChange();
                    SaveUtility.SaveClueFileUpload(Filename, "Clue", obj.Keygen);
                    return RedirectToAction("ClueModal", "Clue");                               
                }
            }
            TempData.Clear();
            ModelState.AddModelError("", "ข้อมูลไม่ถูกต้อง");
            MvcCaptcha.ResetCaptcha("ClueCaptcha");
            return View(obj); 
        }

        public ActionResult ClueModal()
        {
            return View();
        }

        public ActionResult ClueCondition()
        {
            return View("_ClueConditionTH");
        }
      }
}
