using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectClassLibrary.Repository.Utils
{
    public class ConnectionInfo:IConnectionInfo
    {
        public string servername { get; set; }
        public string databasename { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string GetConnectionString()
        {


            return "Data Source=" + servername + ";Initial Catalog=" + databasename + ";User id=" + username + ";Password=" + password + ";";
        }
    }
}
