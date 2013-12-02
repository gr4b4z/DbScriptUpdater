﻿using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace DbUpdateApp.Test
{
    [TestFixture]
    public class UpdateManagerTest
    {
        private IOrderedEnumerable<ScriptFile> orderedScriptFiles;
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
            orderedScriptFiles = filesList.Select(s => new ScriptFile(s)).OrderBy(e => e);
        }
        [Test]
        public void should_read_all_files()
        {
            var readed = new List<string>();
            var scriptBase = Substitute.For<IScriptBase>();
            scriptBase.GetOrderedFiles().Returns(orderedScriptFiles);
            scriptBase.GetContent(Arg.Any<ScriptFile>()).Returns("").AndDoes(e =>
                readed.Add(((ScriptFile)e.Args()[0]).Name)
                );

            var scriptMngr = Substitute.For<ISqlScriptManager>();
            scriptMngr.RunScript(Arg.Any<string>());

            var version = Substitute.For<IVersion>();
            version.GetVersion().Returns("0");
            var um = new UpdateManager(version, scriptBase, scriptMngr);



            um.Update();

            Assert.That(filesList, Is.EquivalentTo(readed));

        }


        [Test]
        public void should_read_last_5()
        {
            var readed = new List<string>();
            var scriptBase = Substitute.For<IScriptBase>();
            scriptBase.GetOrderedFiles().Returns(orderedScriptFiles);
            scriptBase.GetContent(Arg.Any<ScriptFile>()).Returns("").AndDoes(e => readed.Add(((ScriptFile)e.Args()[0]).Name));

            var scriptMngr = Substitute.For<ISqlScriptManager>();
            scriptMngr.RunScript(Arg.Any<string>());

            var version = Substitute.For<IVersion>();
            version.GetVersion().Returns("2.1.3.2.changedViewModel.sql");

            var um = new UpdateManager(version, scriptBase, scriptMngr);



            um.Update();

            Assert.That(filesList.Skip(7), Is.EquivalentTo(readed));
            Assert.That(5, Is.EqualTo(readed.Count));

        }

        [Test]
        public void should_update_to_correct_version()
        {
            var readed = new List<string>();
            var saved = new List<string>();
            var scriptBase = Substitute.For<IScriptBase>();
            scriptBase.GetOrderedFiles().Returns(orderedScriptFiles);
            scriptBase.GetContent(Arg.Any<ScriptFile>()).Returns("").AndDoes(e => readed.Add(((ScriptFile)e.Args()[0]).Name));

            var scriptMngr = Substitute.For<ISqlScriptManager>();
            scriptMngr.RunScript(Arg.Any<string>());

            var version = Substitute.For<IVersion>();
            version.GetVersion().Returns("2.1.3.changedViewModel.sql");
            version.SaveVersion(Arg.Do<string>(saved.Add));

            var um = new UpdateManager(version, scriptBase, scriptMngr);

            um.UpdateToVersion("2.3");

            Assert.AreEqual(5,readed.Count);
            Assert.AreEqual(5, saved.Count);

            for (int v = readed.Count() - 1; v >= 0; v--)
                Assert.AreSame(readed[v], saved[v]);

        }

        [Test]
        public void should_save_correct_version()
        {
            var readed = new List<string>();
            var saved = new List<string>();
            var scriptBase = Substitute.For<IScriptBase>();
            scriptBase.GetOrderedFiles().Returns(orderedScriptFiles);
            scriptBase.GetContent(Arg.Any<ScriptFile>()).Returns("").AndDoes(e => readed.Add(((ScriptFile)e.Args()[0]).Name));

            var scriptMngr = Substitute.For<ISqlScriptManager>();
            scriptMngr.RunScript(Arg.Any<string>());

            var version = Substitute.For<IVersion>();
            version.GetVersion().Returns("0");
            version.SaveVersion(Arg.Do<string>(saved.Add));

            var um = new UpdateManager(version, scriptBase, scriptMngr);



            um.Update();

            for (int v = readed.Count() - 1; v >= 0; v--)
                Assert.AreSame(readed[v], saved[v]);

        }

        [Test]
        public void should_save_last_correct_version_and_stop_updating_when_exception_occurs()
        {
            var scriptBase = Substitute.For<IScriptBase>();
            scriptBase.GetOrderedFiles().Returns(orderedScriptFiles);
            scriptBase.GetContent(Arg.Any<ScriptFile>()).Returns(
                        info =>
                        {
                            if (((ScriptFile)info.Args()[0]).Name == "2.1.3.fixBug134Model.sql")
                                throw new ScriptFileException(((ScriptFile)info.Args()[0]));
                            return "";
                        }
                );

            var scriptMngr = Substitute.For<ISqlScriptManager>();
            scriptMngr.RunScript(Arg.Any<string>());

            var version = Substitute.For<IVersion>();
            version.GetVersion().Returns("0");
            string lastVersion = null;
            version.SaveVersion(Arg.Do<string>(e => lastVersion = e));

            var um = new UpdateManager(version, scriptBase, scriptMngr);



            um.Update();

            Assert.AreEqual("2.1.2.fixBug134Model.sql", lastVersion);

        }




    }
}