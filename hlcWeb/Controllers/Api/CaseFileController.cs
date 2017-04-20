using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using hlcWeb.Models;

namespace hlcWeb.Controllers.Api
{
    public class CaseFileController : BaseController
    {
        [HttpGet]
        [Route("api/case/search")]
        public List<CaseFile> Search(string search)
        {
            var where = $"cf.LastName LIKE '%{search}%' OR " +
                        $"cf.FirstName LIKE '%{search}%' OR " +
                        $"cf.CongregationName LIKE '%{search}%'";

            var sql = "select cf.ID, cf.CaseDate, cf.FirstName, cf.LastName, cf.CongregationName, cf.DoctorId, cf.HospitalId " +
                      "d.FirstName + ' ' + d.LastName as DoctorName, h.HospitalName " +
                      "from hlc_CaseFile cf " +
                      "left join hlc_Doctor d on d.id = cf.DoctorId " +
                      "left join hlc_Hospital h on h.ID = cf.HospitalId " +
                      $" WHERE {where} ORDER BY LastName, FirstName";

            var results = GetListFromSql<CaseFile>(sql);

            return results;
        }

        /// <summary>
        /// Get all Doctor information (including Hospitals, Specialties and Notes)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CaseFile Get(int id)
        {
            using (var conn = Connection())
            {
                var sql = $"select * from hlc_Case where ID={id};" +
                          $"select ds.*, s.SpecialtyName from hlc_DoctorSpecialty ds left join hlc_Specialty s on s.ID = ds.SpecialtyID where ds.DoctorID = {id} order by SpecialtyName;" +
                          $"select dh.*, h.HospitalName from hlc_DoctorHospital dh left join hlc_Hospital h on h.ID = dh.HospitalID where dh.DoctorID = {id} order by HospitalName;" +
                          $"select dn.*, u.FirstName + ' ' + u.LastName as UserName from hlc_DoctorNote dn left join hlc_User u on u.UserID = dn.UserID where dn.DoctorID = {id} order by DateEntered desc;" +
                          $"select * from hlc_Practice where ID = (select PracticeId from hlc_Doctor where Id={id});";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                var caseFile = multi.Read<CaseFile>().FirstOrDefault();
                if (caseFile != null)
                {
                    //caseFile.Specialties = multi.Read<DoctorSpecialty>().ToList();
                    //caseFile.Hospitals = multi.Read<DoctorHospital>().ToList();
                    //caseFile.Notes = multi.Read<DoctorNote>().ToList();
                    //caseFile.Practice = multi.Read<Practice>().FirstOrDefault();
                }

                return caseFile;

            }

        }


    }
}
