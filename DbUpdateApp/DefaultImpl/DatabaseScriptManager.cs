using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DbUpdateApp
{
    public class SqlDatabaseScriptManager:IDatabaseScriptManager
    {
        readonly SqlConnection _con1;
        readonly Server _server;
        public SqlDatabaseScriptManager(string connectionString)
        {
            _con1 = new SqlConnection(connectionString);
            _server = new Server(new ServerConnection(_con1));
        }

        public void RunScript(string content)
        {
            _server.ConnectionContext.ExecuteNonQuery(content);
        }

        public void Dispose()
        {
            _con1.Close();
        }
    }
}