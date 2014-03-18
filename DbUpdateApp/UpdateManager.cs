using System;
using System.Linq;
using DbUpdateApp.Interfaces;

namespace DbUpdateApp
{
    public class UpdateManager:IDisposable
    {
        private readonly IDatabaseVersion _dbDatabaseVersion;
        private readonly IScriptService _iscriptService;
        private readonly IDatabaseScriptManager _scriptManager;

        public UpdateManager(IDatabaseVersion dbDatabaseVersion,IScriptService iscriptService,IDatabaseScriptManager scriptManager)
        {
            _dbDatabaseVersion = dbDatabaseVersion;
            _iscriptService = iscriptService;
            _scriptManager = scriptManager;
        }

        public void Update()
        {
            UpdateToSpecifiedVersion();
        }
        private void UpdateToSpecifiedVersion(ScriptVersion endOn = null)
        {
            var startFrom = _dbDatabaseVersion.GetVersion();
            Console.WriteLine("Database is in version: "+startFrom);
            var s = new ScriptVersion(startFrom);
            var files = _iscriptService.GetOrderedFiles().Where(e => e.CompareTo(s) > 0);
            if (endOn != null) files = files.Where(r => r.CompareTo(endOn) <= 0);
            try
            {
                foreach (var scriptFile in files)
                {
                    Console.WriteLine("Starting updating to version : " + scriptFile.Name);
                    
                    _scriptManager.RunScript(_iscriptService.GetContent(scriptFile));
                    _dbDatabaseVersion.SaveVersion(scriptFile.Name);
                    
                    Console.WriteLine("Db updated to version : " + scriptFile.Name);
                }
            }
            catch (ScriptFileException exc)
            {
                Console.WriteLine("There was problem with file "+exc.Message);
            }
        }
        public void UpdateToVersion(string version)
        {
            var sf = new ScriptVersion(version);
            UpdateToSpecifiedVersion(sf);
        }

        public void Dispose()
        {
            _scriptManager.Dispose();
        }
    }
}