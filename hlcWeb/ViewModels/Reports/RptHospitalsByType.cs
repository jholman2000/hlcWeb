namespace hlcWeb.ViewModels.Reports
{
    public class RptHospitalsByType
    {
        public int Id { get; set; }
        public int HospitalType { get; set; }
        public string HospitalTypeDesc { get; set; }
        public string HospitalName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string HospitalCityState { get; set; }
        public bool HasBSMP { get; set; }
        public bool HasPediatrics { get; set; }
        public int NumberOfDoctors { get; set; }
    }
}