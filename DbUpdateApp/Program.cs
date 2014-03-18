using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using DbUpdateApp.FileService;
using DbUpdateApp.Interfaces;
using Ionic.Zip;

namespace DbUpdateApp
{
    public enum StartMode
    {
        AddFile, UpdateDb
    }
    public enum ConfigLocation
    {
        Args, File,
        None
    }
    public struct RunParameters
    {
        public string[] Cs;
        public string Dir;
        public string MaxVersion;
    }

    public class AppStartParameters
    {
        public RunParameters SettingsFileParameters()
        {
            return new RunParameters
            {
                Dir = ConfigurationManager.AppSettings["Path"],
                Cs = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>()
                    .Where(cs => cs.Name.StartsWith("Update_")).Select(c => c.ConnectionString).ToArray(),
            };
        }
        public RunParameters ComandLineParameters(string[] args)
        {
            var arg = new RunParameters();
            for (int a = 0; a < args.Length; a++)
            {
                switch (args[a])
                {
                    case "-cs":
                        arg.Cs = args[++a];
                        break;
                    case "-path":
                        arg.Dir = args[++a];
                        break;
                    case "-max":
                        arg.MaxVersion = args[++a];
                        break;
                }
            }
            return arg;
        }
    }
    class Program
    {

        static void RunScriptMode(string dirOrFile,string connectionString,string maxViersion)
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




        private static void AddFiles(string[] filenames, RunParameters runParameters)
        {
            var sqlFilesLocation = ConfigurationManager.AppSettings["Path"] ?? Util.AssemblyDirectory;
            IFilesService fileService = new MultipleFileService(sqlFilesLocation);
            var lastVersion = new ScriptService(fileService).GetOrderedFiles().Last().Version;


            foreach (var fn in filenames)
            {
                lastVersion = ScriptVersion.NewVersionNumber(lastVersion);
                var prefix = string.Join(".", lastVersion)+".";
                if (fn.StartsWith("d%"))
                {
                    prefix = prefix + DateTime.Now.ToString("yyyyMMdd") + "." + fn.Substring(2);
                }
                else
                {
                    prefix = prefix + fn;
                }
            }



        }


        public static StartMode StartMode =StartMode.UpdateDb;
        public static ConfigLocation ConfigLocation = ConfigLocation.None;
        static void Main(string[] args)
        {
            RunParameters runParameters;

            if (args.Length > 0 && args.All(f => f.EndsWith(".sql")))
                {
                    StartMode = StartMode.AddFile;
                    ConfigLocation = ConfigLocation.File;
                    runParameters = new AppStartParameters().SettingsFileParameters();    
                }
            else {
                if(args.Length>0)
                {
                    runParameters = new AppStartParameters().ComandLineParameters(args);    
                    StartMode = StartMode.UpdateDb;
                    ConfigLocation = ConfigLocation.File;
                }
            else
                {
                    runParameters = new AppStartParameters().SettingsFileParameters();
                StartMode = StartMode.UpdateDb;
                ConfigLocation = ConfigLocation.Args;
            }}





            if (StartMode == StartMode.AddFile)
            {
                AddFiles(args,runParameters);
            }
            else
            {
                runParameters.Cs.ToList().ForEach(e=>
                    RunScriptMode(runParameters.Dir,e,runParameters.MaxVersion)
                    );
            }



        }
    }
}
