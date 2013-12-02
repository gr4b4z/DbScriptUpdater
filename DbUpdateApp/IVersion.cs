using System.Collections.Generic;
using Simple.Data.Ado.Schema;

namespace DbUpdateApp
{
    public interface IVersion
    {
        string GetVersion();
        void SaveVersion(string version);
    }

    public class CCrp7Version : IVersion
    {
        private dynamic db;
        public CCrp7Version(string connectionString)
        {
            db = Simple.Data.Database.OpenConnection(connectionString);
        }

        public string GetVersion()
        {
            var item = db.Configuration.FindAllByKey("DbVersion").Select(db.Configuration.Value).ToScalarOrDefault<string>();
            return item;
        }

        public void SaveVersion(string version)
        {
            db.Configuration.UpdateByKey(Key:"DbVersion",Value:version);
            
        }
    }
}