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
        [Route("api/presentations/search/{search}")]
        public List<Presentation> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"Description LIKE '%{search}%' OR " +
                  $"FacilityName LIKE '{search}%' OR " +
                  $"CoordinatorName LIKE '{search}%' OR " +
                  $"DepartmentName LIKE '{search}%' OR " +
                  $"ContactName LIKE '{search}%' ";

            var sql = "with PresList as (" +
                "select p.Id, p.DatePlanned, p.Description, f.PracticeName as FacilityName, coord.FirstName + ' ' + coord.LastName as CoordinatorName, d.DepartmentName, p.ContactName, p.DatePresented " +
                "from hlc_Presentation p " +
                "left join hlc_Practice f on f.ID = p.FacilityId " +
                "left join hlc_Department d on d.Id = p.DepartmentId " +
                "left join hlc_User coord on coord.UserID = p.CoordinatorID " +
                "where p.PresentationFacilityType <> 99 " +
                "union all " +
                "select p.Id, p.DatePlanned, p.Description, h.HospitalName as FacilityName, coord.FirstName + ' ' + coord.LastName as CoordinatorName, d.DepartmentName, p.ContactName, p.DatePresented " +
                "from hlc_Presentation p " +
                "left join hlc_Hospital h on h.ID = p.FacilityId " +
                "left join hlc_Department d on d.Id = p.DepartmentId " +
                "left join hlc_User coord on coord.UserID = p.CoordinatorID " +
                "where p.PresentationFacilityType = 99 )" +
                $"select * from PresList where {where} order by DatePlanned, FacilityName";

            var results = GetListFromSql<Presentation>(sql);

            return results;
        }

        [HttpGet]
        [Route("api/presentations/search/all")]
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

            var sql = "select p.*, f.PracticeName as FacilityName, d.DepartmentName, ft.Description as FacilityTypeName, coord.FirstName + ' ' + coord.LastName as CoordinatorName " +
                      "from hlc_Presentation p " +
                      "left join hlc_Practice f on f.ID = p.FacilityId " +
                      "left join hlc_Department d on d.Id = p.DepartmentId " +
                      "left join hlc_FacilityType ft on ft.Id = p.PresentationFacilityType " +
                      "left join hlc_User coord on coord.UserID = p.CoordinatorID " +
                      $"where p.id={id} and p.PresentationFacilityType <> 99 " +
                      "union " +
                      "select p.*, h.HospitalName as FacilityName, d.DepartmentName, ft.Description as FacilityTypeName, coord.FirstName + ' ' + coord.LastName as CoordinatorName " +
                      "from hlc_Presentation p " +
                      "left join hlc_Hospital h on h.ID = p.FacilityId " +
                      "left join hlc_Department d on d.Id = p.DepartmentId " +
                      "left join hlc_FacilityType ft on ft.Id = p.PresentationFacilityType " +
                      "left join hlc_User coord on coord.UserID = p.CoordinatorID " +
                      $"where p.id={id} and p.PresentationFacilityType = 99";
            
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
