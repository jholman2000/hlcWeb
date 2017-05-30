using hlcWeb.ViewModels.Reports;
using System;
using System.Collections.Generic;

namespace hlcWeb.Controllers.Api
{
    public class ReportsController : BaseController
    {
        public List<RptDoctorsAddedRemovedViewModel> Report(DateTime dateFrom, DateTime dateTo)
        {
            var sql = "select d.DateLastUpdated, u.FirstName + ' ' + u.LastName as UserName, d.FirstName + ' ' + d.LastName as DoctorName, " +
            "d.Attitude, d.Status, p.PracticeName, dn.DateEntered, dn.Notes " +
            "from hlc_Doctor d " +
            "left join hlc_User u ON u.userid = d.lastupdatedby " +
            "left join hlc_Practice p on p.Id = d.PracticeId " +
            "left join hlc_DoctorNote dn ON dn.DoctorID = d.ID " +
            $"where d.DateLastUpdated >= '{dateFrom}' and d.DateLastUpdated <= '{dateTo}' " +
            "order by d.DateLastUpdated, d.LastName, d.FirstName, DateEntered";

            return GetListFromSql<RptDoctorsAddedRemovedViewModel>(sql);

        }
    }
}
