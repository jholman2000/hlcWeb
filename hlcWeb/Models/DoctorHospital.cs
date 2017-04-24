using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("DoctorHospital")]
    public class DoctorHospital
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public string Notes { get; set; }
        public string HospitalName { get; set; }
    }
}