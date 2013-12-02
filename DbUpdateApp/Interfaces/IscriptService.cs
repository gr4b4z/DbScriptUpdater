using System.Linq;

namespace DbUpdateApp.Interfaces
{
    public interface IScriptService
    {
        IOrderedEnumerable<ScriptVersion> GetOrderedFiles();
        string GetContent(ScriptVersion version);
    }
}
