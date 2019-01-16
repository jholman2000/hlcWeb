using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class PresentationsController : BaseController
    {
        [HttpGet]
        [Route("api/presentation/search/{search}")]
        public List<Presentation> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"cf.LastName LIKE '{search}%' OR " +
                  $"cf.FirstName LIKE '{search}%' OR " +
                  $"d.LastName LIKE '{search}%' OR " +
                  $"h.HospitalName LIKE '{search}%' OR " +
                  $"cf.CongregationName LIKE '{search}%'";

            var sql = "select cf.ID, cf.CaseDate, cf.FirstName, cf.LastName, cf.CongregationName, cf.DoctorId, cf.HospitalId, " +
                      "d.FirstName + ' ' + d.LastName as DoctorName, h.HospitalName " +
                      "from hlc_Presentation cf " +
                      "left join hlc_Doctor d on d.id = cf.DoctorId " +
                      "left join hlc_Hospital h on h.ID = cf.HospitalId " +
                      $" WHERE {where} ORDER BY LastName, FirstName";

            var results = GetListFromSql<Presentation>(sql);

            return results;
        }

        [HttpGet]
        [Route("api/presentation/search/all")]
        public List<Presentation> Search()
        {
            return Search("*");
        }

        /// <summary>
        /// Get all Case File information (including Hospitals, Specialties and Notes)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Presentation Get(int id)
        {

            var sql = "select cf.*, d.FirstName + ' ' + d.LastName as DoctorName,  " +
                      "  dasst.FirstName + ' ' + dasst.LastName as AssistName, " +
                      "  danes.FirstName + ' ' + danes.LastName as AnesthName, " +
                      "  h.HospitalName, dg.DiagnosisName  " +
                      "from hlc_Presentation cf  " +
                      "left join hlc_Doctor d on d.id = cf.DoctorId  " +
                      "left join hlc_Doctor dasst on dasst.id = cf.AssistingID  " +
                      "left join hlc_Doctor danes on danes.id = cf.AnesthID " +
                      "left join hlc_Hospital h on h.ID = cf.HospitalId  " +
                      "left join hlc_Diagnosis dg on dg.Id = cf.DiagnosisId  " +
                      $" WHERE cf.Id = {id}";

            var results = GetListFromSql<Presentation>(sql).FirstOrDefault();
            return results;


        }

        internal bool Save(Presentation model)
        {
            try
            {
                if (model.Id == 0)
                {
                    var newId = Connection.Insert(model);
                    return newId > 0;
                }
                return Connection.Update(model);
            }
            catch (Exception ex)
            {
                LogException(ex, model);
                return false;
            }
        }

        [HttpPost]
        [Route("api/presentation/remove")]
        public bool Remove(HlcDto dto)
        {
            try
            {
                var sql = $"delete from hlc_Presentation where Id={dto.Id};";
                ExecuteSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, new { deleteOp = $"Error deleting Presentation: {dto.Id}" });
                return false;
            }
        }

    }
}
