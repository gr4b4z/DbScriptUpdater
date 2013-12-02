using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdateApp
{
    public interface IFiles
    {
        IEnumerable<string> Files { get; set; }
        string  ReadContent(string file);
    }

    public interface IScriptBase
    {
        IOrderedEnumerable<ScriptFile> GetOrderedFiles();
        string GetContent(ScriptFile file);
    }
}
