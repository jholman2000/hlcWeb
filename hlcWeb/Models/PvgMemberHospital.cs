using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_PvgMemberHospital")]
    public class PvgMemberHospital
    {
        public int Id { get; set; }
        public int PvgMemberId { get; set; }

        [Display(Name = "Hospital")]
        public int HospitalId { get; set; }

        [Display(Name = "Weekday")]
        public DayOfWeek DayOfWeek { get; set; }

        public string Notes { get; set; }

        [Computed]
        public string HospitalName { get; set; }

        [Computed]
        public string Weekday { get; set; }

        [Computed]
        public bool Remove { get; set; }  // used by EditAssignments 

    }
}