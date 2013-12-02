using System.Collections;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdateApp
{
    public interface IScriptBase
    {
        IOrderedEnumerable<ScriptFile> GetOrderedFiles();
        string GetContent(ScriptFile file);
    }
}
