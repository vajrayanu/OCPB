using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OCPB.Model;
using PagedList;


namespace OCPB.Controllers
{
    public class PublicSearchController : Controller
    {
        // GET: PublicSearch
        public ActionResult Index()
        {
            SearchModelCenter obj = new SearchModelCenter();
            obj.Mode = "";
            return View(obj);
        }

        public PartialViewResult IndexPatial(SearchModelCenter obj, string page, string value, string Mode)
        {
            if (!string.IsNullOrEmpty(value)) {
                obj.Value = value;
            }
            if (!string.IsNullOrEmpty(Mode))
            {
                obj.Mode = Mode;
            }
            ViewBag.Param = obj;

            var _Result = new List<DirecAll>();
            _Result = GetDirecAll.GetByValue(obj.Mode, obj.Value);
            return PartialView("IndexList_DirecAll", _Result.ToPagedList(page.ToPagging(), 20));

        }

    }
}