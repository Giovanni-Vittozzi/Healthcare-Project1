using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient; //connect to microsoft SQL server (provides acess to connection object, command object and reader)
using System.Data.SqlTypes; //in case we need data strings, but we most likely won't in a web app
using System.Configuration; //gives access to web config files (pull out connection string)

namespace HealthcareCompanion.DataAccessLayer
{
    public class BaseTier
    {
        public SqlConnection conn { get; set; }
        public SqlCommand cmd { get; set; }
        public string query { get; set; }
        public string connectionString { get; set; }
        public SqlDataReader reader { get; set; }
        public bool success { get; set; }
        public BaseTier()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        }
    }
}