using System;

namespace hlcWeb.Models
{
    public class User
    {
        public string UserID { get; set; }
        public int CommitteeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public DateTime? DateLastOn { get; set; }
        public bool IsActive { get; set; }
        public bool MustChangePassword { get; set; }
        // Derived fields
        public string FullName
        {
            get { return (FirstName + " " + LastName); }
        }

    }
}