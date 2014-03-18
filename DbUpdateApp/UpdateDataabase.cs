using System.Linq;
using DbUpdateApp.FileService;
using DbUpdateApp.Interfaces;

namespace DbUpdateApp
{
    public class UpdateDataabase
    {
        private readonly RunParameters _runParameters;

        public UpdateDataabase(RunParameters runParameters)
        {
            _runParameters = runParameters;
        }

        public void Execute()
        {
            _runParameters.Cs.ToList().ForEach(e => RunScriptMode(_runParameters.Dir, e, _runParameters.MaxVersion));
        }
        static void RunScriptMode(string dirOrFile, string connectionString, string maxViersion)
        {
            var version = new DefaultDatabaseVersion(connectionString);

            //TODO: fileFactory
            IFilesService fileService;
            if (dirOrFile.EndsWith(".zip"))
                fileService = new ZipMultipleFileService(dirOrFile);
            else fileService = new MultipleFileService(dirOrFile);

            var scriptBase = new ScriptService(fileService);
            var scriptMngr = new SqlDatabaseScriptManager(connectionString);
            var u = new UpdateManager(version, scriptBase, scriptMngr);
            if (maxViersion != null)
                u.UpdateToVersion(maxViersion);
            else
                u.Update();
        }
    }
}