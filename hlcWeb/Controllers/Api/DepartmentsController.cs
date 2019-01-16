using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
using System.Web.Mvc;
using System.Runtime.Caching;

namespace hlcWeb.Controllers.Api
{
    public class DepartmentsController : BaseController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/departments/search/{search}")]
        public List<Department> Search(string search)
        {
            var where = $"DepartmentName LIKE '{search}%' ";

            var sql = "select Id, DepartmentName, DateEntered, EnteredBy " +
                      "from hlc_Department " +
                      $" WHERE {where} ORDER BY DepartmentName";

            var results = GetListFromSql<Department>(sql);

            return results;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/departments/search/all")]
        public List<Department> Search()
        {
            return Search("");
        }

        /// <summary>
        /// Get Department record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Department Get(int id)
        {

            var sql = "select * from hlc_Department " +
                      $" WHERE Id = {id}";

            var results = GetListFromSql<Department>(sql).FirstOrDefault();
            return results;
        }


        internal bool Save(Department model)
        {
            try
            {
                if (model.Id == 0)
                {
                    Connection.Insert(model);
                }
                else
                {
                    return Connection.Update(model);
                }
                GetSelectList(true);
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, model);
                return false;
            }

        }

        internal SelectList GetSelectList(bool refresh = false)
        {
            ObjectCache cache = MemoryCache.Default;

            var list = (SelectList)cache["DepartmentSelectList"];

            if (refresh || list == null)
            {
                var items = Search("")
                        .Select(s => new
                        {
                            Text = s.DepartmentName,
                            Value = s.Id
                        })
                        .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("DepartmentSelectList");
                cache.Add("DepartmentSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }


    }
}
