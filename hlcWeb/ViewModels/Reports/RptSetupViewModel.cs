using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hlcWeb.ViewModels.Reports
{
    public class RptSetupViewModel
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public int DiagnosisId { get; set; }
        public string EnteredBy { get; set; }
        public bool IsPediatricCase { get; set; }
    }
}