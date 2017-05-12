using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    /// <summary>
    /// Specialties search drilldown: List Specialty info and list of Doctors with this Specialty
    /// </summary>
    public class SpecialtyViewModel
    {
        public Specialty Specialty { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}