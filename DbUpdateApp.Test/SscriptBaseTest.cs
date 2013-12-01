using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace DbUpdateApp.Test
{
    [TestFixture]
    public class SscriptBaseTest
    {
        private IList<string> randomiezedFilesList;
        private IList<string> filesList;
        [SetUp] 
        public void InitTest()
        {
            filesList = new[]
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
            randomiezedFilesList= new[]
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
        public void Should_Return_Ordered_Files()
        {

            var files = Substitute.For<IFiles>();
            files.Files.Returns(randomiezedFilesList);

            var script = new ScriptBase(files);
            var names = script.GetOrderedFiles().Select(r => r.Name).ToList();
            for (int v = filesList.Count() - 1; v >= 0; v--)
                Assert.AreSame(filesList[v], names[v]);
        }
        [Test]
        public void Can_read_content()
        {

            var files = Substitute.For<IFiles>();
            files.Files.Returns(new[] { "file1.sql", "file2.sql" });
            files.ReadContent(Arg.Any<string>()).Returns(s =>
                s[0] + "Content"
                );

            var script = new ScriptBase(files);
            var i = script.GetOrderedFiles().ToList();


            var f1 = i.FirstOrDefault(r => r.Name == "file1.sql").Content;
            var f2 = i.FirstOrDefault(r => r.Name == "file2.sql").Content;

            Assert.AreEqual("file1.sqlContent", f1);
            Assert.AreEqual("file2.sqlContent", f2);

        }


        [Test]
        public void Content_is_lazy_loaded()
        {

            var files = Substitute.For<IFiles>();
            files.Files.Returns(new[] { "file1.sql", "file2.sql" });
            int l = 0;

            files.ReadContent(Arg.Any<string>()).Returns(s =>
                s[0] + "Content").AndDoes(info =>l++ );

            var script = new ScriptBase(files);
            var i = script.GetOrderedFiles();

            var f1 = i.First();
            var valueAfter1 = l;
            var f2 = i.ToList();
            var valueAfter2 = l;
            
            Assert.AreEqual(1,valueAfter1);
            Assert.AreEqual(3,valueAfter2);

        }




    }
}