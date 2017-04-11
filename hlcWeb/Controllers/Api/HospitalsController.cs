using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class HospitalsController : BaseController
    {
        public List<Hospital> Search(string search)
        {
            string where;

            if (search == "XYZ")
            {
                where = "HospitalName LIKE 'X%' OR HospitalName LIKE 'Y%' OR HospitalName LIKE 'Z%'";
            }
            else if (search.Length == 1)
            {
                where = $"HospitalName LIKE '{search}%'";
            }
            else
            {
                where = $"HospitalName LIKE '%{search}%'";

            }
            var sql = "SELECT ID, HospitalName, City, State, " +
                       "(SELECT COUNT(ID) FROM hlc_DoctorHospital dh WHERE dh.HospitalID = h.ID) as NumberOfDoctors " +
                       "FROM hlc_Hospital h " +
                       $"WHERE {where} ORDER BY HospitalName";

            var results = GetListFromSql<Hospital>(sql);

            return results;

        }
    }
}
