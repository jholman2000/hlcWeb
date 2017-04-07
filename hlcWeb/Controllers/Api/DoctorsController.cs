using System.Collections.Generic;
using System.Web.Http;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class DoctorsController : BaseController
    {
        //private SqlConnection _conn;

        public IHttpActionResult Get()
        {
            //base.ClearParameters();
            //base.AddParameter("@HospitalID", 5);
            //var x = base.GetListFromSP<Doctor>("hlc_BrowseDoctorsForHospital");
            //return Ok(x);

            string sql = "select d.ID, d.FirstName, d.LastName, d.Attitude, d.EmailAddress, d.MobilePhone, d.Pager from hlc_Doctor d ";
            sql += "where Attitude <> 0";

            var results = GetListFromSql<Doctor>(sql);

            if (results == null)
                return NotFound();

            return Ok(results);

        }

        [HttpGet]
        [Route("api/doctors/search")]
        public List<Doctor> Search(string search)
        {
            var results = GetListFromSql<Doctor>($"SELECT ID, FirstName, LastName, Attitude FROM hlc_Doctor WHERE LastName LIKE '{search}%'");

            return results;
        }

        public IHttpActionResult Get(int id)
        {
            var results = GetMemberFromId<Doctor>(id);

            if (results == null)
                return NotFound();

            return Ok(results);
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
