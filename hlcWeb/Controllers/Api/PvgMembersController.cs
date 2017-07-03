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
    public class PvgMembersController : BaseController
    {
        public List<PvgMember> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"LastName LIKE '%{search}%' OR " +
                  $"FirstName LIKE '%{search}%' ";

            var sql = "SELECT * FROM hlc_PvgMember " +
                      $"WHERE {where} ORDER BY LastName";

            var results = GetListFromSql<PvgMember>(sql);

            return results;

        }

        /// <summary>
        /// Get PvgMember details and a list of all Doctors at the PvgMember
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //internal PvgMember Get(int id)
        //{
        //    var sql = $"select * from hlc_PVGMember where Id='{id}'";

        //    var results = GetListFromSql<PvgMember>(sql).FirstOrDefault();
        //    return results;
        //}

        internal PvgMemberViewModel Get(int id)
        {
            var model = new PvgMemberViewModel();

            using (var conn = Connection)
            {
                var sql = $"select * from hlc_PVGMember where ID={id};" +
                          "select pvgh.ID, HospitalId, HospitalName, pvgh.Notes, " +
                          "	   case DayOfWeek " +
                          "		when 1 then 'Sunday' " +
                          "		when 2 then 'Monday' " +
                          "		when 3 then 'Tuesday' " +
                          "		when 4 then 'Wednesday' " +
                          "		when 5 then 'Thursday' " +
                          "		when 6 then 'Friday' " +
                          "		when 7 then 'Saturday' " +
                          "		when 8 then 'As Needed' " +
                          "		when 9 then 'Alternate' " +
                          "		when 10 then 'Weekends' " +
                          "		else 'Unknown' " +
                          "  	   end as WeekDay " +
                          "from hlc_PVGMemberHospital  pvgh " +
                          "  INNER JOIN hlc_Hospital h             ON pvgh.HospitalID = h.ID " +
                          $"where pvgh.PVGMemberID={id} " +
                          "order by 2;";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                model.PvgMember = multi.Read<PvgMember>().FirstOrDefault();
                model.Hospitals = multi.Read<PvgMemberHospital>().ToList();
            }
            return model;
        }

        internal bool Save(PvgMemberViewModel model)
        {
            try
            {
                if (model.PvgMember.Id == 0)
                {
                    Connection.Insert(model.PvgMember);
                }
                else
                {
                    Connection.Update(model.PvgMember);
                }
                GetSelectList(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, model.PvgMember);
                return false;
            }
        }

        internal bool SaveAssignments(PvgMemberViewModel viewModel)
        {
            // First, delete all assignments currently associated with this Member and 
            // then add back in the ones that are not marked as Remove
            try
            {
                var sql = $"delete from hlc_PVGMemberHospital where PVGMemberId={viewModel.PvgMember.Id}";
                ExecuteSql(sql);

                foreach (var toInsert in viewModel.Hospitals)
                {
                    if (!toInsert.Remove && toInsert.HospitalId != 0)
                    {
                        toInsert.PvgMemberId = viewModel.PvgMember.Id;
                        toInsert.Id = 0;
                        var newId = Connection.Insert(toInsert);
                        toInsert.Id = (int)newId;
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

            var list = (SelectList)cache["PvgMemberSelectList"];

            if (refresh || list == null)
            {
                var items = Search("")
                    .Select(s => new
                    {
                        Text = s.LastName + ", " + s.FirstName,
                        Value = s.Id
                    })
                    .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("PvgMemberSelectList");
                cache.Add("PvgMemberSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

    }

}

