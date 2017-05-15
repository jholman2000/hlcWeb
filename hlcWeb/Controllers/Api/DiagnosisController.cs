using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class DiagnosisController : BaseController
    {
        [HttpGet]
        [Route("api/diagnosis/search")]
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
                    var x = Connection.Insert(model);
                    return x > 0;
                }
                return Connection.Update(model);
            }
            catch (Exception ex)
            {
                LogException(ex, model);
                return false;
            }

        }

    }
}
