using hlcWeb.ViewModels.Reports;
using System;
using System.Collections.Generic;

namespace hlcWeb.Controllers.Api
{
    public class ReportsController : BaseController
    {
        public RptAnnualReport AnnualReport()
        {
            var annualReport = new RptAnnualReport();

            string sql;

            #region HLC/PVG/OftenReceives count
            sql =
            "select 'HLC Member' as Name, count(*) as Count from hlc_user " +
                "where (UserRole = 'HLC Member' or UserID = 'mjones') " +
                "  and IsActive = 1 " +
                "union " +
                "select 'PVG Member' as Name, count(*) as Count from hlc_PVGMember";

            var results = GetListFromSql<RptNameCount>(sql);
            annualReport.HlcCount = results[0].Count;
            annualReport.PvgCount = results[1].Count;
            #endregion

            #region Hospital Types Count
            sql =
                "with GroupData as " +
                "(select HospitalType, count(*) as GroupCount from hlc_Hospital group by HospitalType) " +
                "select Description as Name, coalesce(GroupCount,0) as Count " +
                "from hlc_HospitalType ht " +
                "left join GroupData gd on gd.HospitalType = ht.Id " +
                "union " +
                "select 'Hospitals with pediatrics' as Name, Count(*) as Count " +
                "from hlc_Hospital h " +
                "where h.HasPediatrics = 1 " +
                "order by Description";

            annualReport.HospitalTypes = GetListFromSql<RptNameCount>(sql);
            #endregion

            #region Coop
            sql =
                "with GroupData as " +
                "( " +
                "select s.ID, s.SpecialtyName, count(s.ID) as GroupCount " +
                "from hlc_Doctor d " +
                "left join hlc_DoctorSpecialty ds on ds.DoctorID = d.ID " +
                "left join hlc_Specialty s on s.ID = ds.SpecialtyID and s.SpecialtyCode='ANL'" +
                "where d.Attitude = 1 " +
                "group by s.Id, s.SpecialtyName " +
                ") " +
                "select s.SpecialtyName as Name, coalesce(GroupCount,0) as Count " +
                "from hlc_Specialty s " +
                "left join GroupData gd on gd.ID = s.ID  " +
                "where s.SpecialtyCode='ANL' " +
                "order by s.SpecialtyName";

            annualReport.CoopDoctors = GetListFromSql<RptNameCount>(sql);
            #endregion

            #region BMSP Hospitals
            sql =
                "select HospitalName, Address1, Address2, City, State, Zip, " +
                "       BMSPCoordName, BmspCoordPhone, BmspCoordIsWitness, BmspCommitment, " +
                "	   BmspSpecialties, BmspPhone, BmspNumberOfDoctors " +
                "from hlc_Hospital " +
                "where HasBSMP = 1 " +
                "order by HospitalName";

            annualReport.BMSP = GetListFromSql<RptAnnualBMSP>(sql);
            #endregion

            #region OftenReceives Hospitals

            sql =
                "select HospitalName, City, State, HasBSMP " +
                "from hlc_Hospital where OftenReceivesWitnesses = 1 " +
                "order by HospitalName";

            annualReport.OftenReceives = GetListFromSql<RptAnnualOftenReceives>(sql);
            #endregion

            return annualReport;
        }

        public List<RptDoctorsAddedRemovedViewModel> DoctorsAddedRemoved(DateTime dateFrom, DateTime dateTo)
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

        public List<RptDoctorsSpecialtyViewModel> DoctorsSpecialty(int attitude, string specialtyList)
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
                $"and d.Attitude = {attitude} " +
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
                "left join hlc_User u      on u.UserId = cf.EnteredBy " +
                $"where (cast(cf.DateEntered as Date) between '{dateFrom.ToShortDateString()}' and '{dateTo.ToShortDateString()}') " +
                where +
                "order by cf.CaseDate";

            return GetListFromSql<RptCaseFilesViewModel>(sql);
        }

        public List<RptHospitalsByType> HospitalsByType(int hospitalType)
        {
            var where = "" +
                        (hospitalType >= 0 ? $" where h.HospitalType = {hospitalType} " : " ");

            var sql =
                "select ht.Description as HospitalTypeDesc, h.ID, h.HospitalType, h.HospitalName, h.Address1, h.Address2, " +
                "       h.City + ' ' + h.State as HospitalCityState, h.HasBSMP, h.HasPediatrics, " +
                "(select count(ID) FROM hlc_DoctorHospital dh WHERE dh.HospitalID = h.ID) as NumberOfDoctors " +
                "from hlc_Hospital h " +
                "left join hlc_HospitalType ht on ht.Id = h.HospitalType " +
                where +
                "order by HospitalTypeDesc, HospitalName";

            return GetListFromSql<RptHospitalsByType>(sql);
        }

        public List<RptPVGMembers> PVGMembers(int hospitalId, int dayOfWeek)
        {
            var where = "" +
                        (hospitalId != 0 ? $" and mh.HospitalID = {hospitalId} " : "") +
                        (dayOfWeek != -1 ? $" and mh.DayOfWeek = {dayOfWeek} " : "");

            var sql =
                "select distinct m.ID, m.FirstName, m.LastName, m.FirstName + ' ' + m.LastName as PVGMemberName, m.Address, m.City + ' ' + m.State + ' ' + m.Zip as PVGCityState, " +
                "       m.MobilePhone, m.HomePhone, m.EmailAddress, m.Congregation, " +
                "	   Hospitals = (select h.HospitalName + '~' + w.Description + '~' + coalesce(mh.Notes,'') + '|' as [text()] " +
                "					from hlc_PVGMemberHospital mh " +
                "					left join hlc_Hospital h on h.Id = mh.HospitalID " +
                "					left join hlc_DayOfWeek w on w.Id = mh.DayOfWeek " +
                "					where mh.PVGMemberID = m.ID " +
                                    where +
                "					for xml path ('')) " +
                "from hlc_PVGMember m " +
                "left join hlc_PVGMemberHospital mh on mh.PVGMemberID = m.ID " +
                "where 1=1 " +
                where +
                "order by m.LastName, m.FirstName";

            return GetListFromSql<RptPVGMembers>(sql);

        }
    }
}
