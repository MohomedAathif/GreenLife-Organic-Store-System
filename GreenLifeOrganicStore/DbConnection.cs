using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace GreenLifeOrganicStore
{
    public static class DbConnection
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(
                "Data Source=DESKTOP-RTT32SQ\\SQLEXPRESS;Initial Catalog=GreenLifeDB;Integrated Security=True;Encrypt=False;"
            );
        }
    }
}
