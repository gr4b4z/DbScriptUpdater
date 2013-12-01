using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdateApp
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class UpdateManager
    {
        private readonly IVersion _dbVersion;
        private readonly IScriptBase _scriptBase;

        public UpdateManager(IVersion dbVersion,IScriptBase scriptBase)
        {
            _dbVersion = dbVersion;
            _scriptBase = scriptBase;
        }

        public void Update()
        {
            var startFrom = _dbVersion.GetVersion();
            _scriptBase.GetOrderedFiles(startFrom);



        }
        public void UpdateToVersion(string version)
        {
           
        }
    }

    internal interface IVersion
    {
        string GetVersion();
        void SaveVersion(string version);

    }
}
