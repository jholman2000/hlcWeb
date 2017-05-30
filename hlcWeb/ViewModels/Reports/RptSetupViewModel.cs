using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using hlcWeb.Infrastructure;

namespace hlcWeb.ViewModels.Reports
{
    public class RptSetupViewModel
    {
        public RptSetupViewModel()
        {
            DateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            DateTo = DateTime.Today;
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

        public bool IsPediatricCase { get; set; }
    }
}