using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DbUpdateApp
{
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
                        var connectionStrings = new List<string>();
                        do
                        {
                            connectionStrings.Add(args[++a]);
                        } while (a + 1 < args.Length && !args[a + 1].Contains("-"));
                        arg.Cs = connectionStrings.ToArray();
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
}