using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_CaseFile")]
    public class CaseFile
    {
        public CaseFile()
        {
            CaseDate = DateTime.Now;
            DateEntered = DateTime.Now;
        }
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CaseDate { get; set; }

        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string UpdatedBy { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(80)]
        public string CongregationName { get; set; }

        public bool IsPediatricCase { get; set; }
        public bool CourtOrderSought { get; set; }
        public CourtOrderSoughtBy CourtOrderSoughtBy { get; set; }
        public bool CourtOrderGranted { get; set; }
        public CourtOrderNotGrantedReason CourtOrderNotGrantedReason { get; set; }
        public string CourtOrderComments { get; set; }
        public bool TransfusionGiven { get; set; }
        public bool ParentsChargedNeglect { get; set; }
        public bool ParentsRightsRemoved { get; set; }
        public bool PatientTransferred { get; set; }
        public string TransferDetails { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int HospitalId { get; set; }

        public string MedicalDiagnosis { get; set; }
        public string MedicalHistory { get; set; }
        public string TreatmentPlan { get; set; }
        public string PossibleStrategies { get; set; }
        public string ArticlesShared { get; set; }
        public string ConsultingDoctor { get; set; }
        public string Outcome { get; set; }

        // Derived fields
        [Computed]
        public string PatientName => (FirstName + " " + LastName);

        [Computed]
        public string DoctorName { get; set; }

        [Computed]
        public string HospitalName { get; set; }
        
    }
}