using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class NotesController : BaseController
    {
        public List<DoctorNote> Search(string search)
        {
            var where = $"dn.Notes LIKE '%{search}%'";

            var sql = "SELECT dn.ID, dn.DateEntered, " +
                      $"SUBSTRING(dn.Notes, CHARINDEX('{search}', dn.Notes)-10, 100) as Notes," +
                      "d.FirstName + ' ' + d.LastName AS DoctorName, " +
                      "u.FirstName + ' ' + u.LastName AS UserName " +
                      "FROM hlc_DoctorNote dn " +
                      "LEFT JOIN hlc_Doctor d ON d.ID = dn.DoctorID " +
                      "LEFT JOIN hlc_User u ON u.UserID = dn.UserID " +
                      $"WHERE {where} ORDER BY dn.DateEntered DESC";

            var results = GetListFromSql<DoctorNote>(sql);

            return results;

        }

    }
}
