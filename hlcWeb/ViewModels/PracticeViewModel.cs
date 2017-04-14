using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    /// <summary>
    /// Practices search drilldown: List Hospital info and list of Doctors at this Hospital
    /// </summary>
    public class PracticeViewModel
    {
        public Practice Practice { get; set; }
        public List<DoctorListViewModel> Doctors { get; set; }
    }
}