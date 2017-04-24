using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("DoctorSpecialty")]
    public class DoctorSpecialty
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public string AreaOfExpertise { get; set; }
        public string SpecialtyName { get; set; }
    }
}