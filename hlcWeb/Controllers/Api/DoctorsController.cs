using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using Dapper;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class DoctorsController : BaseController
    {
        //private SqlConnection _conn;
        public DoctorsController()
        {
           // _conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\_Sandbox\hlcWeb\hlcWeb\App_Data\hlcWeb_local.mdf;Integrated Security=True");
        }
        
        // GET: api/Doctors
        public List<Doctor> Get()
        {
            string sql = "select d.ID, d.FirstName, d.LastName, d.Attitude, d.EmailAddress, d.MobilePhone, d.Pager from hlc_Doctor d ";
            sql += "where Attitude <> 0";

            var x = base.GetSQL<Doctor>(sql);

            return x;
            //using (_conn)
            //{
            //    string sql = "select d.ID, d.FirstName, d.LastName, d.Attitude, d.EmailAddress, d.MobilePhone, d.Pager from hlc_Doctor d ";
            //    sql += "where Attitude <> 0";

            //    _conn.Open();

            //    return  _conn.Query<Doctor>(sql).ToList();

            //}

        }

        // GET: api/Doctors/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Doctors
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Doctors/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Doctors/5
        public void Delete(int id)
        {
        }
    }
}
