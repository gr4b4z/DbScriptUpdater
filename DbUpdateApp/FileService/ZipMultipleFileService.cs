using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbUpdateApp.Interfaces;
using Ionic.Zip;

namespace DbUpdateApp.FileService
{
    public class ZipMultipleFileService : IFilesService
    {
        private readonly string _zipFileLocation;
        private string _tempFolderPath;

        public ZipMultipleFileService(string zipFileLocation)
        {
            _zipFileLocation = zipFileLocation;
        }

        private static string TempPath()
        {
            string temp = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(temp);
            return temp;
        }
        private void ExtractFiles()
        {
            _tempFolderPath = TempPath();
            using (ZipFile zip = ZipFile.Read(_zipFileLocation))
            {
                zip.ExtractAll(_tempFolderPath,ExtractExistingFileAction.OverwriteSilently);
            }
        }
        private IDictionary<string,string> _files;

        private IDictionary<string, string> Getfiles
        {
            get{ return (_files?? (_files=ReadFiles()));}
        }
        private IDictionary<string, string> ReadFiles()
        {
            Dictionary<string, string> files;
            using (ZipFile zip = ZipFile.Read(_zipFileLocation))
            {
                files = zip.EntryFileNames
                    .Select(e => new {fullname = e, name = e.Substring(e.LastIndexOf('/') + 1)})
                    .Where(e => e.name.EndsWith(".sql"))
                    .ToDictionary(k => k.name, v => v.fullname);
                 

            }
            if (!files.Any()) throw new NoSqlFilesException();
            return files;

        }
    
        public IEnumerable<string> Files
        {
            get
            {
                return (Getfiles).Keys;
            }
        }
        public string ReadContent(string file)
        {
            if (_tempFolderPath == null) ExtractFiles();

            var fullPath = Path.Combine(_tempFolderPath, Getfiles[file]);
            return File.OpenText(fullPath).ReadToEnd();
        }
    }
}