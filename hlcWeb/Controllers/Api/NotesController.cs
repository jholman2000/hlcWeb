using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers.Api
{
    public class NotesController : BaseController
    {
        internal List<DoctorNote> Search(string search)
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

        internal DoctorNote Get(int id)
        {
            var sql = "SELECT dn.ID, dn.DateEntered, dn.Notes, dn.DoctorId, dn.UserId, " +
                        "d.FirstName + ' ' + d.LastName AS DoctorName, " +
                        "u.FirstName + ' ' + u.LastName AS UserName " +
                        "FROM hlc_DoctorNote dn " +
                        "LEFT JOIN hlc_Doctor d ON d.ID = dn.DoctorID " +
                        "LEFT JOIN hlc_User u ON u.UserID = dn.UserID " +
                        $"WHERE dn.ID = {id}";

            var results = GetListFromSql<DoctorNote>(sql).FirstOrDefault();

            return results;
        }

        internal bool Save(DoctorNote note)
        {
            if (note.Id == 0)
            {
                var x = Connection().Insert(note);
                return x > 0;
            }
            else
            {
                return Connection().Update(note);
            }
            //sql = "update hlc_DoctorNote set"
            
            //return ExecuteSql(sql);
        }
    }
}
