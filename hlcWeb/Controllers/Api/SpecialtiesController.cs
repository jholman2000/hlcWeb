﻿using System;
using System.Collections.Generic;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.Controllers.Api
{
    public class SpecialtiesController : BaseController
    {
        public List<Specialty> Search(string search)
        {
            string where;

            if (search == "XYZ")
            {
                where = "SpecialtyName LIKE 'X%' OR SpecialtyName LIKE 'Y%' OR SpecialtyName LIKE 'Z%'";
            }
            else if (search.Length == 1)
            {
                where = $"SpecialtyName LIKE '{search}%'";
            }
            else
            {
                where = $"SpecialtyName LIKE '%{search}%'";

            }
            var sql = "SELECT ID, SpecialtyName, " +
                      "(SELECT COUNT(ID) FROM hlc_DoctorSpecialty ds WHERE ds.SpecialtyID = s.ID) as NumberOfDoctors " +
                      "FROM hlc_Specialty s " +
                      $"WHERE {where} ORDER BY SpecialtyName";

            var results = GetListFromSql<Specialty>(sql);

            return results;

        }

        internal List<SpecialtyDoctorViewModel> GetDoctors(int id)
        {
            var sql =
                "select d.id, d.FirstName + ' ' +d.LastName as DoctorName, d.MobilePhone, p.PracticeName, p.OfficePhone1 " +
                "from hlc_DoctorSpecialty ds " +
                "left join hlc_Doctor d on d.ID = ds.DoctorID " +
                "left join hlc_Practice p on p.ID = d.PracticeID " +
                $"where ds.SpecialtyID = {id}";

            var results = GetListFromSql<SpecialtyDoctorViewModel>(sql);

            return results;
        }
    }
}
