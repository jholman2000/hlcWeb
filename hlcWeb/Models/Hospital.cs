using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace hlcWeb.Models
{
    [Table("hlc_Hospital")]
    public class Hospital
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Hospital Name")]
        public string HospitalName { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        public int CommitteeId { get; set; }

        [Display(Name="Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zip { get; set; }
        public string OfficePhone1 { get; set; }
        public string Fax { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Has BSMP?")]
        public bool HasBSMP { get; set; }
        public string BSMPFirstName { get; set; }
        public string BSMPLastName { get; set; }
        public string BSMPAddress { get; set; }
        public string BSMPCity { get; set; }
        public string BSMPState { get; set; }
        public string BSMPZip { get; set; }
        public string BSMPEmailAddress { get; set; }
        public string BSMPHomePhone { get; set; }
        public string BSMPMobilePhone { get; set; }
        public string BSMPNotes { get; set; }

        public HospitalType HospitalType { get; set; }
        public bool HasPediatrics { get; set; }
        public string BSMPCoordName { get; set; }
        public string BSMPCoordPhone { get; set; }
        public bool BSMPCoordIsWitness { get; set; }
        public string BSMPCommitment { get; set; }
        public string BSMPPhone { get; set; }
        public int BSMPNumberOfDoctors { get; set; }

        [Computed]
        public int NumberOfDoctors { get; set; }
    }
}