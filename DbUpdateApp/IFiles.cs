using System.Collections.Generic;

namespace DbUpdateApp
{
    public interface IFiles
    {
        IEnumerable<string> Files { get; }
        string  ReadContent(string file);
    }
}