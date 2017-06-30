using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
using System.Web.Mvc;
using System.Runtime.Caching;

namespace hlcWeb.Controllers.Api
{
    public class DiagnosisController : BaseController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/diagnosis/search")]
        public List<Diagnosis> Search(string search)
        {
            var where = $"DiagnosisName LIKE '%{search}%' ";

            var sql = "select Id, DiagnosisName, DateEntered, EnteredBy " +
                      "from hlc_Diagnosis " +
                      $" WHERE {where} ORDER BY DiagnosisName";

            var results = GetListFromSql<Diagnosis>(sql);

            return results;
        }

        /// <summary>
        /// Get Diagnosis record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Diagnosis Get(int id)
        {

            var sql = "select dg.*, " +
                      "from hlc_Diagnosis dg " +
                      $" WHERE dg.Id = {id}";

            var results = GetListFromSql<Diagnosis>(sql).FirstOrDefault();
            return results;


        }


        internal bool Save(Diagnosis model)
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

            var list = (SelectList)cache["DiagnosisSelectList"];

            if (refresh || list == null)
            {
                var items = Search("")
                        .Select(s => new
                        {
                            Text = s.DiagnosisName,
                            Value = s.Id
                        })
                        .ToList();
                list = new SelectList(items, "Value", "Text");

                if (refresh) cache.Remove("DiagnosisSelectList");
                cache.Add("DiagnosisSelectList", list, new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable });
            }

            return list;
        }

    }
}
