using System;
using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
using System.Web.Mvc;
using System.Web.Http;
using System.Runtime.Caching;

namespace hlcWeb.Controllers.Api
{
    public class DiagnosisController : BaseController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/diagnosis/search/{search}")]
        public List<Diagnosis> Search(string search)
        {
            var where = $"DiagnosisName LIKE '{search}%' ";

            var sql = "select Id, DiagnosisName, DateEntered, EnteredBy, " +
                      "(SELECT COUNT(Id) FROM hlc_CaseFile cf WHERE cf.DiagnosisID = d.ID) as NumberInUse " +
                      "from hlc_Diagnosis d " +
                      $" WHERE {where} ORDER BY DiagnosisName";

            var results = GetListFromSql<Diagnosis>(sql);

            return results;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/diagnosis/search/all")]
        public List<Diagnosis> Search()
        {
            return Search("");
        }

        /// <summary>
        /// Get Diagnosis record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Diagnosis Get(int id)
        {

            var sql = "select * from hlc_Diagnosis " +
                      $" WHERE Id = {id}";

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

        /// <summary>
        /// Edit a Diagnosis name description
        /// </summary>
        /// <param name="text">Object contains Id, FieldText to save </param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/diagnosis/update")]
        public string Update(HlcDto text)
        {
            var sql = $"update hlc_Diagnosis set DiagnosisName = '{text.FieldText?.Replace("'", "''")}' where Id={text.Id}";
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/diagnosis/remove")]
        public bool Remove(HlcDto dto)
        {
            try
            {
                var sql = $"delete from hlc_CaseFile ds WHERE DiagnosisID = {dto.Id};" +
                          $"delete from hlc_Diagnosis where Id={dto.Id};";
                ExecuteSql(sql);

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
