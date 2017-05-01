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
        [Display(Name = "Not applicable")]
        NotApplicable = 0,

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

        [Display(Name = "Other")]
        Other                   = 99
    }

    public enum CourtOrderNotGrantedReason
    {
        [Display(Name = "Not applicable")]
        NotApplicable = 0,

        [Display(Name = "Request pending")]
        RequestPending = 1,

        [Display(Name = "Questionable emergency")]
        Questionable = 2,

        [Display(Name = "Blood unlikely")]
        UnlikelyBloodNeeded = 3,

        [Display(Name = "Child transferred")]
        ChildTransferred = 4,

        [Display(Name = "Successful legal rep")]
        SuccessLegal = 5,

        [Display(Name = "Mature Minor")]
        MatureMinor = 6,

        [Display(Name = "Other")]
        Other = 99
    }

    public enum HospitalType
    {
        Unknown = 0,
        ChildrensHospital = 1,
        Level1TraumaCenter = 2,
        Level2TraumaCenter = 3,
        Level3TraumaCenter = 4,
        PublicHospital = 5,
        UniversityHospital = 6,
        SpecializedCare = 7,
        Other = 9,
    }
}