using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers.Api
{
    public class SpecialtiesController : BaseController
    {
        internal List<Specialty> Search(string search)
        {
            var where = search == "*"
                ? "1=1"
                : $"SpecialtyName LIKE '%{search}%'";

            var sql = "SELECT ID, SpecialtyName, " +
                      "(SELECT COUNT(ID) FROM hlc_DoctorSpecialty ds WHERE ds.SpecialtyID = s.ID) as NumberOfDoctors " +
                      "FROM hlc_Specialty s " +
                      $"WHERE {where} ORDER BY SpecialtyName";

            var results = GetListFromSql<Specialty>(sql);

            return results;

        }

        internal SpecialtyViewModel Get(int id)
        {
            var model = new SpecialtyViewModel();
            using (var conn = Connection)
            {
                var sql = $"select * from hlc_Specialty where ID={id};" +
                          "select d.id, d.FirstName, d.LastName, d.MobilePhone, d.Attitude, p.PracticeName, p.OfficePhone1 " +
                          "from hlc_DoctorSpecialty ds " +
                          "left join hlc_Doctor d on d.ID = ds.DoctorID " +
                          "left join hlc_Practice p on p.ID = d.PracticeID " +
                          $"where ds.SpecialtyID = {id} " +
                          "order by d.LastName, d.FirstName;";

                conn.Open();
                var multi = conn.QueryMultiple(sql);

                model.Specialty = multi.Read<Specialty>().FirstOrDefault();
                //model.Doctors = multi.Read<DoctorListViewModel>().ToList();
                model.Doctors = multi.Read<Doctor>().ToList();
            }
            return model;
        }
    }
}
