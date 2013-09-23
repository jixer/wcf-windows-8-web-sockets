using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Myers.WebSockDemo.Client.Web.Models;

namespace Myers.WebSockDemo.Client.Web.Controllers
{
    public class ChatController : Controller
    {
        //
        // GET: /Chat/Login

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login(LoginViewModel vm)
        {
            return RedirectToAction("ChatRoom", new { username = vm.Username });
        }

        public ActionResult ChatRoom(string username)
        {
            return View((object)username);
        }
    }
}
