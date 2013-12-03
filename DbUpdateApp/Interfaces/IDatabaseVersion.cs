using System.Collections.Generic;
using Simple.Data.Ado.Schema;

namespace DbUpdateApp
{
    public interface IDatabaseVersion
    {
        string GetVersion();
        void SaveVersion(string version);
    }
}