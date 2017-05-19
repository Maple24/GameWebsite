using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login() {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f) {
            GameWebSiteEntities db = new GameWebSiteEntities();
            string user_id = f["user_id"];
            string password = f["password"];
            var userlist = from u in db.user
                           where u.user_id ==user_id&&u.user_pwd==password
                           select u;
            var userinfo = userlist.FirstOrDefault();
            if (userinfo != null) {
                Session["user_id"] = userinfo.user_id;
                return RedirectToAction("Index", "Home");
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
            string user_id = fc["user_id"];
            GameWebSiteEntities db = new GameWebSiteEntities();
            var userlist = from u in db.user
                           where u.user_id == user_id
                           select u;
            if (userlist.FirstOrDefault() != null)
            {
                Response.Write("<script>alert('该用户已注册')</script>");
            }
            else if (fc["user_id"].Length < 6)
            {
                Response.Write("<script>alert('账户名过短，请重新输入')</script>");
            }
            else if (fc["user_id"].Length > 15)
            {
                Response.Write("<script>alert('账户名过长，请重新输入')</script>");
            }
            else if (fc["password"].Length < 6)
            {
                Response.Write("<script>alert('密码过短，请重新输入')</script>");
            }
            else if (fc["password"].Length > 15)
            {
                Response.Write("<script>alert('密码过长，请重新输入')</script>");
            }
            else if (fc["password"] != fc["password1"])
            {
                Response.Write("<script>alert('两次密码不一致，请重新输入')</script>");
            }
            else if ((fc["email"] == "") || fc["email"].Length < 6) {
                Response.Write("<script>alert('邮箱格式输入错误，请重新输入')</script>");
            }
            else
            {
                user userinfo = new user();
                userinfo.user_id = fc["user_id"];
                userinfo.user_pwd = fc["password"];
                userinfo.user_email = fc["email"];
                db.user.Add(userinfo);
                db.SaveChanges();
                Response.Write("<script>alert('注册成功！')</script>");
            }
            return View();
        }
        public ActionResult ForgetPwd() {
            if (Session["user_id"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwd(FormCollection fc)
        {
            string user_id = fc["user_id"];
            string user_email = fc["user_email"];
            string pwd = fc["new_password"];
            if (user_id == "") {
                Response.Write("<script>alert('用户名不能为空')</script>");
            }
            else if (user_email=="")
            {
                Response.Write("<script>alert('邮箱不能为空')</script>");
            }
            else if (pwd == "")
            {
                Response.Write("<script>alert('新密码不能为空')</script>");

            }
            else if (pwd.Length < 6)
            {
                Response.Write("<script>alert('新密码过短，请重新输入')</script>");
            }
            else if (pwd.Length > 15)
            {
                Response.Write("<script>alert('新密码过长，请重新输入')</script>");
            }
            else
            {
                GameWebSiteEntities db = new GameWebSiteEntities();
                var userlist = from u in db.user
                               where u.user_id == user_id && u.user_email == user_email
                               select u;
                user _user = userlist.FirstOrDefault();
                if (_user != null)
                {
                    if (pwd == _user.user_pwd)
                    {
                        Response.Write("<script>alert('新密码不能和旧密码一样')</script>");
                    }
                    _user.user_pwd = pwd;
                    db.Entry<user>(_user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    Response.Write("<script>alert('修改成功！')</script>");
                }
                else
                {
                    Response.Write("<script>alert('用户名或邮箱错误')</script>");
                }
            }
            return View();
        }
        public ActionResult Comment(FormCollection fc,string id) {
            string title = id;
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Login","User");
            }
            else if (fc["comment_content"] == "")
            {
                Response.Write("<script>alert('评论内容不能为空')</script>");
                return RedirectToAction("NewsDisplay","News",new { id=title});
            }
            GameWebSiteEntities db = new GameWebSiteEntities();
            comment c = new comment();
            c.comment_content = fc["comment_content"];
            c.user_id = Session["user_id"].ToString();
            c.news_title = id;
            c.date = DateTime.Now;
            db.comment.Add(c);
            db.SaveChanges();
            return RedirectToAction("NewsDisplay", "News", new { id = title });
        }
        public ActionResult Exit() {
            Session["user_id"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}