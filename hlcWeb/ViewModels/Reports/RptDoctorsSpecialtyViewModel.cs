using System;

namespace hlcWeb.ViewModels.Reports
{
    public class RptDoctorsSpecialtyViewModel
    {
        public int Id { get; set; }
        public string SpecialtyName { get; set; }
        public string Status { get; set; }
        public string DoctorName { get; set; }
        public string PracticeName { get; set; }
        public string PracticeCityState { get; set; }
        public string OfficePhone1 { get; set; }
        public string Hospitals { get; set; }
    }
}