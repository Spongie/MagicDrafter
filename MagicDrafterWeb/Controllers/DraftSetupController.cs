using System.Web.Mvc;
using MagicDrafterCore;
using MagicDrafterWeb.Services;

namespace MagicDrafterWeb.Controllers
{
    public class DraftSetupController : Controller
    {
        private readonly IDraftService service;

        public DraftSetupController(IDraftService service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            return View(service.GetDraft(Session.SessionID));
        }

        [HttpPost]
        public ActionResult AddPlayer(string name)
        {
            var draft = service.GetDraft(Session.SessionID);
                draft.Players.Add(new Player(name));
            service.SaveDraft(draft, Session.SessionID);

            return RedirectToAction(nameof(Index));
        }
    }
}