using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dapper.Contrib.Extensions;
using hlcWeb.Infrastructure;

namespace hlcWeb.Models
{
    [Table("hlc_CaseFile")]
    public class CaseFile
    {
        public CaseFile()
        {
            CaseDate = DateTime.Now;
            DateEntered = DateTime.MinValue;
            DateLastUpdated = DateTime.MinValue;
            DiagnosisId = -1;
        }

        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Case Date")]
        public DateTime CaseDate { get; set; }

        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Age")]
        public int? Age { get; set; }

        //[Required]   -  1/14 Per Mark, may not always know Congregation
        [StringLength(80)]
        [Display(Name="Congregation")]
        public string CongregationName { get; set; }

        [Display(Name = Constants.IsPediatricCase)]
        public bool IsPediatricCase { get; set; }
        public bool CourtOrderSought { get; set; }
        public CourtOrderSoughtBy CourtOrderSoughtBy { get; set; }
        public bool CourtOrderGranted { get; set; }
        public CourtOrderNotGrantedReason CourtOrderNotGrantedReason { get; set; }

        [StringLength(500)]
        public string CourtOrderComments { get; set; }
        public bool TransfusionGiven { get; set; }
        public bool ParentsNotified { get; set; }
        public bool ParentsChargedNeglect { get; set; }
        public bool ParentsRightsRemoved { get; set; }
        public bool PatientTransferred { get; set; }

        [StringLength(500)]
        public string TransferDetails { get; set; }

        [Required]
        [Display(Name = Constants.DoctorId)]
        public int DoctorId { get; set; }

        [Required]
        [Display(Name = Constants.HospitalId)]
        public int HospitalId { get; set; }

        [Display(Name= Constants.DiagnosisId)]
        public int DiagnosisId { get; set; }

        // 1/19/2018: Added two new fields per Mark's request
        [Display(Name = Constants.AssistingId)]
        public int AssistingId { get; set; }

        [Display(Name = Constants.AnesthId)]
        public int AnesthId { get; set; }

        [Computed]
        [StringLength(80)]
        [Display(Name="Other diagnosis")]
        public string DiagnosisOther { get; set; }

        [StringLength(500)]
        public string ConsultingDoctor { get; set; }

        [AllowHtml]
        public string MedicalHistory { get; set; }
        [AllowHtml]
        public string TreatmentPlan { get; set; }
        [AllowHtml]
        public string PossibleStrategies { get; set; }
        [AllowHtml]
        public string ArticlesShared { get; set; }
        [AllowHtml]
        public string Outcome { get; set; }

        // Derived fields
        [Computed]
        public string PatientName => (FirstName + " " + LastName);

        [Computed]
        public string DoctorName { get; set; }

        [Computed]
        public string AssistName { get; set; }

        [Computed]
        public string AnesthName { get; set; }

        [Computed]
        public string HospitalName { get; set; }

        [Computed]
        public string DiagnosisName { get; set; }
    }
}