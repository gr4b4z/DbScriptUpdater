using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using DbUpdateApp.Interfaces;
using NSubstitute;
using NUnit.Framework;
using Simple.Data;

namespace DbUpdateApp.Test
{
    [TestFixture]
    public class DefaultDatabaseVersionTest
    {
        private dynamic db;
        [SetUp] 
        public void InitTest()
        {
            Database.UseMockAdapter(new InMemoryAdapter());
            db = Database.Open();
            db.Configuration.Insert(Id:1, Key: "DbVersion",Value:"1.3.4");
        }
        [Test]
        public void Should_read_correct_version_from_database()
        {
            var defaultDbVersionb = new DefaultDatabaseVersion("");

            var version = defaultDbVersionb.GetVersion();

            Assert.That("1.3.4", Is.EqualTo(version));
        }

        [Test]
        public void Should_write_correct_version_to_database()
        {
            var defaultDbVersionb = new DefaultDatabaseVersion("");


            defaultDbVersionb.SaveVersion("1.5.6.file.sql");

            var rec = db.Configuration.FindById(1);
            Assert.That("1.5.6.file.sql", Is.EqualTo(rec.Value));
        }
        
    }
}