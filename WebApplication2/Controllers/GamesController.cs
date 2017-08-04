using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class GamesController : Controller
    {
        // GET: Games
        public ActionResult Shooting()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var gamelist = from g in db.game
                           where g.game_type == "射击游戏"
                           select g;
            if (gamelist.FirstOrDefault() == null)
            {
                ViewBag.Game_Search_State = false;
            }
            else
            {
                ViewBag.Game_Serach_State = true;
                List<game> G = new List<game>();
                    ViewBag.Game_Serach_State = true;
                    List<game> SG = new List<game>();

                    foreach (var sg in gamelist)
                    {
                        SG.Add(sg);
                    }
                    ViewData["GameList"] = SG;

            }
            return View();
        }
        public ActionResult RPG() {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var gamelist = from g in db.game
                           where g.game_type == "角色扮演"
                           select g;
            if (gamelist.FirstOrDefault() == null)
            {
                ViewBag.Game_Search_State = false;
            }
            else
            {
                ViewBag.Game_Serach_State = true;
                List<game> G = new List<game>();
            
                    ViewBag.Game_Serach_State = true;
                    List<game> RPG = new List<game>();

                    foreach (var sg in gamelist)
                    {
                        RPG.Add(sg);
                    }
                    ViewData["GameList"] =RPG;

            }
            return View();
        }
        public ActionResult Sports() {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var gamelist = from g in db.game
                           where g.game_type == "体育游戏"
                           select g;
            if (gamelist.FirstOrDefault() == null)
            {
                ViewBag.Game_Search_State = false;
            }
            else
            {
                ViewBag.Game_Serach_State = true;
                List<game> SPG = new List<game>();
                    
                    foreach (var sg in gamelist)
                    {
                        SPG.Add(sg);
                    }
                    ViewData["GameList"] = SPG;

            }
            return View();
        }
        public ActionResult GameDisplay(string id)
        {
            if (id == null || id == "")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                GameWebSiteEntities db = new GameWebSiteEntities();
                var GameDisplayList = from g in db.game
                                      where g.game_name == id
                                      select g;
                var Gamedisplay = GameDisplayList.FirstOrDefault();
                if (Gamedisplay == null)
                {
                    Response.Write("<script>alert('游戏不存在')</script>");
                }
                else
                {
                    ViewBag.GameDisplay_State = true;
                    ViewData["GameDisplay"] = Gamedisplay;
                }
            }
            return View();
        }
    }

    }
