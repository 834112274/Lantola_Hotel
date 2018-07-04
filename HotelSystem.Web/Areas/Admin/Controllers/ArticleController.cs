using HotelSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HotelSystem.Web.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {
        DBModelContainer DbContext = new DBModelContainer();
        // GET: Admin/Article
        [Login(Area = "Admin", Role = "system")]
        public ActionResult Manage(string id)
        {
            switch (id)
            {
                case "Advertisement":
                    var Advertisement = from m in DbContext.Article where m.Type == 1 select m;

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

                    if (Complaint.Count() > 0)
                    {
                        return View(Complaint.First());
                    }
                    else
                    {
                        return View(new Article() { Type = 6, Content = "", });
                    }
                default:
                    var article = from m in DbContext.Article where m.Id==id select m;

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
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(Article article)
        {
            if (string.IsNullOrEmpty(article.Id))
            {
                article.Id = Guid.NewGuid().ToString();
                article.IsVaid = true;
                article.CreateTime = DateTime.Now;
                article.CreateUser = SessionInfo.systemUser.Id;
                article.UpdateTime = DateTime.Now;
                article.UpdateUser = SessionInfo.systemUser.Id;
                DbContext.Article.Add(article);
            }
            else
            {
                var a = DbContext.Article.Single(m => m.Id == article.Id);
                a.Content = article.Content;
                a.UpdateTime = DateTime.Now;
                a.UpdateUser = SessionInfo.systemUser.Id;
            }
            try
            {
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("Manage", "Article", new { id = article.Id });
        }
    }
}