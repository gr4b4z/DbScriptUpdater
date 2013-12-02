namespace DbUpdateApp
{
    public class DefaultDatabaseVersion : IDatabaseVersion
    {
        private readonly dynamic db;
        private const string keyName = "DbVersion";
        public DefaultDatabaseVersion(string connectionString)
        {
            db = Simple.Data.Database.OpenConnection(connectionString);
        }

        public string GetVersion()
        {
            var item = db.Configuration.FindAllByKey(keyName).Select(db.Configuration.Value).ToScalarOrDefault<string>();
            return item;
        }

        public void SaveVersion(string version)
        {
            db.Configuration.UpdateByKey(Key:keyName,Value:version);
            
        }
    }
}