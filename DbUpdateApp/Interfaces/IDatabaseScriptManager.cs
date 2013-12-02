using System;

namespace DbUpdateApp
{
    public interface IDatabaseScriptManager:IDisposable
    {
        void RunScript(string p);
    }
}