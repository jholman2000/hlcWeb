using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using System.Data.SqlClient;

namespace hlcWeb.Controllers.Api
{
    public class BaseController : ApiController
    {
        private SqlConnection _conn;
        public BaseController()
        {
            _conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Sandbox\hlcWeb\hlcWeb\App_Data\hlcWeb_local.mdf;Integrated Security=True");
        }

        public List<T> GetSQL<T>(string sql) where T: class
        {
            using (_conn)
            {

                _conn.Open();

                return _conn.Query<T>(sql).ToList();

            }

        }
    }
}
