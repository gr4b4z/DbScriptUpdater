using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DbUpdateApp
{
    public class ScriptBase : IScriptBase
    {
        
        private IFiles files;
        public ScriptBase(IFiles files)
        {
            this.files = files;
        }


        public IEnumerable<ScriptFile> GetOrderedFiles(string startForm)
        {
            var listOfFiles =  files.Files.ToList().Select(r=>new ScriptFile(r)).ToList();
            listOfFiles.Sort((i1, i2) => i1.CompareTo(i2));
            return listOfFiles.Select(r => r);
        }

        public string GetContent(ScriptFile file)
        {
            return files.ReadContent(file.Name);
        }


      
    }
}