namespace hlcWeb.Models
{
    public class PvgMemberHospital
    {
        public int Id { get; set; }
        public int PvgMemberId { get; set; }
        public int HospitalId { get; set; }
        public int DayOfWeek { get; set; }
        public string Notes { get; set; }
        public string HospitalName { get; set; }

    }
}