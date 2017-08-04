using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Login() {
            if (Session["admin_id"] != null)
            {
                Response.Write("<script>alert('您已登陆')</script>");
                return RedirectToAction("Home","Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string admin_id = fc["admin_id"];
            string admin_pwd = fc["admin_pwd"];
            GameWebSiteEntities db = new GameWebSiteEntities();
            var adminlist = from ad in db.admin
                            where ad.admin_id == admin_id && ad.admin_pwd == admin_pwd
                            select ad;
            admin adminInfo = adminlist.FirstOrDefault();
            if (adminInfo != null) {
                Session["admin_id"] = adminInfo.admin_id;
                 return RedirectToAction("Home");
            }
            else
            {
                Response.Write("<script>alert('用户名或密码错误，请重新登陆')</script>");
                return View();
            }
        }
        public ActionResult Register() {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection fc) {
            string admin_id = fc["admin_id"];
            string admin_pwd = fc["admin_pwd"];
            GameWebSiteEntities db = new GameWebSiteEntities();
            var adminlist = from ad in db.admin
                            where ad.admin_id == admin_id
                            select ad;
            if (adminlist.FirstOrDefault() != null)
            {
                Response.Write("<script>alert('该用户已注册')</script>");
            }
            else if (fc["admin_id"]=="") {
                Response.Write("<script>alert('账户名不能为空，请重新输入')</script>");
            }
            else if (fc["admin_pwd"] =="")
            {
                Response.Write("<script>alert('密码不能为空，请重新输入')</script>");
            }
            else if (fc["admin_id"].Length < 6)
            {
                Response.Write("<script>alert('账户名过短，不能小于6位，请重新输入')</script>");
            }
            else if (fc["admin_id"].Length > 15)
            {
                Response.Write("<script>alert('账户名过长，不能超过15位，请重新输入')</script>");
            }
            else if (fc["admin_pwd"].Length < 6)
            {
                Response.Write("<script>alert('密码过短,不能小于6位，请重新输入')</script>");
            }
            else if (fc["admin_pwd"].Length > 15)
            {
                Response.Write("<script>alert('密码过长，，请重新输入')</script>");
            }
            else if (fc["admin_pwd"] != fc["admin_pwd1"])
            {
                Response.Write("<script>alert('两次密码不一致，请重新输入')</script>");
            }
            else
            {
                admin a = new admin();
                a.admin_id = admin_id;
                a.admin_pwd = admin_pwd;
                db.admin.Add(a);
                db.SaveChanges();
                ViewBag.Admin_Register_State = true;
                return RedirectToAction("Login","Admin");
            }
            return View();
        }
        public ActionResult UpGame()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpGame(FormCollection fc) {
            game NewGame = new game();
            if (fc["game_name"] =="")
            {
                Response.Write("<script>alert('游戏名不能为空')</script>");
            }
            else if (fc["game_intro"] =="")
            {
                Response.Write("<script>alert('游戏简介不能为空')</script>");
            }
            else if (fc["game_content"] =="")
            {
                Response.Write("<script>alert('游戏内容不能为空')</script>");
            }
            else if (fc["game_pic"] == "")
            {
                Response.Write("<script>alert('游戏封面不能为空')</script>");
            }
            else if (fc["game_pic_show"] =="")
            {
                Response.Write("<script>alert('游戏图片不能为空')</script>");
            }
            else if (fc["game_link"] =="")
            {
                Response.Write("<script>alert('游戏链接不能为空')</script>");
            }
            else if (fc["game_type"] =="")
            {
                Response.Write("<script>alert('游戏类型不能为空')</script>");
            }
            else
            {
                NewGame.game_name = fc["game_name"];
                NewGame.game_pic = "/Images/" + fc["game_pic"].ToString();
                NewGame.game_intro = fc["game_intro"];
                NewGame.game_content = fc["game_content"];
                NewGame.game_pic_show = "/Images/" + fc["game_pic_show"].ToString();
                NewGame.game_link = fc["game_link"];
                NewGame.game_type = fc["game_type"];
                GameWebSiteEntities db = new GameWebSiteEntities();
                try
                {
                    db.game.Add(NewGame);
                    upgame_record ugr = new upgame_record();
                    ugr.admin_id = Session["admin_id"].ToString();
                    ugr.date = DateTime.Now;
                    ugr.game_name = NewGame.game_name;
                    db.upgame_record.Add(ugr);
                    db.SaveChanges();
                    return RedirectToAction("GameList", "Admin");
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('游戏已经存在，不能重复添加')</script>");
                }
            }
            return View();
        }
        public ActionResult DelGame(string id) {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var gamelist = from g in db.game
                           where g.game_name == id
                           select g;
            try
            {
                delgame_record dgr = new delgame_record();
                dgr.admin_id = Session["admin_id"].ToString();
                dgr.date = DateTime.Now;
                dgr.game_name = gamelist.FirstOrDefault().game_name;
                db.delgame_record.Add(dgr);
                db.game.Remove(gamelist.FirstOrDefault());
                db.SaveChanges();
                
            }
            catch(Exception e)
            {
                Response.Write("<script>alert('删除失败,游戏或已不存在')</script>");
            }
            return RedirectToAction("GameList", "Admin");
        }
        public ActionResult UpNews()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpNews(FormCollection fc)
        {
            news New_News = new news();
            if (fc["news_title"] == "")
            {
                Response.Write("<script>alert('新闻标题不能为空')</script>");
            }
            else if (fc["news_pic"] == "")
            {
                Response.Write("<script>alert('新闻图片不能为空')</script>");
            }
            else if (fc["news_content"] == "")
            {
                Response.Write("<script>alert('新闻内容不能为空')</script>");
            }
            else if (fc["news_intro"] == "")
            {
                Response.Write("<script>alert('游戏简介不能为空')</script>");
            }
            else
            {
                New_News.news_title = fc["news_title"];
                New_News.news_pic = "/Images/" + fc["news_pic"].ToString();
                New_News.news_intro = fc["news_intro"];
                New_News.news_content = fc["news_content"];
                GameWebSiteEntities db = new GameWebSiteEntities();
                try
                {
                    db.news.Add(New_News);
                    upnews_record unr = new upnews_record();
                    unr.admin_id = Session["admin_id"].ToString();
                    unr.date = DateTime.Now;
                    unr.news_title = New_News.news_title;
                    db.upnews_record.Add(unr);
                    db.SaveChanges();
                    return RedirectToAction("NewsList", "Admin");
                }
                catch (Exception e)
                {
                    Response.Write("<script>alert('新闻已经存在，不能重复添加')</script>");
                    return RedirectToAction("NewsList", "Admin");
                }
            }
            return View();

        }
        public ActionResult EditGame(string id) {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GameWebSiteEntities db = new GameWebSiteEntities();
            var gamelist = from g in db.game
                           where g.game_name == id
                           select g;
            ViewData["current_game"] = gamelist.FirstOrDefault();

            return View();
        }
        [HttpPost]
        public ActionResult EditGame(FormCollection fc) {
            GameWebSiteEntities db = new GameWebSiteEntities();
            if (fc["game_name"] == "")
            {
                Response.Write("<script>alert('游戏名不能为空')</script>");
            }
            else
            {
                string game_name = fc["game_name"];
                var Gamelist = from g in db.game
                               where g.game_name == game_name
                               select g;
                ViewData["current_game"] = Gamelist.FirstOrDefault();
            }
             if (fc["game_pic"] == "")
            {
                Response.Write("<script>alert('游戏封面不能为空')</script>");
            }
            else if (fc["game_intro"] == "")
            {
                Response.Write("<script>alert('游戏简介不能为空')</script>");
            }
            else if (fc["game_content"] == "")
            {
                Response.Write("<script>alert('游戏内容不能为空')</script>");
            }
            else if (fc["game_pic_show"] == "")
            {
                Response.Write("<script>alert('游戏图片不能为空')</script>");
            }
            else if (fc["game_link"] == "")
            {
                Response.Write("<script>alert('游戏链接不能为空')</script>");
            }
            else if (fc["game_type"] == "")
            {
                Response.Write("<script>alert('游戏类型不能为空')</script>");
            }
            else
            {
                string game_name = fc["game_name"];
                var game = from g in db.game
                           where g.game_name == game_name
                           select g;
                game current = game.FirstOrDefault();
                if ( current== null)
                {
                    Response.Write("<script>alert('游戏不存在')</script>");
                }
                else
                {
                    try
                    {
                        current.game_name = fc["game_name"];
                        current.game_pic = "/Images/" + fc["game_pic"];
                        current.game_type = fc["game_type"];
                        current.game_pic_show = "/Images/" + fc["game_pic_show"];
                        current.game_intro = fc["game_intro"];
                        current.game_content = fc["game_content"];
                        current.game_link = fc["game_link"];
                        db.Entry<game>(current).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("GameList","Admin");
                        
                    }
                    catch(Exception e)
                    {
                        Response.Write("<script>alert('请填写完整游戏信息')</script>");
                    }
                }
            }
            return View();
        }
        public ActionResult EditNews(string id) {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GameWebSiteEntities db = new GameWebSiteEntities();
            var newslist = from n in db.news
                           where n.news_title == id
                           select n;
            ViewData["current_news"] = newslist.FirstOrDefault();
            return View();
        }
        [HttpPost]
        public ActionResult EditNews(FormCollection fc) {
            GameWebSiteEntities db = new GameWebSiteEntities();

            if (fc["news_title"] =="")
            {
                Response.Write("<script>alert('新闻标题不能为空')</script>");
            }
            else
            {
                string news_title = fc["news_title"];
                var Newslist = from n in db.news
                               where n.news_title == news_title
                              select n;
                ViewData["current_news"]=Newslist.FirstOrDefault();
            }
            if (fc["news_pic"] =="")
            {
                Response.Write("<script>alert('新闻图片不能为空');</script>");
            }
            else if (fc["news_content"] =="")
            {
                
                Response.Write("<script>alert('新闻内容不能为空')</script>");
            }
            else if (fc["news_intro"] =="")
            {
                Response.Write("<script>alert('游戏简介不能为空')</script>");
            }
            else
            {
                 string news_title = fc["news_title"];
                var newslist = from n in db.news
                               where n.news_title == news_title
                               select n;
                var current = newslist.FirstOrDefault();
                if (current == null)
                {
                    
                    Response.Write("<script>alert('新闻不存在')</script>");
                }
                else
                {
                        current.news_title = fc["news_title"];
                        current.news_content = fc["news_content"];
                        current.news_intro = fc["news_intro"];
                        current.news_pic = "/Images/" + fc["news_pic"];
                        db.Entry<news>(current).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("NewsList", "Admin");
                }
            }
            return View();
        }
        
        public ActionResult DelNews(string id) {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var newslist = from n in db.news
                           where n.news_title == id
                           select n;
            try
            {
                delnews_record dnr = new delnews_record();
                dnr.admin_id = Session["admin_id"].ToString();
                dnr.date = DateTime.Now;
                dnr.news_title = newslist.FirstOrDefault().news_title;
                db.delnews_record.Add(dnr);
                db.news.Remove(newslist.FirstOrDefault());
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('删除失败,新闻或已不存在')</script>");
            }
            return RedirectToAction("NewsList", "Admin");
        }
       
        public ActionResult DelUser(string id) {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var userlist = from u in db.user
                           where u.user_id == id
                           select u;
            try
            {
                deluser_record dur = new deluser_record();
                dur.admin_id = Session["admin_id"].ToString();
                dur.date = DateTime.Now;
                dur.user_id = userlist.FirstOrDefault().user_id;
                db.deluser_record.Add(dur);
                db.user.Remove(userlist.FirstOrDefault());
                db.SaveChanges();
            }
            catch
            {
                Response.Write("<script>alert('删除失败,用户已不存在')</script>");
            }
            return RedirectToAction("UserList", "Admin");

        }
        public ActionResult DelDur(int id)
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var durlist = from d in db.deluser_record
                              where  d.dur_id== id
                              select d;
            try
            {
                db.deluser_record.Remove(durlist.FirstOrDefault());
                db.SaveChanges();
            }
            catch
            {
                Response.Write("<script>alert('删除失败,记录已不存在')</script>");
            }
            return RedirectToAction("DurList", "Admin");

        }
        public ActionResult DelDnr(int id)
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var dnrlist = from d in db.delnews_record
                          where d.dnr_id == id
                          select d;
            try
            {
                db.delnews_record.Remove(dnrlist.FirstOrDefault());
                db.SaveChanges();
            }
            catch
            {
                Response.Write("<script>alert('删除失败,记录已不存在')</script>");
            }
            return RedirectToAction("DnrList", "Admin");

        }
        public ActionResult DelDgr(int id)
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var dgrlist = from d in db.delgame_record
                          where d.dgr_id == id
                          select d;
            try
            {
                db.delgame_record.Remove(dgrlist.FirstOrDefault());
                db.SaveChanges();
            }
            catch
            {
                Response.Write("<script>alert('删除失败,记录已不存在')</script>");
            }
            return RedirectToAction("DgrList", "Admin");

        }
        public ActionResult DelUgr(int id)
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var ugrlist = from d in db.upgame_record
                          where d.ugr_id == id
                          select d;
            try
            {
                db.upgame_record.Remove(ugrlist.FirstOrDefault());
                db.SaveChanges();
            }
            catch
            {
                Response.Write("<script>alert('删除失败,记录已不存在')</script>");
            }
            return RedirectToAction("UgrList", "Admin");
        }
        public ActionResult DelUnr(int id)
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var unrlist = from d in db.upnews_record
                          where d.unr_id == id
                          select d;
            try
            {
                db.upnews_record.Remove(unrlist.FirstOrDefault());
                db.SaveChanges();
            }
            catch
            {
                Response.Write("<script>alert('删除失败,记录已不存在')</script>");
            }
            return RedirectToAction("UnrList", "Admin");
        }

        public ActionResult DelComment(int id)
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var commentlist = from c in db.comment
                           where c.comment_id == id
                           select c;
            try
            {
                db.comment.Remove(commentlist.FirstOrDefault());
                db.SaveChanges();
            }
            catch
            {
                Response.Write("<script>alert('删除失败,评论已不存在')</script>");
            }
            return RedirectToAction("CommentList", "Admin");

        }
        public ActionResult Home() {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllAdmin();
            GetAllUser();
            return View();
        }
        public ActionResult UserList()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllUser();
            return View();
        }
        public ActionResult NewsList() {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllNews();
            return View();
        }
        public ActionResult GameList() {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllGames();
            return View();
        }
        public ActionResult CommentList()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllComment();
            return View();
        }
        public ActionResult DurList()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllDur();
            return View();
        }
        public ActionResult DnrList()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllDnr();
            return View();
        }
        public ActionResult DgrList()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllDgr();
            return View();
        }
        public ActionResult UgrList()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllUgr();
            return View();
        }
        public ActionResult UnrList()
        {
            if (Session["admin_id"] == null)
            {
                Response.Write("<script>alert('当前无管理员，请登陆后再试')</script>");
                return RedirectToAction("Login", "Admin");
            }
            GetAllUnr();
            return View();
        }
        public int GetUserCount() {
            GameWebSiteEntities db = new GameWebSiteEntities();
            return db.user.Count();
        }
        public int GetGameCount() {
            GameWebSiteEntities db = new GameWebSiteEntities();
            return db.game.Count();
        }
        public int GetNewsCount()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            return db.news.Count();
        }
        public int GetCommentCount()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            return db.comment.Count();
        }
        public void GetAllUser() {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var userlist = from u in db.user
                           select u;
            List<user> AllUser = new List<user>();
            foreach (var User in userlist)
            {
                AllUser.Add(User);
            }
            ViewData["AllUser"] = AllUser;
        }
        public void GetAllAdmin() {
            GameWebSiteEntities db = new GameWebSiteEntities();
            List<admin> AllAdmin = new List<admin>();
            var adminlist = from a in db.admin
                            select a;
            foreach (var Admin in adminlist)
            {
                AllAdmin.Add(Admin);
            }
            ViewData["AllAdmin"] = AllAdmin;
        }
        public void GetAllNews()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            List<news> AllNews = new List<news>();
            var newslist = from n in db.news
                            select n;
            foreach (var News in newslist)
            {
                AllNews.Add(News);
            }
            ViewData["AllNews"] = AllNews;
        }
        public void GetAllGames()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            List<game> AllGames = new List<game>();
            var gamelist = from g in db.game
                           select g;
            foreach (var game in gamelist)
            {
                AllGames.Add(game);
            }
            ViewData["AllGames"] = AllGames;
        }
        public void GetAllComment() {
            GameWebSiteEntities db = new GameWebSiteEntities();
            List<comment> AllComment = new List<comment>();
            var commentlist = from c in db.comment
                           select c;
            foreach (var comment in commentlist)
            {
                AllComment.Add(comment);
            }
            ViewData["AllComment"] = AllComment;
        }
        public void GetAllDur()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var durlist = from dur in db.deluser_record
                          select dur;
            List<deluser_record> AllDur = new List<deluser_record>();
            foreach (var Dur in durlist)
            {
                AllDur.Add(Dur);
            }
            ViewData["AllDur"] = AllDur;
        }
        public void GetAllDnr()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var dnrlist = from dnr in db.delnews_record
                          select dnr;
            List<delnews_record> AllDnr = new List<delnews_record>();
            foreach (var Dnr in dnrlist)
            {
                AllDnr.Add(Dnr);
            }
            ViewData["AllDnr"] = AllDnr;
        }
        public void GetAllDgr()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var dgrlist = from dgr in db.delgame_record
                          select dgr;
            List<delgame_record> AllDgr = new List<delgame_record>();
            foreach (var Dgr in dgrlist)
            {
                AllDgr.Add(Dgr);
            }
            ViewData["AllDgr"] = AllDgr;
        }
        public void GetAllUnr()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var unrlist = from unr in db.upnews_record
                          select unr;
            List<upnews_record> AllUnr = new List<upnews_record>();
            foreach (var Unr in unrlist)
            {
                AllUnr.Add(Unr);
            }
            ViewData["AllUnr"] = AllUnr;
        }
        public void GetAllUgr()
        {
            GameWebSiteEntities db = new GameWebSiteEntities();
            var ugrlist = from ugr in db.upgame_record
                          select ugr;
            List<upgame_record> AllUgr = new List<upgame_record>();
            foreach (var Ugr in ugrlist)
            {
                AllUgr.Add(Ugr);
            }
            ViewData["AllUgr"] = AllUgr;
        }
        public ActionResult Exit() {
            Session["admin_id"] = null;
            return RedirectToAction("Login", "Admin");
        }
    }
}