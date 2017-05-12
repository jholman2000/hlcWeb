using System.Collections.Generic;
using System.Linq;
using Dapper;
using hlcWeb.Models;
using hlcWeb.ViewModels;

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
            var sql = "SELECT Id, PracticeName, Address1, City, State, OfficePhone1 " +
                      "FROM hlc_Practice " +
                      $"WHERE {where} ORDER BY PracticeName";

            var results = GetListFromSql<Practice>(sql);

            return results;

        }

        /// <summary>
        /// Get Practice details and a list of all Doctors at the Practice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal PracticeViewModel Get(int id)
        {
            var model = new PracticeViewModel();

            using (var conn = Connection)
            {
                var sql = $"select * from hlc_Practice where ID={id};" +
                          "select d.id, d.FirstName, d.LastName, d.MobilePhone, p.PracticeName, p.OfficePhone1 " +
                          "from hlc_Doctor d " +
                          "left join hlc_Practice p on p.ID = d.PracticeID " +
                          $"where d.PracticeId = {id} " +
                          "order by d.LastName, d.FirstName;";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                model.Practice = multi.Read<Practice>().FirstOrDefault();
                model.Doctors = multi.Read<Doctor>().ToList();
            }
            return model;
        }
    }
}
