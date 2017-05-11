using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_Doctor")]
    public class Doctor
    {
        public Doctor()
        {
            Attitude = Attitude.Unknown;
            Status = Status.Unknown;
            RegContacted = YesNoUnknown.Unknown;
            SpecificallyKnown = YesNoUnknown.Unknown;
            FrequentlyTreat = YesNoUnknown.Unknown;
            Helpful = YesNoUnknown.Unknown;
            Specialties = new List<DoctorSpecialty>();
            Hospitals = new List<DoctorHospital>();
            DoctorNotes = new List<DoctorNote>();
        }

        // hlc_Doctor fields
       
        public int Id { get; set; }

        //[Required]
        //[StringLength(50)]
        //[DisplayName("First Name")]
        public string FirstName { get; set; }

        //[Required]
        //[StringLength(50)]
        //[DisplayName("Last Name")]
        public string LastName { get; set; }

        //[Required]
        //[DisplayName("Practice")]
        public int PracticeId { get; set; }

        [Computed]
        public string PracticeName { get; set; }
        [Computed]
        public string OfficePhone1 { get; set; }

        //[StringLength(12)]
        //[DisplayName("Mobile Phone")]
        public string MobilePhone { get; set; }

        //[StringLength(12)]
        //[DisplayName("Home Phone")]
        public string HomePhone { get; set; }

        //[StringLength(12)]
        //[DisplayName("Pager")]
        public string Pager { get; set; }

        //[StringLength(80)]
        //[DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        //[DisplayName("Member of BSMP")]
        // ReSharper disable once InconsistentNaming
        public bool IsBSMP { get; set; }

        //[DisplayName("High Risk Pregnancy doctor")]
        // ReSharper disable once InconsistentNaming
        public bool IsHRP { get; set; }

        //[DisplayName("Peer review")]
        public string PeerReview { get; set; }

        public Attitude Attitude { get; set; }
        public bool FavAdultEmergency { get; set; }
        public bool FavAdultNonEmergency { get; set; }
        public bool NotFavAdult { get; set; }
        public bool FavChildEmergency { get; set; }
        public bool FavChildNonEmergency { get; set; }
        public bool NotFavChild { get; set; }
        public bool AcceptsMedicaid { get; set; }
        public bool ConsultAdultEmergency { get; set; }
        public bool ConsultChildEmergency { get; set; }
        // ReSharper disable once InconsistentNaming
        public string NOTES { get; set; }
        public string Docnotes { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        [Required]
        [DisplayName("Status")]
        public Status Status { get; set; }

        [Computed]
        public Status OriginalStatus { get; set; }

        public DateTime StatusDate { get; set; }
        public YesNoUnknown RegContacted { get; set; }
        public YesNoUnknown SpecificallyKnown { get; set; }
        public YesNoUnknown FrequentlyTreat { get; set; }
        public YesNoUnknown Helpful { get; set; }
        public int TreatYears { get; set; }
        // ReSharper disable once InconsistentNaming
        public DateTime DateEntered { get; set; }  // Per Mark Jones request 4/15/17
        public string EnteredBy { get; set; }

        #region Derived Fields
        [Computed]
        public string FullName => (FirstName + " " + LastName);

        [Computed]
        public string AttitudeText => Enum.GetName(Attitude.GetType(), Attitude);

        [Computed]
        public string AttitudeIcon
        {
            // FontAwesome icon to display for this attitude
            get
            {
                switch (Attitude)
                {
                    case Attitude.Cooperative:
                        return "fa-handshake-o";
                    case Attitude.Favorable:
                        return "fa-thumbs-o-up";
                    case Attitude.Limitations:
                        return "fa-hand-stop-o";
                    case Attitude.NotFavorable:
                        return "fa-thumbs-o-down";
                    default:
                        return "fa-question-circle";
                }
            }
        }
        [Computed]
        public string StatusText => Enum.GetName(Status.GetType(), Status);

        [Computed]
        public string AttitudeAdultText
        {
            get
            {
                if (NotFavAdult)
                    return "Not favorable for Adults";
                if (FavAdultEmergency && FavAdultNonEmergency)
                    return "Adults (Emergency and non-emergency)";
                if (FavAdultEmergency)
                    return "Adults (Emergency)";
                if (FavAdultNonEmergency)
                    return "Adults (Non-Emergency)";
                return "Not determined for Adults";
            }
        }

        [Computed]
        public string AttitudeChildText
        {
            get
            {
                if (NotFavChild)
                    return "Not favorable for Child";
                if (FavChildEmergency && FavChildNonEmergency)
                    return "Child (Emergency and non-emergency)";
                if (FavChildEmergency)
                    return "Child (Emergency)";
                if (FavChildNonEmergency)
                    return "Child (Non-Emergency)";
                return "Not determined for Child";
            }
        }

        [Computed]
        public string AcceptsMedicaidText => AcceptsMedicaid ? "Accepts Medicaid" : "Not accepts Medicaid";

        [Computed]
        public string ConsultAdultText => ConsultAdultEmergency
            ? "Consults for Adult emergencies"
            : "Not consults for Adult emergencies";

        [Computed]
        public string ConsultChildText => ConsultChildEmergency
            ? "Consults for Child emergencies"
            : "Not consults for Child emergencies";

        #endregion

        // Related table data
        [Computed]
        public List<DoctorSpecialty> Specialties { get; set; }
        [Computed]
        public List<DoctorHospital> Hospitals { get; set; }
        [Computed]
        public List<DoctorNote> DoctorNotes { get; set; }
        [Computed]
        public Practice Practice { get; set; }
    }
}