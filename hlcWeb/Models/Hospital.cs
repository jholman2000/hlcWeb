﻿using System;
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using hlcWeb.Infrastructure;

namespace hlcWeb.Models
{
    [Table("hlc_Hospital")]
    public class Hospital
    {
        public int Id { get; set; }

        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        [Required]
        [Display(Name ="Hospital Name")]
        [StringLength(80)]
        public string HospitalName { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        [Display(Name = "State")]
        public string State { get; set; }

        public int CommitteeId { get; set; }

        [Display(Name="Address")]
        [StringLength(80)]
        public string Address1 { get; set; }

        [StringLength(80)]
        public string Address2 { get; set; }

        [StringLength(10)]
        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [StringLength(14)]
        [Display(Name="Main Phone")]
        public string OfficePhone1 { get; set; }

        [StringLength(14)]
        public string Fax { get; set; }
        public string Notes { get; set; }

        [Display(Name = "This hospital has a Blood Management Surgery Program (BMSP)")]
        public bool HasBSMP { get; set; }

        // These are old fields that can be removed from the table at a later date.
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
        // (End old fields)

        /***************************************************
         * New fields used for the Annual HLC Questionnaire
         ***************************************************/
        [Required]
        [Display(Name = Constants.HospitalType)]
        public HospitalType HospitalType { get; set; }

        [Display(Name = "This hospital often receives Witness patients")]
        public bool OftenReceivesWitnesses { get; set; }

        [Display(Name = "This hospital has Pediatrics care")]
        public bool HasPediatrics { get; set; }

        [Display(Name = "Coordinator")]
        [StringLength(80)]
        public string BmspCoordName { get; set; }

        [Display(Name = "Phone")]
        [StringLength(12)]
        public string BmspCoordPhone { get; set; }

        [Display(Name= "Coordinator is a Witness")]
        public bool BmspCoordIsWitness { get; set; }

        [Display(Name = "Level of Commitment:")]
        public string BmspCommitment { get; set; }

        [Display(Name = "Outstanding medical specialties:")]
        public string BmspSpecialties { get; set; }

        public string BmspPhone { get; set; }

        [Display(Name = "# Cooperative")]
        [StringLength(50)]
        public string BmspNumberOfDoctors { get; set; }

        [Computed]
        public int NumberOfDoctors { get; set; }
    }
}