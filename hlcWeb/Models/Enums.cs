using System.ComponentModel.DataAnnotations;

namespace hlcWeb.Models
{
    public enum Attitude
    {
        Unknown = 0,
        Cooperative = 1,
        Favorable = 2,
        Limitations = 3,

        [Display(Name = "Not Favorable")]
        NotFavorable = 4
    }

    public enum YesNoUnknown
    {
        Unknown = 0,
        Yes = 1,
        No = 2
    }

    public enum Status
    {
        [Display(Name = "Unknown")]
        Unknown = 0,

        [Display(Name = "Newly Identified")]
        NewlyIdentified = 1,

        [Display(Name = "Letter Sent")]
        LetterSent = 2,

        Deceased = 7,
        [Display(Name = "Moved Out of Area")]
        MovedOutOfArea = 8,

        Active = 9,

        Retired = 10,

        Deleted = 99
    }

    public enum CourtOrderSoughtBy
    {
        None = 0,
        [Display(Name = "Child Protective Services")]
        ChildProtectiveServices = 1,

        [Display(Name = "County/State Social Worker")]
        CountyStateSocialWorker = 2,

        [Display(Name = "Hospital Social Worker")]
        HospitalSocialWorker = 3,

        [Display(Name = "Hospital Attorney")]
        HospitalAttorney = 4,

        [Display(Name = "Risk Management")]
        RiskManagement = 5,

        [Display(Name = "Treating Physician")]
        TreatingPhysician = 6,

        Other                   = 99
    }

    public enum CourtOrderNotGrantedReason
    {
        Unknown = 0,

        [Display(Name = "Questionable emergency")]
        Questionable = 1,

        [Display(Name = "Unlikely blood needed")]
        UnlikelyBloodNeeded = 2,

        [Display(Name = "Child transferred different facility")]
        ChildTransferred = 3,

        [Display(Name = "Succesful legal representation")]
        SuccessLegal = 4,

        [Display(Name = "Mature Minor")]
        MatureMinor = 5,

        [Display(Name = "")]
        Other = 99
    }
}