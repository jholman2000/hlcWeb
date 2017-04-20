using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;
using hlcWeb.Models;
namespace hlcWeb.Models
{
    [Table("hlc_CaseFile")]
    public class CaseFile
    {
        public int Id { get; set; }
        public DateTime CaseDate { get; set; }
        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CongregationName { get; set; }
        public bool IsPediatricCase { get; set; }
        public bool CourtOrderSought { get; set; }
        public string CourtOrderSoughtBy { get; set; }
        public bool CourtOrderGranted { get; set; }
        public string CourtOrderComments { get; set; }
        public bool TransfusionGiven { get; set; }
        public bool ParentsChargedNeglect { get; set; }
        public bool ParentsRightsRemoved { get; set; }
        public bool PatientTransferred { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public string MedicalDiagnosis { get; set; }
        public string MedicalHistory { get; set; }
        public string TreatmentPlan { get; set; }
        public string PossibleStrategies { get; set; }
        public string ArticlesShared { get; set; }
        public string ConsultingDoctor { get; set; }
        public string Outcome { get; set; }

        [Computed]
        public string PatientName => (FirstName + " " + LastName);

        [Computed]
        public string DoctorName { get; set; }

        [Computed]
        public string HospitalName { get; set; }
        
    }
}