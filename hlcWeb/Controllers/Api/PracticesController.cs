using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class PracticesController : BaseController
    {
        public List<Practice> Search(string search)
        {
            string where;

            if (search == "XYZ")
            {
                where = "PracticeName LIKE 'X%' OR PracticeName LIKE 'Y%' OR PracticeName LIKE 'Z%'";
            }
            else if (search.Length == 1)
            {
                where = $"PracticeName LIKE '{search}%'";
            }
            else
            {
                where = $"PracticeName LIKE '%{search}%'";

            }
            var sql = "SELECT Id, PracticeName, City, State, OfficePhone1 " +
                      "FROM hlc_Practice " +
                      $"WHERE {where} ORDER BY PracticeName";

            var results = GetListFromSql<Practice>(sql);

            return results;

        }
    }
}
