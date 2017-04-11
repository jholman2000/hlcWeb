using System.Collections.Generic;

namespace hlcWeb.Models
{
    public class PvgMember
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string EmailAddress { get; set; }
        public string Notes { get; set; }
        public string Congregation { get; set; }

        // Derived fields
        public string FullName => (FirstName + " " + LastName);

        // Related tables
        public List<PvgMemberHospital> Hospitals { get; set; }
    }

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