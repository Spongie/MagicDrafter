using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MagicDrafterCore;
using MagicDrafterWeb.Services;

namespace MagicDrafterWeb.Controllers
{
    public class MatchesController : Controller
    {
        private readonly IDraftService service;

        public MatchesController(IDraftService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            return View(service.GetDraft(Session.SessionID));
        }

        public ActionResult StartDraft(int rounds)
        {
            var draft = service.GetDraft(Session.SessionID);
            draft.NumberOfRounds = rounds;
            draft.Start(false);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult ReportMatch(Match match)
        {
            throw new NotImplementedException();
        }
    }
}