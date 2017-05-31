using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using hlcWeb.Infrastructure;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    [Table("hlc_Doctor")]
    public class DoctorAttitudesViewModel
    {
        public int Id { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime DateEntered { get; set; }  // Per Mark Jones request 4/15/17
        public string EnteredBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [Display(Name= Constants.Attitude)]
        public Attitude Attitude { get; set; }

        [Display(Name="Favorable for adults (emergency)")]
        public bool FavAdultEmergency { get; set; }

        [Display(Name = "Favorable for adults (non-emergency)")]
        public bool FavAdultNonEmergency { get; set; }

        [Display(Name = "Not favorable for adults")]
        public bool NotFavAdult { get; set; }

        [Display(Name = "Favorable for children (emergency)")]
        public bool FavChildEmergency { get; set; }

        [Display(Name = "Favorable for children (non-emergency)")]
        public bool FavChildNonEmergency { get; set; }

        [Display(Name = "Not favorable for children")]
        public bool NotFavChild { get; set; }

        [Display(Name = "Accepts Medicaid")]
        public bool AcceptsMedicaid { get; set; }
        
        [Display(Name = "Willing to consult for adults")]
        public bool ConsultAdultEmergency { get; set; }

        [Display(Name = "Willing to consult for children")]
        public bool ConsultChildEmergency { get; set; }

        [Display(Name = "Is doctor regularly contacted?")]
        public YesNoUnknown RegContacted { get; set; }

        [Display(Name = "Specifically known by you?")]
        public YesNoUnknown SpecificallyKnown { get; set; }

        [Display(Name = "Has been helpful in prior cases?")]
        public YesNoUnknown Helpful { get; set; }

        [Display(Name = "Frequently treats Witnesses?")]
        public YesNoUnknown FrequentlyTreat { get; set; }

        [Display(Name = "If so, how many years")]
        public int TreatYears { get; set; }

        [Computed]
        public string FullName => (FirstName + " " + LastName);


    }
}