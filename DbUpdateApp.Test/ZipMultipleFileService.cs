using System.Collections.Generic;
using System.Linq;
using DbUpdateApp.FileService;
using DbUpdateApp.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace DbUpdateApp.Test
{
    [TestFixture]
    public  class ZipMultipleFileServiceTest
    {
        private string[] _filesList;
        private const string treeStructure = "Data/flat_structure.zip";
        private const string flatStructure = "Data/tree_structure.zip";
        [SetUp] 
        public void InitTest()
        {
            _filesList = new[]
            {
            
            
                "1.firstScript.sql",
                "2.updatedModel.sql",
                "2.1.fixBug123Model.sql",
                "2.1.1.fixBug134Model.sql",
                "2.1.2.fixBug134Model.sql",
                "2.1.3.fixBug134Model.sql",
                "2.1.3.1.fixBug134Model.sql",
                "2.1.3.2.changedViewModel.sql",
                "2.2.changedRemovedList.sql",
                "2.3.extendedJobField.sql",
            };
            
        
        }
        [Test]
        public void Should_return_all_files_from_flat_structure()
        {
            var z = new ZipMultipleFileService(flatStructure);

            var files = z.Files;

            Assert.That(_filesList, Is.EquivalentTo(files));
        }

        [Test]
        public void Should_return_all_files_from_tree_structure()
        {
            var z = new ZipMultipleFileService(treeStructure);

            var files = z.Files;

            Assert.That(_filesList, Is.EquivalentTo(files));
        }
        [Test]
        public void Can_read_all_files_from_flat_structure()
        {
            var z = new ZipMultipleFileService(flatStructure);

            var files = z.Files;


            foreach (var file in files)
            {
                z.ReadContent(file);
            }


            foreach (var file in _filesList)
            {
                z.ReadContent(file);
            }
            
            
        }

        [Test]
        public void Can_read_all_files_from_tree_structure()
        {
            var z = new ZipMultipleFileService(treeStructure);

            var files = z.Files;


            foreach (var file in files)
            {
                z.ReadContent(file);
            }


            foreach (var file in _filesList)
            {
                z.ReadContent(file);
            }
            
            
        }
    }
}