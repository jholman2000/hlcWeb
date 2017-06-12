using System.Collections.Generic;

namespace hlcWeb.ViewModels.Reports
{
    public class RptAnnualReport
    {
        public List<RptNameCount> Hospitals { get; set; }
        public List<RptNameCount> CoopDoctors { get; set; }
    }

    public class RptNameCount
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}