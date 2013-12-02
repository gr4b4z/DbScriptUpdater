using System;

namespace DbUpdateApp
{
    public interface ISqlScriptManager:IDisposable
    {
        void RunScript(string p);
    }
}