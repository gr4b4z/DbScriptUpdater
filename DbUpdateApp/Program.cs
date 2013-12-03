﻿using System.Collections;
using System.Configuration;
using System.Linq;

namespace DbUpdateApp
{
    class Program
    {
        struct Args
        {
            public string Cs;
            public string Dir;
            public string MaxVersion;
        }


        static Args MapArgs(string[] args)
        {
            var arg = new Args();
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

        static void RunScriptMode(Args args)
        {
            var version = new DefaultDatabaseVersion(args.Cs);
            var scriptBase = new ScriptService(new MultipleFileService(args.Dir));
            var scriptMngr = new SqlDatabaseScriptManager(args.Cs);
            var u = new UpdateManager(version, scriptBase, scriptMngr);
            if(args.MaxVersion!=null) 
                u.UpdateToVersion(args.MaxVersion);
            else 
                u.Update();
        }
        static void AddFiles(string[] filenames)
        {


        }
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                var a = MapArgs(args);
                if (a.Cs == null && a.Dir == null)
                {
                    AddFiles(args);
                }
                else
                {
                    RunScriptMode(a);
                }

            }
            else
            {
                var a = new Args
                {
                    Dir = ConfigurationManager.AppSettings["Path"],
                };

                foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().Where(cs => cs.Name.StartsWith("Update_")))
                {
                    a.Cs = cs.ConnectionString;
                    RunScriptMode(a);
                }
            }
        }
    }
}
