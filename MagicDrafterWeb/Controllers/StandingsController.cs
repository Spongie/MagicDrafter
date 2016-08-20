using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicDrafterWeb.Services;

namespace MagicDrafterWeb.Controllers
{
    public class StandingsController : Controller
    {
        private readonly IDraftService service;

        public StandingsController(IDraftService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            return View(service.GetDraft(Session.SessionID));
        }
    }
}