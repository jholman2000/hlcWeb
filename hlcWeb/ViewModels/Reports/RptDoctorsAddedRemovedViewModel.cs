using hlcWeb.Models;
using System;

namespace hlcWeb.ViewModels.Reports
{
    public class RptDoctorsAddedRemovedViewModel
    {
        public string TransType { get; set; }
        public DateTime TransDate { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string UserName { get; set; }
        public string DoctorName { get; set; }
        public Attitude Attitude { get; set; }
        public Status Status { get; set; }
        public string PracticeName { get; set; }
        //public DateTime DateEntered { get; set; }
        //public string Notes { get; set; }
    }
}