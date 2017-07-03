using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using hlcWeb.Infrastructure;
using hlcWeb.Models;

namespace hlcWeb.ViewModels.Reports
{
    public class RptSetupViewModel
    {
        public RptSetupViewModel()
        {
            DateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            DateTo = DateTime.Today;
            Attitude = Attitude.Cooperative;
        }

        [Display(Name = Constants.DateFrom)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }

        [Display(Name = Constants.DateTo)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; }

        [Display(Name = Constants.DoctorId)]
        public int DoctorId { get; set; }

        [Display(Name = Constants.HospitalId)]
        public int HospitalId { get; set; }

        [Display(Name = Constants.DiagnosisId)]
        public int DiagnosisId { get; set; }

        [Display(Name = Constants.EnteredBy)]
        public string EnteredBy { get; set; }

        [Display(Name = Constants.Attitude)]
        public Attitude Attitude { get; set; }

        [Display(Name = Constants.HospitalType)]
        public int HospitalType { get; set; }

        [Display(Name = "Specialty")]
        public List<int> Specialties { get; set; }

        [Display(Name = Constants.IsPediatricCase)]
        public bool IsPediatricCase { get; set; }
    }
}