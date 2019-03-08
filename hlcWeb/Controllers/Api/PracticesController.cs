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
        public List<Practice> Search(string search, bool includeOnlyPractices = false)
        {
            var where = search == "*"
                ? "1=1"
                : $"(PracticeName LIKE '%{search}%' OR " +
                  $"City LIKE '{search}%') ";

            var sql = "SELECT Id, PracticeName, Address1, City, State, OfficePhone1 " +
                      "FROM hlc_Practice " +
                      $"WHERE {where} ";

            if (includeOnlyPractices)
            {
                sql += " AND FacilityType=0 ";
            }

            sql += "ORDER BY PracticeName";

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
                GetSelectList(FacilityType.Practice, true);
                GetSelectList(FacilityType.Legal, true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, model.Practice);
                return false;
            }
        }

        /// <summary>
        /// Returns a SelectList for populating dropdown lists containing either Practices only or all Practices/Facilities
        /// </summary>
        /// <param name="facilityType">Practice to retrieve all Practices only; Legal to retrieve all types</param>
        /// <param name="refresh">Pass True to force a refresh of the SelectList</param>
        /// <returns></returns>
        internal SelectList GetSelectList(FacilityType facilityType, bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            SelectList list;

            if (facilityType == FacilityType.Practice)
            {
                list = (SelectList) cache["PracticeSelectList"];
                if (refresh || list == null)
                {
                    // Store just the Practices for use on the Doctor screen
                    var items = Search("", true)
                        .Select(s => new
                        {
                            Text = s.PracticeName + (s.City != null ? " - " : "") + s.City + " " + s.State,
                            Value = s.Id
                        })
                        .ToList();
                    list = new SelectList(items, "Value", "Text");

                    if (refresh) cache.Remove("PracticeSelectList");
                    cache.Add("PracticeSelectList", list,
                        new CacheItemPolicy {Priority = CacheItemPriority.NotRemovable});
                }
            }
            else
            {
                list = (SelectList)cache["FacilitySelectList"];
                if (refresh || list == null)
                {

                    // Store all Practices/Facilities for use on Presentation screen
                    var itemsAll = Search("")
                        .Select(s => new
                        {
                            Text = s.PracticeName + (s.City != null ? " - " : "") + s.City + " " + s.State,
                            Value = s.Id
                        })
                        .ToList();
                    list = new SelectList(itemsAll, "Value", "Text");

                    if (refresh) cache.Remove("FacilitySelectList");
                    cache.Add("FacilitySelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });                    
                }
            }

            return list;
        }
    }
}
