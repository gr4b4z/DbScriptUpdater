using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

    class Program
    {


        private static void AddFiles(IEnumerable<string> filenames)
        {

            var sqlFilesLocation = Directory.Exists(ConfigurationManager.AppSettings["Path"]) ? ConfigurationManager.AppSettings["Path"] : Util.AssemblyDirectory;

            IFilesService fileService = new MultipleFileService(sqlFilesLocation);
            var lastVersion = new[] { 0 };
            if (fileService.Files.Any())
                lastVersion = new ScriptService(fileService).GetOrderedFiles().Last().Version;

            foreach (var fn in filenames)
            {
                var fileName = Path.GetFileName(fn);
                lastVersion = ScriptVersion.NewVersionNumber(lastVersion);
                var prefix = string.Join(".", lastVersion).TrimEnd(new[] { '0', '.' }) + ".";
                if (fileName.StartsWith("d%"))
                {
                    prefix = prefix + "d" + DateTime.Now.ToString("yyyyMMdd") + "." + fileName.Substring(2);
                }
                else
                {
                    prefix = prefix + fileName;
                }
                File.Copy(fn, sqlFilesLocation + "/" + prefix);
            }
        }


        public static StartMode StartMode = StartMode.UpdateDb;
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
            else
            {
                if (args.Length > 0)
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
                }
            }





            if (StartMode == StartMode.AddFile)
            {
                Console.WriteLine("Add new file to repository");
                AddFiles(args);
            }
            else
            {
                new UpdateDataabase(runParameters).Execute();
            }



        }
    }
}
