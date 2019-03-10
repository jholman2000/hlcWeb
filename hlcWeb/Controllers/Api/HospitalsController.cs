using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
using hlcWeb.ViewModels;
using System.Web.Mvc;
using System.Runtime.Caching;

namespace hlcWeb.Controllers.Api
{
    public class HospitalsController : BaseController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/hospitals/search/{search}")]
        public List<Hospital> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"HospitalName LIKE '%{search}%' OR " +
                  $"City LIKE '{search}%'";

            var sql = "SELECT Id, HospitalName, City, State, " +
                       "(SELECT COUNT(ID) FROM hlc_DoctorHospital dh WHERE dh.HospitalID = h.ID) as NumberOfDoctors " +
                       "FROM hlc_Hospital h " +
                       $"WHERE {where} ORDER BY HospitalName";

            var results = GetListFromSql<Hospital>(sql);

            return results;

        }

        [HttpGet]
        [Route("api/hospitals/search/all")]
        public List<Hospital> Search()
        {
            return Search("*");
        }

        /// <summary>
        /// Get Hospital details and a list of all Doctors at the Hospital
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal HospitalViewModel Get(int id)
        {
            var model = new HospitalViewModel();

            using (var conn = Connection)
            {
                var sql = $"select * from hlc_Hospital where ID={id};" +
                          "select d.id, d.FirstName, d.LastName, d.MobilePhone, d.Attitude, p.PracticeName, p.OfficePhone1 " +
                          "from hlc_DoctorHospital dh " +
                          "left join hlc_Doctor d on d.ID = dh.DoctorID " +
                          "left join hlc_Practice p on p.ID = d.PracticeID " +
                          $"where dh.HospitalID = {id} " +
                          "order by d.LastName, d.FirstName;" +
                          "select m.id,  m.FirstName, m.LastName, m.Address, m.City, m.State, m.Zip, m.MobilePhone, m.HomePhone, " +
                          "m.EmailAddress, m.Congregation, w.Description as DayOfWeek, mh.Notes as PVGComments " +
                          "from hlc_PVGMemberHospital mh " +
                          "left join hlc_PVGMember m on m.ID = mh.PVGMemberID " +
                          "left join hlc_DayOfWeek w on w.Id = mh.DayOfWeek " +
                          $"where mh.HospitalId = {id} " +
                          "order by m.LastName, m.FirstName, mh.DayOfWeek";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                model.Hospital = multi.Read<Hospital>().FirstOrDefault();
                model.Doctors = multi.Read<Doctor>().ToList();
                model.PVGMembers = multi.Read<PvgMember>().ToList();
            }
            return model;
        }

        internal bool Save(HospitalViewModel model)
        {
            try
            {
                if (model.Hospital.Id == 0)
                {
                    Connection.Insert(model.Hospital);
                }
                else
                {
                    Connection.Update(model.Hospital);
                }
                GetSelectList(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, model.Hospital);
                return false;
            }
        }

        internal SelectList GetSelectList(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            var list = (SelectList)cache["HospitalSelectList"];

            if (refresh || list == null)
            {
                var items = Search("")
                        .Select(s => new
                        {
                            Text = s.HospitalName + (s.City != null ? " - " : "") + s.City + " " + s.State,
                            Value = s.Id
                        })
                        .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("HospitalSelectList");
                cache.Add("HospitalSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

        //internal SelectList GetSelectListHospitalType(bool refresh = false)
        //{
        //    ObjectCache cache = MemoryCache.Default;

        //    var list = (SelectList)cache["HospitalTypeSelectList"];

        //    if (refresh || list == null)
        //    {
        //        var items = Get
        //            .ToList();
        //        list = new SelectList(items, "Value", "Text");

        //        if (refresh) cache.Remove("HospitalTypeSelectList");
        //        cache.Add("HospitalTypeSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
        //    }

        //    return list;
        //}

    }
}
