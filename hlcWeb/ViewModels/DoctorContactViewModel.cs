﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    [Table("hlc_Doctor")]
    public class DoctorContactViewModel
    {
        public int Id { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime DateEntered { get; set; }  // Per Mark Jones request 4/15/17
        public string EnteredBy { get; set; }

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

        [Computed]
        public Practice Practice { get; set; }
    
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
        [EmailAddress]
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