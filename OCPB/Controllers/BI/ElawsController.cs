using Newtonsoft.Json;
using OCPB.Model;
using OCPB.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml.Linq;

namespace OCPB.Controllers.BI
{
    public class LawsResult
    {
        public List<Laws> LawsList;
        public int pages;
    }

    public class ElawsController : BaseServiceController
    {
        private int PageSize;
        private int Skip;
        private int allPage;
        private int limitPage;

        // GET api/elaws
        [Route("api/Elaws")]
        public void Get(int? pageIndex = 0)
        {
            try
            {
                this.PageSize = Int32.Parse(WebConfigurationManager.AppSettings["laws_perpage"].ToString());
                this.limitPage = Int32.Parse(WebConfigurationManager.AppSettings["laws_limitpage"].ToString());

                if (pageIndex.Value > 0)
                    this.Skip = this.PageSize * (pageIndex.Value - 1);

                int limitTake = this.limitPage * this.PageSize;

                LawsMapDao Map = new LawsMapDao();

                List<Laws> Laws = Map.FindByActive();
                int allRow = Laws.Count();

                if (allRow > 0)
                {
                    decimal pages = Math.Ceiling((decimal)allRow / this.PageSize);
                    this.allPage = (int)pages;

                    Laws = Laws.Where(c => c.Active).OrderByDescending(o => o.ID).Take(limitTake).ToList();
                    if (pageIndex.Value > 0)
                        Laws = Laws.Skip(this.Skip).Take(this.PageSize).ToList();
                    else
                        Laws = Laws.Take(2).ToList();
                }

                if (this.allPage > this.limitPage)
                    this.allPage = this.limitPage;

                _result(new LawsResult() { LawsList = Laws, pages = this.allPage });

            }
            catch (Exception ex)
            {
                _result(ex.Message.ToString());
            }
        }

        private void _result(Object value)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            string jsonStr = JsonConvert.SerializeObject(Trueresult(value));

            HttpContext.Current.Response.Write(jsonStr);
            HttpContext.Current.Response.End();
        }

        // GET api/elaws/5
        [Route("api/Elaws")]
        public void Get(int id)
        {
            LawsMapDao Map = new LawsMapDao();
            _result(Map.FindById(id));   
        }

        

     }
}
