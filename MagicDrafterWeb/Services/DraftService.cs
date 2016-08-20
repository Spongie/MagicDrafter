using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagicDrafterCore;

namespace MagicDrafterWeb.Services
{
    public class DraftService : IDraftService
    {
        private static Dictionary<string, Draft> drafts = new Dictionary<string, Draft>();

        public Draft GetDraft(string id)
        {
            if (!drafts.ContainsKey(id))
                drafts.Add(id, new Draft());

            return drafts[id];
        }

        public void SaveDraft(Draft draft, string id)
        {
            drafts[id] = draft;
        }
    }
}