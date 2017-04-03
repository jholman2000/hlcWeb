using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hlcWeb.Models
{
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
            Notes = new List<DoctorNote>();
        }

        // hlc_Doctor fields
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PracticeID { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string Pager { get; set; }
        public string EmailAddress { get; set; }
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
        public string NOTES { get; set; }
        public string DOCNOTES { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public Status Status { get; set; }
        public DateTime StatusDate { get; set; }
        public YesNoUnknown RegContacted { get; set; }
        public YesNoUnknown SpecificallyKnown { get; set; }
        public YesNoUnknown FrequentlyTreat { get; set; }
        public YesNoUnknown Helpful { get; set; }
        public int TreatYears { get; set; }
        public bool IsHRP { get; set; }         // High Risk Pregnancy
        public bool IsBSMP { get; set; }        // Bloodless Surgery Management Program
        public string PeerReview { get; set; }

        // Derived fields
        public string FullName
        {
            get { return (FirstName + " " + LastName); }
        }
        public string AttitudeText
        {
            get { return Enum.GetName(Attitude.GetType(), Attitude); }
        }
        public string StatusText
        {
            get { return Enum.GetName(Status.GetType(), Status); }
        }

        // Related table data
        public List<DoctorSpecialty> Specialties { get; set; }
        public List<DoctorHospital> Hospitals { get; set; }
        public List<DoctorNote> Notes { get; set; }
    }

    public class DoctorSpecialty
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public string AreaOfExpertise { get; set; }
        public string SpecialtyName { get; set; }
    }
    public class DoctorHospital
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public string Notes { get; set; }
        public string HospitalName { get; set; }
    }
    public class DoctorNote
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime DateEntered { get; set; }
        public string Notes { get; set; }
        public string UserName { get; set; }
    }
    public enum Attitude
    {
        Unknown = 0,
        Cooperative = 1,
        Favorable = 2,
        Limitations = 3,
        NotFavorable = 4
    }

    public enum Status
    {
        Unknown = 0,
        NewlyIdentified = 1,
        LetterSent = 2,
        Deceased = 7,
        MovedOutOfArea = 8,
        Active = 9,
        Retired = 10
    }
    public enum YesNoUnknown
    {
        Unknown = 0,
        Yes = 1,
        No = 2
    }
}