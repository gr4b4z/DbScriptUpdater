﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DbUpdateApp
{
    public class FilesImplementation : IFiles
    {
        private readonly string _baseDir;

        public FilesImplementation(string baseDir)
        {
            _baseDir = baseDir;
        }

        private IDictionary<string,string> _files;
        private IDictionary<string, string> ReadFiles()
        {
            var files = new DirectoryInfo(_baseDir).EnumerateFiles("*.sql", SearchOption.AllDirectories)
                .ToDictionary(k => k.Name, v => v.FullName);
            if (!files.Any()) throw new NoSqlFilesException();
            return files;

        }
        public IEnumerable<string> Files
        {
            get
            {
                return (_files?? (_files=ReadFiles())).Keys;
            }
            
        }

        public string ReadContent(string file)
        {
            var fullPath = _files[file];
            return File.OpenText(fullPath).ReadToEnd();
        }
    }
}