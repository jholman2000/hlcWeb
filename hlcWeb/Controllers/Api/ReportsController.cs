using hlcWeb.ViewModels.Reports;
using System;
using System.Collections.Generic;

namespace hlcWeb.Controllers.Api
{
    public class ReportsController : BaseController
    {
        public List<RptDoctorsAddedRemovedViewModel> Report(DateTime dateFrom, DateTime dateTo)
        {
            var sql = 
                "select d.DateLastUpdated, u.FirstName + ' ' + u.LastName as UserName, d.FirstName + ' ' + d.LastName as DoctorName, " +
                "d.Attitude, d.Status, p.PracticeName, dn.DateEntered, dn.Notes " +
                "from hlc_Doctor d " +
                "left join hlc_User u ON u.userid = d.lastupdatedby " +
                "left join hlc_Practice p on p.Id = d.PracticeId " +
                "left join hlc_DoctorNote dn ON dn.DoctorID = d.ID " +
                $"where d.DateLastUpdated >= '{dateFrom}' and d.DateLastUpdated <= '{dateTo}' " +
                "order by d.DateLastUpdated, d.LastName, d.FirstName, DateEntered";

            return GetListFromSql<RptDoctorsAddedRemovedViewModel>(sql);

        }

        public List<RptCaseFilesViewModel> CaseFiles(DateTime dateFrom, DateTime dateTo, int doctorId, int hospitalId, int diagnosisId, string viewModelEnteredBy, bool isPediatricCase)
        {
            var where = "" +
                (doctorId != 0   ? $" and cf.DoctorId = {doctorId} " : "") + 
                (hospitalId != 0 ? $" and cf.HospitalId = {hospitalId} " : "") +
                (diagnosisId != 0 ? $" and cf.DiagnosisId = {diagnosisId} " : "");

            var sql =
                "select cf.Id, cf.CaseDate, cf.DateEntered, " +
                "u.FirstName + ' ' + u.LastName as UserName, " +
                "d.FirstName + ' ' + d.LastName as DoctorName, " +
                "cf.LastName + ', ' + cf.FirstName as PatientName, " +
                "h.HospitalName, i.DiagnosisName, " +
                "cf.IsPediatricCase, cf.CourtOrderSought, cf.CourtOrderGranted, cf.TransfusionGiven " +
                "    from hlc_CaseFile cf " +
                "left join hlc_Doctor d    on d.ID = cf.DoctorId " +
                "left join hlc_Hospital h  on h.ID = cf.HospitalId " +
                "left join hlc_Diagnosis i on i.ID = cf.DiagnosisId " +
                "left join hlc_User u      on u.UserID = cf.EnteredBy " +
                $"where (cast(cf.DateEntered as Date) between '{dateFrom.ToShortDateString()}' and '{dateTo.ToShortDateString()}') " +
                where;

            return GetListFromSql<RptCaseFilesViewModel>(sql);
        }
    }
}
