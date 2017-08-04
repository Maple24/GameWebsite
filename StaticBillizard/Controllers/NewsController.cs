using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult News()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            List<news> News=new List<news>();
            if (db.news.Count() == 0)
            {
                ViewBag.News_Search_State = false;
            }
            else {
                ViewBag.News_Search_State =true;
                
                    var newslist = db.news.Take(db.news.Count());
                    foreach (news _news in newslist)
                    {
                        News.Add(_news);
                    }
                    ViewData["NewsList"] = News;
            }
            return View();
        }
        public ActionResult NewsDisplay(string id) {
            if (id == null)
            {
                return RedirectToAction("News", "News");
            }
            GameWebSiteEntities db = new GameWebSiteEntities();
            var newslist = from n in db.news
                           where n.news_title == id
                           select n;
            var _news = newslist.FirstOrDefault();
            if (_news == null)
            {
                ViewBag.NewsDisplay_State = false;
            }
            else
            {
                ViewBag.NewsDisplay_State = true;
                ViewData["NewsDisplay"] = _news;
            }
            List<comment> CommentList = new List<comment>();
            var commentlist = from c in db.comment
                              where c.news_title == id
                              select c;
            foreach(var cm in commentlist)
            {
                CommentList.Add(cm);
            }
            ViewData["Commentlist"] = CommentList;
            return View();
        }
    }
}