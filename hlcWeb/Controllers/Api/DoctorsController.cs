using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dapper;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class DoctorsController : BaseController
    {

        public IHttpActionResult Get()
        {

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
            string where;

            if (search == "XYZ")
            {
                where = "LastName LIKE 'X%' OR LastName LIKE 'Y%' OR LastName LIKE 'Z%'";
            }
            else if (search.Length == 1)
            {
                where = $"LastName LIKE '{search}%'";
            }
            else
            {
                where = $"LastName LIKE '%{search}%' OR FirstName LIKE '%{search}%'";

            }
            var sql = $"SELECT ID, FirstName, LastName, Attitude FROM hlc_Doctor WHERE {where} ORDER BY LastName";

            var results = GetListFromSql<Doctor>(sql);

            return results;
        }

        public IHttpActionResult Get(int id)
        {
            var results = GetMemberFromId<Doctor>(id);

            if (results == null)
                return NotFound();

            return Ok(results);
        }

        /// <summary>
        /// Get all Doctor information (including Hospitals, Specialties and Notes)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Doctor GetDoctor(int id)
        {
            using (var conn = Connection())
            {
                var sql = $"select * from hlc_Doctor where ID={id};" +
                          $"select ds.*, s.SpecialtyName from hlc_DoctorSpecialty ds left join hlc_Specialty s on s.ID = ds.SpecialtyID where ds.DoctorID = {id};" +
                          $"select dh.*, h.HospitalName from hlc_DoctorHospital dh left join hlc_Hospital h on h.ID = dh.HospitalID where dh.DoctorID = {id};" +
                          $"select dn.*, u.FirstName + ' ' + u.LastName as UserName from hlc_DoctorNote dn left join hlc_User u on u.UserID = dn.UserID where dn.DoctorID = {id};";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                var doctor = multi.Read<Doctor>().FirstOrDefault();
                if (doctor != null)
                {
                    doctor.Specialties = multi.Read<DoctorSpecialty>().ToList();
                    doctor.Hospitals = multi.Read<DoctorHospital>().ToList();
                    doctor.Notes = multi.Read<DoctorNote>().ToList();
                }

                return doctor;

            }

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
