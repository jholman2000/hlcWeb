using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    [Table("hlc_Doctor")]
    public class DoctorViewModel
    {
        public int Id { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        [Required]
        [DisplayName("Status")]
        public Status Status { get; set; }

        [Computed]
        public Status OriginalStatus { get; set; }

        public Attitude Attitude { get; set; }

        public DateTime StatusDate { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Practice")]
        public int PracticeId { get; set; }
       
        [StringLength(12)]
        [DisplayName("Mobile Phone")]
        public string MobilePhone { get; set; }

        [StringLength(12)]
        [DisplayName("Home Phone")]
        public string HomePhone { get; set; }

        [StringLength(12)]
        [DisplayName("Pager")]
        public string Pager { get; set; }

        [StringLength(80)]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Member of BSMP")]
        // ReSharper disable once InconsistentNaming
        public bool IsBSMP { get; set; }

        [DisplayName("High Risk Pregnancy doctor")]
        // ReSharper disable once InconsistentNaming
        public bool IsHRP { get; set; }

        [DisplayName("Peer review")]
        public string PeerReview { get; set; }
    }
}