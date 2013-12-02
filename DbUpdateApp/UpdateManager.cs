using System;
using System.Linq;

namespace DbUpdateApp
{
    public class UpdateManager:IDisposable
    {
        private readonly IVersion _dbVersion;
        private readonly IScriptBase _scriptBase;
        private readonly ISqlScriptManager _scriptManager;

        public UpdateManager(IVersion dbVersion,IScriptBase scriptBase,ISqlScriptManager scriptManager)
        {
            _dbVersion = dbVersion;
            _scriptBase = scriptBase;
            _scriptManager = scriptManager;
        }

        public void Update()
        {
            UpdateToSpecifiedVersion();
        }
        private void UpdateToSpecifiedVersion(ScriptFile endOn = null)
        {
            var startFrom = _dbVersion.GetVersion();
            var s = new ScriptFile(startFrom);
            var files = _scriptBase.GetOrderedFiles().Where(e => e.CompareTo(s) >= 0);
            if (endOn != null) files = files.Where(r => r.CompareTo(endOn) <= 0);
            try
            {
                foreach (var scriptFile in files)
                {
                    _scriptManager.RunScript(_scriptBase.GetContent(scriptFile));
                    _dbVersion.SaveVersion(scriptFile.Name);
                }
            }
            catch (ScriptFileException exc)
            {
                Console.WriteLine("There was problem with file");
            }
        }
        public void UpdateToVersion(string version)
        {
            var sf = new ScriptFile(version);
            UpdateToSpecifiedVersion(sf);
        }

        public void Dispose()
        {
            _scriptManager.Dispose();
        }
    }
}