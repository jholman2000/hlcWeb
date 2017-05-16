using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_DoctorHospital")]
    public class DoctorHospital
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Computed]
        public string HospitalName { get; set; }
    }
}