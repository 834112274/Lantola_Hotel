using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelSystem.Web.Controllers
{
    public class ArticleController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Article
        public ActionResult Index(string id)
        {
            switch (id)
            {
                case "Advertisement":
                    var Advertisement = from m in DbContext.Article where m.Type == 1 select m;
                    ViewBag.Title = "LANTOLA广告业务";
                    if (Advertisement.Count() > 0)
                    {
                        return View(Advertisement.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 1, Content = "", });
                    }

                case "About":
                    var About = from m in DbContext.Article where m.Type == 2 select m;
                    ViewBag.Title = "关于LANTOLA";
                    if (About.Count() > 0)
                    {
                        return View(About.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 2, Content = "", });
                    }

                case "Safe":
                    var Safe = from m in DbContext.Article where m.Type == 3 select m;
                    ViewBag.Title = "LANTOLA（郎拓拉）隐私政策";
                    if (Safe.Count() > 0)
                    {
                        return View(Safe.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 3, Content = "", });
                    }

                case "Help":
                    var Help = from m in DbContext.Article where m.Type == 4 select m;
                    ViewBag.Title = "LANTOLA（郎拓拉）服务条款";
                    if (Help.Count() > 0)
                    {
                        return View(Help.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 4, Content = "", });
                    }

                case "License":
                    var License = from m in DbContext.Article where m.Type == 5 select m;
                    ViewBag.Title = "营业执照";
                    if (License.Count() > 0)
                    {
                        return View(License.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 5, Content = "", });
                    }

                case "Complaint":
                    var Complaint = from m in DbContext.Article where m.Type == 6 select m;
                    ViewBag.Title = "投诉与建议";
                    if (Complaint.Count() > 0)
                    {
                        return View(Complaint.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 6, Content = "", });
                    }
                default:
                    var article = from m in DbContext.Article where m.Id == id select m;

                    if (article.Count() > 0)
                    {
                        return View(article.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 0, Content = "", });
                    }
            }
        }
    }
}