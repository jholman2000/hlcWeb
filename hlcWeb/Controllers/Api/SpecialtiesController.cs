using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using Dapper;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers.Api
{
    public class SpecialtiesController : BaseController
    {
        internal List<Specialty> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"SpecialtyName LIKE '%{search}%'";

            var sql = "SELECT ID, SpecialtyName, SpecialtyCode, " +
                      "(SELECT COUNT(ID) FROM hlc_DoctorSpecialty ds WHERE ds.SpecialtyID = s.ID) as NumberOfDoctors " +
                      "FROM hlc_Specialty s " +
                      $"WHERE {where} ORDER BY SpecialtyName";

            var results = GetListFromSql<Specialty>(sql);

            return results;

        }

        internal SpecialtyViewModel Get(int id)
        {
            var model = new SpecialtyViewModel();
            using (var conn = Connection)
            {
                var sql = $"select * from hlc_Specialty where ID={id};" +
                          "select d.id, d.FirstName, d.LastName, d.MobilePhone, d.Attitude, p.PracticeName, p.OfficePhone1 " +
                          "from hlc_DoctorSpecialty ds " +
                          "left join hlc_Doctor d on d.ID = ds.DoctorID " +
                          "left join hlc_Practice p on p.ID = d.PracticeID " +
                          $"where ds.SpecialtyID = {id} " +
                          "order by d.LastName, d.FirstName;";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                model.Specialty = multi.Read<Specialty>().FirstOrDefault();
                //model.Doctors = multi.Read<DoctorListViewModel>().ToList();
                model.Doctors = multi.Read<Doctor>().ToList();
            }
            return model;
        }

        /// <summary>
        /// Edit a Specialty name description
        /// </summary>
        /// <param name="text">Object contains Id, FieldText to save </param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/specialties/update")]
        public string Update(HlcDto text)
        {
            var sql = $"update hlc_Specialty set SpecialtyName = '{text.FieldText?.Replace("'", "''")}' where Id={text.Id}";
            try
            {
                ExecuteSql(sql);
                GetSelectList(true);
                return "OK";
            }
            catch (Exception ex)
            {
                LogException(ex, text);
                return "ERROR";
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/specialties/add")]
        public bool Add(HlcDto dto)
        {
            if (string.IsNullOrEmpty(dto.FieldText))
                return false;

            try
            {
                var sql = "insert into hlc_Specialty (SpecialtyName, SpecialtyCode) values " +
                          $"('{dto.FieldText.Replace("'", "''")}', '');";

                ExecuteSql(sql);
                GetSelectList(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, new { deleteOp = $"Error adding Specialty: {dto.Id}" });
                return false;
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/specialties/remove")]
        public bool Remove(HlcDto dto)
        {
            try
            {
                //var sql = $"delete from hlc_DoctorSpecialty ds WHERE ds.SpecialtyID = {dto.Id};" +
                //          $"delete from hlc_Specialty where Id={dto.Id};";
                var sql = $"delete from hlc_Specialty where Id={dto.Id};";
                ExecuteSql(sql);
                GetSelectList(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, new { deleteOp = $"Error deleting Specialty: {dto.Id}" });
                return false;
            }
        }

        internal SelectList GetSelectList(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            var list = (SelectList)cache["SpecialtySelectList"];

            if (refresh || list == null)
            {
                var items = Search("")
                    .Select(s => new
                    {
                        Text = s.SpecialtyName,
                        Value = s.Id
                    })
                    .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("SpecialtySelectList");
                cache.Add("SpecialtySelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

    }
}
