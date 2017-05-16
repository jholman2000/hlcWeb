using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    public class DoctorHospitalsViewModel
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public List<DoctorHospital> Hospitals { get; set; }
    }
}