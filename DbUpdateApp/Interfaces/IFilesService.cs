using System.Collections.Generic;

namespace DbUpdateApp.Interfaces
{
    public interface IFilesService
    {
        IEnumerable<string> Files { get; }
        string  ReadContent(string file);
    }
}