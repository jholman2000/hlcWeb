﻿using System.ComponentModel.DataAnnotations;

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

    // If adding to Status then need to update the SQL in Api.ReportsController.DoctorsSpecialty()
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
        [Display(Name = "Active")]
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

    // Copied in hlc_HospitalType
    // TODO: See if this can be refactored to read from hlc_HospitalType instead of using Enum or hard-coding
    public enum HospitalType
    {
        Unknown = 0,

        [Display(Name = "Children's Hospital")]
        ChildrensHospital = 1,
        [Display(Name = "Level 1 Trauma Center")]
        Level1TraumaCenter = 2,
        [Display(Name = "Level 2 Trauma Center")]
        Level2TraumaCenter = 3,
        [Display(Name = "Level 3 Trauma Center")]
        Level3TraumaCenter = 4,
        [Display(Name = "Public Hospital")]
        PublicHospital = 5,
        [Display(Name = "University Hospital")]
        UniversityHospital = 6,
        [Display(Name = "Specialized Care")]
        SpecializedCare = 7,
        Other = 9
    }

    public enum DayOfWeek
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7,
        [Display(Name = "As Needed")]
        AsNeeded = 8,
        Alternate = 9,
        Weekends = 10
    }

    // Added for Presentations
    public enum FacilityType
    {
        [Display(Name = "Practice/Physician's office")]
        Practice = 0,
        [Display(Name = "Legal/Law firm")]
        Legal = 1,
        Other = 9
    }

    public enum PresentationFacilityType
    {
        [Display(Name = "Practice/Physician's office")]
        Practice = FacilityType.Practice,
        [Display(Name = "Legal/Law firm")]
        Legal = FacilityType.Legal,
        Other = FacilityType.Other,
        Hospital = 99
    }
}