using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index() {
            GetNews();
            GetGames();
            return View();
        }
       public void GetGames()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            List<game> Home_Games = new List<game>();
            if (db.game.Count() >= 5) {
                var gamelist = db.game.Take(5);
                foreach(var g in gamelist)
                {
                    Home_Games.Add(g);
                }
            }
            else if (db.game.Count() > 0) {
                var gamelist = from game in db.game
                               select game;
                foreach (var g in gamelist)
                {
                    Home_Games.Add(g);
                }
            }
            else
            {
                Home_Games = null;
            }
            ViewData["Home_Games"] = Home_Games;
        }
        public void GetNews()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            List<news> Home_News = new List<news>();
            if (db.news.Count() >= 6)
            {
                var newslist = db.news.Take(5);
                foreach (var g in newslist)
                {
                    Home_News.Add(g);
                }
            }
            else if (db.news.Count() > 0)
            {
                var newslist = from news in db.news
                               select news;
                foreach (var g in newslist)
                {
                    Home_News.Add(g);
                }
            }
            else
            {
                Home_News = null;
            }
            ViewData["Home_News"] = Home_News;
        }
        [HttpPost]
        public ActionResult FindGame(FormCollection fc) {
            string GameName = fc["FindGameName"];
            return RedirectToAction("GameDisplay", "Games",new { id=GameName});
        }
    }
}