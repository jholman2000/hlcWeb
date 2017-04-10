using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class SpecialtiesController : BaseController
    {
        public SpecialtiesController()
        {
                
        }
        public List<Specialty> Search(string search)
        {
            string where;

            if (search == "XYZ")
            {
                where = "SpecialtyName LIKE 'X%' OR SpecialtyName LIKE 'Y%' OR SpecialtyName LIKE 'Z%'";
            }
            else if (search.Length == 1)
            {
                where = $"SpecialtyName LIKE '{search}%'";
            }
            else
            {
                where = $"SpecialtyName LIKE '%{search}%'";

            }
            var sql = "SELECT ID, SpecialtyName, " +
                      "(SELECT COUNT(ID) FROM hlc_DoctorSpecialty ds WHERE ds.SpecialtyID = s.ID) as NumberOfDoctors " +
                      "FROM hlc_Specialty s " +
                      $"WHERE {where} ORDER BY SpecialtyName";

            var results = GetListFromSql<Specialty>(sql);

            return results;

        }

    }
}
