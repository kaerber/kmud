using Kaerber.MUD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAdmin.Controllers {
    public class AreaController : Controller {
        // GET: Area
        public ActionResult Index() {
            return View( new List<Area>() );
        }
    }
}