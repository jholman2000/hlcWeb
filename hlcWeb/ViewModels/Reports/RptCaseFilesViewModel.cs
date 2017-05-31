using System;

namespace hlcWeb.ViewModels.Reports
{
    public class RptCaseFilesViewModel
    {
        public int Id { get; set; }
        public DateTime CaseDate { get; set; }
        public DateTime DateEntered { get; set; }
        public string UserName { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public string HospitalName { get; set; }
        public string DiagnosisName { get; set; }
        public bool IsPediatricCase{ get; set; }
        public bool CourtOrderSought { get; set; }
        public bool CourtOrderGranted { get; set; }
        public bool TransfusionGiven { get; set; }
    }
}