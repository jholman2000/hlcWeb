﻿using hlcWeb.ViewModels.Reports;
using System;
using System.Collections.Generic;
using hlcWeb.Models;

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

        public List<RptDoctorsSpecialtyViewModel> DoctorsSpecialty(Attitude attitude, string specialtyList)
        {
            var where = "" +
                (!string.IsNullOrEmpty(specialtyList) ? $" and ds.SpecialtyID in ({specialtyList}) " : " ");

            var sql =
                "select d.ID, s.SpecialtyName, " +
                "case when Status = 0 then 'Unknown' " +
                "     when Status = 1 then 'Newly Identified' " +
                "     when Status = 2 then 'Letter Sent' " +
                "     when Status = 7 then 'Deceased' " +
                "     when Status = 8 then 'Moved' " +
                "     when Status = 9 then 'Active' " +
                "     when Status = 10 then 'Retired' " +
                "     when Status = 99 then 'Deleted' " +
                "end as Status, " +
                "d.FirstName + ' ' + d.LastName    as DoctorName,  " +
                "p.PracticeName, p.City + ' ' + p.State as PracticeCityState, p.OfficePhone1, " +
                "Hospitals = (select h.HospitalName + '|' as [text()] " +
                "             from hlc_DoctorHospital dh " +
                "             left join hlc_Hospital h on h.Id = dh.HospitalID " +
                "             where dh.DoctorID = d.Id " +
                "             for xml path ('')) " +
                "from hlc_Doctor d " +
                "left join hlc_DoctorSpecialty ds on ds.DoctorID = d.Id " +
                "left join hlc_Specialty s        on s.ID = ds.SpecialtyID " +
                "left join hlc_Practice p         on p.ID = d.PracticeID " +
                "where ds.SpecialtyID is not null " +
                where +
                "order by s.SpecialtyName, d.LastName, d.FirstName";

            return GetListFromSql<RptDoctorsSpecialtyViewModel>(sql);
        }

        public List<RptCaseFilesViewModel> CaseFiles(DateTime dateFrom, DateTime dateTo, int doctorId, int hospitalId, int diagnosisId, string enteredBy, bool isPediatricCase)
        {
            var where = "" +
                (doctorId != 0   ? $" and cf.DoctorId = {doctorId} " : "") + 
                (hospitalId != 0 ? $" and cf.HospitalId = {hospitalId} " : "") +
                (diagnosisId != 0 ? $" and cf.DiagnosisId = {diagnosisId} " : "") +
                (!string.IsNullOrEmpty(enteredBy) ? $" and cf.EnteredBy = '{enteredBy}' " : "") +
                (isPediatricCase ? $" and cf.IsPediatricCase = 1 " : "");

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
                where +
                "order by cf.CaseDate";

            return GetListFromSql<RptCaseFilesViewModel>(sql);
        }

    }
}
