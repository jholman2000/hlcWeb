using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using Dapper;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers.Api
{
    public class DoctorsController : BaseController
    {

        /// <summary>
        /// Search Doctors by FirstName or LastName (Ajax call from Search page)
        /// </summary>
        /// <param name="search"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/doctors/search/{search}")]
        public List<Doctor> Search(string search, bool includeDeleted = true)
        {
            var where = search == "*"
                ? "1=1"
                : $"(LastName LIKE '{search}%' OR FirstName LIKE '{search}%') ";

            // If excluding Deleted, then exclude Deceased, Retired and Moved Out of Area
            if (!includeDeleted)
                where += " and Status not in (7,8,10,99)";

            var sql = $"SELECT ID, FirstName, LastName, Attitude, Status FROM hlc_Doctor WHERE {where} ORDER BY LastName";

            var results = GetListFromSql<Doctor>(sql);

            return results;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/doctors/search/all")]
        public List<Doctor> Search()
        {
            return Search("*");
        }

        /// <summary>
        /// Get all Doctor information (including Hospitals, Specialties and Notes)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Doctor Get(int id)
        {
            using (var conn = Connection)
            {
                var sql = $"select * from hlc_Doctor where ID={id};" +
                          $"select ds.*, s.SpecialtyName from hlc_DoctorSpecialty ds left join hlc_Specialty s on s.ID = ds.SpecialtyID where ds.DoctorID = {id} order by SpecialtyName;" +
                          $"select dh.*, h.HospitalName from hlc_DoctorHospital dh left join hlc_Hospital h on h.ID = dh.HospitalID where dh.DoctorID = {id} order by HospitalName;" +
                          $"select dn.*, u.FirstName + ' ' + u.LastName as UserName from hlc_DoctorNote dn left join hlc_User u on u.UserId = dn.UserId where dn.DoctorID = {id} order by DateEntered desc;" +
                          $"select * from hlc_Practice where ID = (select PracticeId from hlc_Doctor where Id={id});" +
                          "select cf.ID, cf.CaseDate, cf.FirstName, cf.LastName, h.HospitalName, d.DiagnosisName " +
                          "from hlc_CaseFile cf " +
                          "left join hlc_Hospital h on h.ID = cf.HospitalId " +
                          "left join hlc_Diagnosis d on d.ID = cf.DiagnosisId " +
                          $"where (cf.DoctorId = {id} or cf.AssistingID = {id} or cf.AnesthID = {id}) " +
                          "order by cf.CaseDate desc;";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                var doctor = multi.Read<Doctor>().FirstOrDefault();
                if (doctor != null)
                {
                    doctor.Specialties = multi.Read<DoctorSpecialty>().ToList();
                    doctor.Hospitals = multi.Read<DoctorHospital>().ToList();
                    doctor.DoctorNotes = multi.Read<DoctorNote>().ToList();
                    doctor.Practice = multi.Read<Practice>().FirstOrDefault();
                    doctor.CaseFiles = multi.Read<CaseFile>().ToList();
                }

                return doctor;

            }

        }

        internal bool SaveContact(DoctorContactViewModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    Connection.Insert(model);
                }
                else
                {
                    Connection.Update(model);
                }
                // Force refresh of Doctors in cache
                GetSelectList(true);
                GetSelectListAnesthesiologists(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, model);
                return false;
            }
        }

        internal bool SaveAttitudes(DoctorAttitudesViewModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var newId = Connection.Insert(model);
                    return newId > 0;
                }
                return Connection.Update(model);
            }
            catch (Exception ex)
            {
                LogException(ex, model);
                return false;
            }
        }

        internal bool SaveDoctorSpecialties(DoctorSpecialtiesViewModel viewModel)
        {
            // First, delete all specialties assigned to this doctor and then
            // add back in the ones that are not marked as Remove
            try
            {
                var sql = $"delete from hlc_DoctorSpecialty where DoctorId={viewModel.DoctorId}";
                ExecuteSql(sql);

                foreach (var spec in viewModel.Specialties)
                {
                    if (!spec.Remove && spec.SpecialtyId != 0)
                    {
                        spec.DoctorId = viewModel.DoctorId;
                        spec.Id = 0;
                        var newId = Connection.Insert(spec);
                        spec.Id = (int)newId;
                    }
                }
                GetSelectListAnesthesiologists(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, viewModel);
                return false;
            }

        }

        internal bool SaveDoctorHospitals(DoctorHospitalsViewModel viewModel)
        {
            // First, delete all hospitals assigned to this doctor and then
            // add back in the ones that are not marked as Remove
            try
            {
                var sql = $"delete from hlc_DoctorHospital where DoctorId={viewModel.DoctorId}";
                ExecuteSql(sql);

                foreach (var spec in viewModel.Hospitals)
                {
                    if (!spec.Remove && spec.HospitalId != 0)
                    {
                        spec.DoctorId = viewModel.DoctorId;
                        spec.Id = 0;
                        var newId = Connection.Insert(spec);
                        spec.Id = (int)newId;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, viewModel);
                return false;
            }
        }

        internal SelectList GetSelectList(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            var list = (SelectList)cache["DoctorSelectList"];

            if ( refresh || list == null)
            {
                var items = Search("", false)
                        .Select(s => new
                        {
                            Text = s.LastName + ", " + s.FirstName,
                            Value = s.Id
                        })
                        .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("DoctorSelectList");
                cache.Add("DoctorSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

        internal SelectList GetSelectListAnesthesiologists(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            var list = (SelectList)cache["AnesthSelectList"];

            if (refresh || list == null)
            {

                var sql = "SELECT d.ID, d.FirstName, d.LastName, s.SpecialtyName " +
                          "FROM hlc_Doctor d " +
                          "LEFT JOIN hlc_DoctorSpecialty ds ON ds.DoctorID = d.ID " +
                          "LEFT JOIN hlc_Specialty s ON s.ID = ds.SpecialtyID " +
                          "WHERE Status not in (7,8,10,99)  " +
                          "  and s.SpecialtyName = 'Anesthesiology' " +
                          "ORDER BY LastName;";

                var results = GetListFromSql<Doctor>(sql);

                var items = results
                    .Select(s => new
                    {
                        Text = s.LastName + ", " + s.FirstName,
                        Value = s.Id
                    })
                    .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("AnesthSelectList");
                cache.Add("AnesthSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

        #region Free-form text edit functions
        [HttpPost]
        [Route("api/casefiles/savetext")]
        public string SaveText(HlcDto text)
        {
            if (text.Id == 0)
            {                
                var sql = $"insert into hlc_DoctorNote (UserId,DateEntered,DoctorId,Notes) values ('{text.UserId}',getDate(),{text.DoctorId}, '{text.FieldText?.Replace("'", "''")}')";
                try
                {
                    ExecuteSql(sql);
                    return "OK";
                }
                catch (Exception ex)
                {
                    LogException(ex, text);
                    return "ERROR";
                }
            }
            else { 
                var sql = $"update hlc_DoctorNote set Notes = '{text.FieldText?.Replace("'", "''")}' where Id={text.Id}";
                try
                {
                    ExecuteSql(sql);
                    return "OK";
                }
                catch (Exception ex)
                {
                    LogException(ex, text);
                    return "ERROR";
                }
            }
        }
        #endregion

    }
}
