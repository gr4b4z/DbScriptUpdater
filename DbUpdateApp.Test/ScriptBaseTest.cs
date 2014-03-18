using System.Collections.Generic;
using System.Linq;
using DbUpdateApp.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace DbUpdateApp.Test
{
    [TestFixture]
    public partial class ScriptBaseTest
    {
        private IList<string> _randomiezedFilesList;
        private IList<string> _filesList;
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
                "3.added stored proc.sql",
                "4.procedure uptaed.sql",
            };
            _randomiezedFilesList= new[]
            {
                "2.1.1.fixBug134Model.sql",
                "4.procedure uptaed.sql",
                "1.firstScript.sql",
                "2.updatedModel.sql",
                "2.1.fixBug123Model.sql",
                "2.3.extendedJobField.sql",
                "3.added stored proc.sql",
                "2.1.3.1.fixBug134Model.sql",
                "2.1.3.2.changedViewModel.sql",
                "2.2.changedRemovedList.sql",
                "2.1.2.fixBug134Model.sql",
                "2.1.3.fixBug134Model.sql",
            };
        
        }
      
        [Test]
        public void Can_read_content()
        {

            var files = Substitute.For<IFilesService>();
            files.Files.Returns(new[] { "file1.sql", "file2.sql" });

            files.ReadContent(Arg.Any<string>()).Returns(s =>s[0] + "Content");

            var script = new ScriptService(files);
            var i = script.GetOrderedFiles().ToList();


            var f1 = script.GetContent(new ScriptVersion("file1.sql"));
            var f2 = script.GetContent(new ScriptVersion("file2.sql"));
            

            Assert.AreEqual("file1.sqlContent", f1);
            Assert.AreEqual("file2.sqlContent", f2);

        }

    }
}