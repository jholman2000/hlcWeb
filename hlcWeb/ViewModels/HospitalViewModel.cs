using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    /// <summary>
    /// Hospitals search drilldown: List Hospital info and list of Doctors at this Hospital
    /// </summary>
    public class HospitalViewModel
    {
        public Hospital Hospital { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}