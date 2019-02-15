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

            var sql = "select Id, DepartmentName, DateEntered, EnteredBy, " +
                      "(SELECT COUNT(Id) FROM hlc_Presentation p WHERE p.DepartmentID = d.ID) as NumberInUse " +
                      "from hlc_Department d" +
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/departments/add")]
        public bool Add(HlcDto dto)
        {
            if (string.IsNullOrEmpty(dto.FieldText))
                return false;

            try
            {
                var sql = "insert into hlc_Department (DepartmentName, DateEntered, EnteredBy) values " +
                          $"('{dto.FieldText.Replace("'","''")}', getDate(), '{dto.UserId}');";

                ExecuteSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, new { deleteOp = $"Error adding Specialty: {dto.Id}" });
                return false;
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/departments/remove")]
        public bool Remove(HlcDto dto)
        {
            try
            {
                //var sql = $"delete from hlc_Presentation WHERE DepartmentID = {dto.Id};" +
                //          $"delete from hlc_Department where Id={dto.Id};";
                var sql = $"delete from hlc_Department where Id={dto.Id};";
                ExecuteSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, new { deleteOp = $"Error deleting Department: {dto.Id}" });
                return false;
            }
        }

        /// <summary>
        /// Edit a Department name description
        /// </summary>
        /// <param name="text">Object contains Id, FieldText to save </param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/departments/update")]
        public string Update(HlcDto text)
        {
            var sql = $"update hlc_Department set DepartmentName = '{text.FieldText?.Replace("'", "''")}' where Id={text.Id}";
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
