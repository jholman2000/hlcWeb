using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_PvgMember")]
    public class PvgMember
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        [StringLength(50)]
        public string Address { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string City { get; set; }

        [Display(Name = "State")]
        [StringLength(2)]
        public string State { get; set; }

        [Display(Name = "Zip")]
        [StringLength(10)]
        public string Zip { get; set; }

        [Display(Name = "Home Phone")]
        [StringLength(12)]
        public string HomePhone { get; set; }

        [Display(Name = "Mobile Phone")]
        [StringLength(12)]
        public string MobilePhone { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [StringLength(80)]
        public string EmailAddress { get; set; }

        [Display(Name = "Comments")]
        [StringLength(1500)]
        public string Notes { get; set; }

        [Display(Name = "Congregation")]
        [StringLength(50)]
        public string Congregation { get; set; }

        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        // Derived fields
        [Computed]
        public string FullName => (FirstName + " " + LastName);

        // Related tables
        [Computed]
        public List<PvgMemberHospital> Hospitals { get; set; }
    }
}