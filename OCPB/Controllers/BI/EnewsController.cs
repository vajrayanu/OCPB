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

namespace OCPB.Controllers
{
    public class EnewsController : BaseServiceController
    {
        private int PageSize;
        private int Skip;

        // GET api/enews
        [Route("api/Enews")]
        public void Get(int? pageIndex = 1)
        {


            try
            {
                this.PageSize = Int32.Parse(WebConfigurationManager.AppSettings["news_perpage"].ToString());

                this.Skip = this.PageSize * (pageIndex.Value - 1);

                //list result
                List<dlt> Obj = new List<dlt>();

                Enews_ex_linkMapDao Map = new Enews_ex_linkMapDao();
                foreach (var Items in Map.FindByActive())
                {
                    foreach (var SItems in (ExObj(Items)))
                    {
                        Obj.Add(SItems);
                    }
                }

                ENewsMapDao EMap = new ENewsMapDao();
                //var Intra = new MainObject { copyright = "สคบ", title = "สคบ", description = "สคบ", item = new List<dlt>() };
                List<dlt> Sitems = new List<dlt>();
                foreach (var Items in EMap.FindByActive())
                {
                    Obj.Add(new dlt { title = Items.Title, description = Items.Description, pubDate = Items.CreateDate_strFull, copyright = "สคบ" });
                }
                //Intra.item.AddRange(Sitems);
                //Obj.Add(Intra);

                _result(Obj.OrderByDescending(o => o.Date).Skip(this.Skip).Take(this.PageSize));

            }
            catch (Exception ex)
            {
                _result(ex.Message.ToString());
            }

        }

        private void _result(Object value)
        {
            HttpContext.Current.Response.ContentType = "application/json";
            HttpContext.Current.Response.Write(JsonConvert.SerializeObject(Trueresult(value)));
            HttpContext.Current.Response.End();
        }
        //public class MainObject
        //{
        //    public string title { get; set; } 
        //    public string description { get; set; } 
        //    public string copyright { get; set; }
        //    public List<dlt> item { get; set; }
        //    public string ErrorMessage { get; set; }
        //}
        public class dlt
        {
            public string title { get; set; }
            public string link { get; set; }
            public string description { get; set; }
            public string pubDate { get; set; }
            public string author { get; set; }
            public string copyright { get; set; }
            public string NewsDate { get; set; }
            public DateTime Date { get; set; }
        }

        #region กรมการขนส่งทางบก
        public List< dlt> ExObj(Enews_ex_link Exlink)
        {
            List<dlt> Res = new List<dlt>();
            try
            {

                var webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                string result = webClient.DownloadString(Exlink.Ex_link);
                XDocument document = XDocument.Parse(result);

                //var tetet = document.Descendants("title");
                List<XElement> items = document.Descendants("item").ToList();//.Skip(this.Skip).Take(this.PageSize).ToList();

                foreach(var item in items)
                {
                    string dateStr = item.GetElementIfExists("pubDate");

                    string dateValue = "";
                    DateTime date = new DateTime();
                    if (!DateTime.TryParse(dateStr, out date))
                        dateValue = date.ToString("d . M . yyyy");
                    else
                    {
                        date = DateTimeOffset.Parse(dateStr).UtcDateTime;
                        dateValue = date.ToString("d . M . yyyy");
                    }

                    Res.Add(
                        new dlt()
                        {
                            title = item.GetElementIfExists("title"),
                            link = item.GetElementIfExists("link"),
                            description = item.GetElementIfExists("description"),
                            pubDate = item.GetElementIfExists("pubDate"),
                            author = item.GetElementIfExists("author"),
                            copyright = Exlink.Title,
                            NewsDate = dateValue,
                            Date = date
                        }
                        );
                }

                return Res;

                //return (
                //    from descendant in document.Descendants("item")
                //        select new dlt()
                //       {
                //           title = descendant.GetElementIfExists("title"),
                //           link = descendant.GetElementIfExists("link"),
                //           description = descendant.GetElementIfExists("description"),
                //           pubDate = descendant.GetElementIfExists("pubDate"),
                //           author = descendant.GetElementIfExists("author"),
                //           copyright = Exlink.Title
                //       }).ToList();
            }
            catch(Exception ex)
            {
                return new List<dlt>();
            }
           
        }
        #endregion
        #region กรมการปกครอง
        public List<dlt> Readopa()
        {
            
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string result = webClient.DownloadString(@"https://www.dopa.go.th/rss_feed/news1.xml");
            XDocument document = XDocument.Parse(result);
            // Obj.copyright = "กรมการขนส่งทางบก";
          
            var tetet = document.Descendants("title");
            return  (from descendant in document.Descendants("item")
                        select new dlt()
                        {
                            title = descendant.Element("title").Value,
                            link = descendant.Element("link").Value,
                            description = descendant.Element("description").Value,
                            pubDate = descendant.Element("pubDate").Value,
                            author = descendant.Element("author").Value ,
                            copyright = "กรมการปกครอง"
                        }).ToList();
      
        }
        #endregion
        #region ข่าวประชาสัมพันธ์ (กรมการปกครอง กระทรวงมหาดไทย)
        public List<dlt> Readoic()
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string result = webClient.DownloadString(@"http://www.oic.or.th/th/feed.xml");
            XDocument document = XDocument.Parse(result);
            //Obj.copyright = "คปภ";
            //Obj.description = "oic";
            //Obj.title = "คปภ";
            var tetet = document.Descendants("title");
         return (from descendant in document.Descendants("item")
                        select new dlt()
                        {
                            title = descendant.Element("title").Value,
                            link = descendant.Element("link").Value,
                            description = descendant.Element("description").Value,
                            pubDate = descendant.Element("pubDate").Value,
                            //author = descendant.Element("author").Value
                                  copyright = "คปภ"
                        }).ToList();
         
        }
        #endregion
        //#region กรมการประกันภัย
        //public MainObject Readoic()
        //{
        //    MainObject Obj = new MainObject();

        //    var webClient = new WebClient();
        //    webClient.Encoding = Encoding.UTF8;
        //    string result = webClient.DownloadString(@"http://www.oic.or.th/th/feed.xml");
        //    XDocument document = XDocument.Parse(result);
        //    Obj.copyright = "กรมการประกันภัย";
        //    Obj.description = "oic";
        //    Obj.title = "กรมการประกันภัย";
        //    var tetet = document.Descendants("title");
        //    Obj.item = (from descendant in document.Descendants("item")
        //                select new dlt()
        //                {
        //                    title = descendant.Element("title").Value,
        //                    link = descendant.Element("link").Value,
        //                    description = descendant.Element("description").Value,
        //                    pubDate = descendant.Element("pubDate").Value,
        //                    //author = descendant.Element("author").Value
        //                }).ToList();
        //    return Obj;
        //}
        //#endregion
        #region กรมพัฒนาธุรกิจการค้า
        public List<dlt> Readdbd()
        {
            

            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string result = webClient.DownloadString(@"http://www.dbd.go.th/rss/group252.xml");
            XDocument document = XDocument.Parse(result);
 
            var tetet = document.Descendants("title");
         return (from descendant in document.Descendants("item")
                        select new dlt()
                        {
                            title = descendant.Element("title").Value,
                            link = descendant.Element("link").Value,
                            description = descendant.Element("description").Value,
                            pubDate = descendant.Element("pubDate").Value,
                            //author = descendant.Element("author").Value
                             copyright = "กรมการประกันภัย"
                        }).ToList();
            
        }
        #endregion

        
        //// GET api/enews/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/enews
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/enews/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/enews/5
        //public void Delete(int id)
        //{
        //}
    }
}
