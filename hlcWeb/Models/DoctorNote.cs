using System;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_DoctorNote")]
    public class DoctorNote
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string UserId { get; set; }
        public DateTime DateEntered { get; set; }
        public string Notes { get; set; }
        [Computed]
        public string UserName { get; set; }
        [Computed]
        public string DoctorName { get; set; }
    }
}