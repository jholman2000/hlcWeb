using System.Collections.Generic;

namespace hlcWeb.ViewModels.Reports
{
    public class RptAnnualReport
    {
        public int HlcCount { get; set; }
        public int PvgCount { get; set; }
        public List<RptNameCount> HospitalTypes { get; set; }
        public List<RptNameCount> CoopDoctors { get; set; }
        public List<RptAnnualBMSP> BMSP { get; set; }
        public List<RptAnnualOftenReceives> OftenReceives { get; set; }
    }

    public class RptAnnualOftenReceives
    {
        public string HospitalName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool HasBSMP { get; set; }
    }
    public class RptAnnualBMSP
    {
        public string HospitalName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string BmspCoordName { get; set; }
        public string BmspCoordPhone { get; set; }
        public bool BmspCoordIsWitness { get; set; }
        public string BmspCommitment { get; set; }
        public string BmspSpecialties { get; set; }
        public string BmspPhone { get; set; }
        public string BmspNumberOfDoctors { get; set; }

    }
    public class RptNameCount
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}