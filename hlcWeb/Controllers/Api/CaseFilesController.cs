using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class CaseFilesController : BaseController
    {
        [HttpGet]
        [Route("api/casefiles/search")]
        public List<CaseFile> Search(string search)
        {
            var where = $"cf.LastName LIKE '%{search}%' OR " +
                        $"cf.FirstName LIKE '%{search}%' OR " +
                        $"d.LastName LIKE '%{search}%' OR " +
                        $"h.HospitalName LIKE '%{search}%' OR " +
                        $"cf.CongregationName LIKE '%{search}%'";

            var sql = "select cf.ID, cf.CaseDate, cf.FirstName, cf.LastName, cf.CongregationName, cf.DoctorId, cf.HospitalId, " +
                      "d.FirstName + ' ' + d.LastName as DoctorName, h.HospitalName " +
                      "from hlc_CaseFile cf " +
                      "left join hlc_Doctor d on d.id = cf.DoctorId " +
                      "left join hlc_Hospital h on h.ID = cf.HospitalId " +
                      $" WHERE {where} ORDER BY LastName, FirstName";

            var results = GetListFromSql<CaseFile>(sql);

            return results;
        }

        /// <summary>
        /// Get all Case File information (including Hospitals, Specialties and Notes)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CaseFile Get(int id)
        {
            
            var sql = "select cf.*, " +
                      "d.FirstName + ' ' + d.LastName as DoctorName, h.HospitalName, dg.DiagnosisName " +
                      "from hlc_CaseFile cf " +
                      "left join hlc_Doctor d on d.id = cf.DoctorId " +
                      "left join hlc_Hospital h on h.ID = cf.HospitalId " +
                      "left join hlc_Diagnosis dg on dg.Id = cf.DiagnosisId " +
                      $" WHERE cf.Id = {id}";

            var results = GetListFromSql<CaseFile>(sql).FirstOrDefault();
            return results;


        }

        [HttpPost]
        [Route("api/casefiles/savetext")]
        public string SaveText([FromBody]string fieldName) //, [FromBody]string textValue)
        {
            return "OK";
        }

        internal bool Save(CaseFile model)
        {
            if (model.Id == 0)
            {
                var x = Connection().Insert(model);
                return x > 0;
            }
            else
            {
                return Connection().Update(model);
            }

        }

    }
}
