using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MagicDrafterWeb.Controllers
{
    public class MatchController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportMatch()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}