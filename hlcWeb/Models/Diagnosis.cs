using System;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_Diagnosis")]
    public class Diagnosis
    {
        public int Id { get; set; }
        public string DiagnosisName { get; set; }
        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }

        [Computed]
        public int NumberOfCases { get; set; }

    }
}