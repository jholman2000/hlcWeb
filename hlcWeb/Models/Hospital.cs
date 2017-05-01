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

        public string City { get; set; }
        public string State { get; set; }
        public int CommitteeID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Zip { get; set; }
        public string OfficePhone1 { get; set; }
        public string Fax { get; set; }
        public string Notes { get; set; }
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
        public string CoordName { get; set; }
        public string CoordPhone { get; set; }
        public bool CoordIsWitness { get; set; }
        public string BSMPCommitment { get; set; }
        public string BSMPPhone { get; set; }

        [Computed]
        public int NumberOfDoctors { get; set; }
    }
}