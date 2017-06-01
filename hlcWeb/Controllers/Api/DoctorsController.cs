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
        [System.Web.Http.Route("api/doctors/search")]
        public List<Doctor> Search(string search, bool includeDeleted = true)
        {
            var where = search == "*"
                ? "1=1"
                : $"(LastName LIKE '%{search}%' OR FirstName LIKE '%{search}%') ";

            if (!includeDeleted)
                where += " and Status <> 99";

            var sql = $"SELECT ID, FirstName, LastName, Attitude FROM hlc_Doctor WHERE {where} ORDER BY LastName";

            var results = GetListFromSql<Doctor>(sql);

            return results;
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
                          $"select * from hlc_Practice where ID = (select PracticeId from hlc_Doctor where Id={id});";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                var doctor = multi.Read<Doctor>().FirstOrDefault();
                if (doctor != null)
                {
                    doctor.Specialties = multi.Read<DoctorSpecialty>().ToList();
                    doctor.Hospitals = multi.Read<DoctorHospital>().ToList();
                    doctor.DoctorNotes = multi.Read<DoctorNote>().ToList();
                    doctor.Practice = multi.Read<Practice>().FirstOrDefault();
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

        internal bool SaveAttitudes(DoctorAttitudesViewModel model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var newId = Connection.Insert(model);
                    return newId > 0;
                }
                else
                {
                    return Connection.Update(model);
                }
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
    }
}
