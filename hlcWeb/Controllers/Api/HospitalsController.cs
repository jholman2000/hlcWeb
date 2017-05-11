using System.Collections.Generic;
using System.Linq;
using Dapper;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers.Api
{
    public class HospitalsController : BaseController
    {
        internal List<Hospital> Search(string search)
        {
            var where = $"HospitalName LIKE '%{search}%'";

            var sql = "SELECT Id, HospitalName, City, State, " +
                       "(SELECT COUNT(ID) FROM hlc_DoctorHospital dh WHERE dh.HospitalID = h.ID) as NumberOfDoctors " +
                       "FROM hlc_Hospital h " +
                       $"WHERE {where} ORDER BY HospitalName";

            var results = GetListFromSql<Hospital>(sql);

            return results;

        }

        /// <summary>
        /// Get Hospital details and a list of all Doctors at the Hospital
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal HospitalViewModel Get(int id)
        {
            var model = new HospitalViewModel();

            using (var conn = Connection)
            {
                var sql = $"select * from hlc_Hospital where ID={id};" +
                          "select d.id, d.FirstName + ' ' + d.LastName as DoctorName, d.MobilePhone, p.PracticeName, p.OfficePhone1 " +
                          "from hlc_DoctorHospital dh " +
                          "left join hlc_Doctor d on d.ID = dh.DoctorID " +
                          "left join hlc_Practice p on p.ID = d.PracticeID " +
                          $"where dh.HospitalID = {id} " +
                           "order by d.LastName, d.FirstName;";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                model.Hospital = multi.Read<Hospital>().FirstOrDefault();
                model.Doctors = multi.Read<DoctorListViewModel>().ToList();
            }
            return model;
        }
    }
}
