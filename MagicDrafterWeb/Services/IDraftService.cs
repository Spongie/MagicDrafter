using MagicDrafterCore;

namespace MagicDrafterWeb.Services
{
    public interface IDraftService
    {
        Draft GetDraft(string id);
        void SaveDraft(Draft draft, string id);
    }
}