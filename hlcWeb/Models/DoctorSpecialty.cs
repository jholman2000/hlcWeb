using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_DoctorSpecialty")]
    public class DoctorSpecialty
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }

        [Display(Name="Specialty")]
        public int SpecialtyId { get; set; }

        [Display(Name = "Area")]
        [StringLength(250)]
        public string AreaOfExpertise { get; set; }

        [Computed]
        public string SpecialtyName { get; set; }

        [Computed]
        public bool Remove { get; set; }  // used by EditSpecialties / DoctorSpecialtiesViewModel
    }
}