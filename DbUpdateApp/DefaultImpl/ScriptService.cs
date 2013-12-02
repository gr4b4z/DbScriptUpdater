using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DbUpdateApp.Interfaces;

namespace DbUpdateApp
{
    public class ScriptService : IScriptService
    {
        
        private readonly IFilesService _filesService;
        public ScriptService(IFilesService filesService)
        {
            this._filesService = filesService;
        }


        public IOrderedEnumerable<ScriptVersion> GetOrderedFiles()
        {
            var listOfFiles =  _filesService.Files.ToList().Select(r=>new ScriptVersion(r)).ToList();
            var ordered =  listOfFiles.AsEnumerable().OrderBy(e => e);
            return ordered;
        }

        public string GetContent(ScriptVersion version)
        {
            return _filesService.ReadContent(version.Name);
        }

      
    }
}