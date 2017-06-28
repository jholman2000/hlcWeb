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
    public class PracticesController : BaseController
    {
        public List<Practice> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"PracticeName LIKE '%{search}%'";

            var sql = "SELECT Id, PracticeName, Address1, City, State, OfficePhone1 " +
                      "FROM hlc_Practice " +
                      $"WHERE {where} ORDER BY PracticeName";

            var results = GetListFromSql<Practice>(sql);

            return results;

        }

        /// <summary>
        /// Get Practice details and a list of all Doctors at the Practice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal PracticeViewModel Get(int id)
        {
            var model = new PracticeViewModel();

            using (var conn = Connection)
            {
                var sql = $"select * from hlc_Practice where ID={id};" +
                          "select d.id, d.FirstName, d.LastName, d.MobilePhone, d.Attitude, p.PracticeName, p.OfficePhone1 " +
                          "from hlc_Doctor d " +
                          "left join hlc_Practice p on p.ID = d.PracticeID " +
                          $"where d.PracticeId = {id} " +
                          "order by d.LastName, d.FirstName;";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                model.Practice = multi.Read<Practice>().FirstOrDefault();
                model.Doctors = multi.Read<Doctor>().ToList();
            }
            return model;
        }

        internal bool Save(PracticeViewModel model)
        {
            try
            {
                if (model.Practice.Id == 0)
                {
                    Connection.Insert(model.Practice);
                }
                else
                {
                    Connection.Update(model.Practice);
                }
                GetSelectList(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, model.Practice);
                return false;
            }
        }

        internal SelectList GetSelectList(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            var list = (SelectList)cache["PracticeSelectList"];

            if (refresh || list == null)
            {
                var items = Search("")
                    .Select(s => new
                    {
                        Text = s.PracticeName + (s.City != null ? " - " : "") + s.City + " " + s.State,
                        Value = s.Id
                    })
                    .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("PracticeSelectList");
                cache.Add("PracticeSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

    }
}
