using System;
using System.ComponentModel.DataAnnotations;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_User")]
    public class User
    {
        [Dapper.Contrib.Extensions.Key]
        [Required]
        [Display(Name = "User Id")]
        [StringLength(12)]
        public string UserId { get; set; }

        public int CommitteeId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public string UserRole { get; set; }

        [Display(Name = "Address")]
        [StringLength(80)]
        public string Address { get; set; }

        [Display(Name = "City")]
        [StringLength(50)]
        public string City { get; set; }

        [Display(Name = "State")]
        [StringLength(2)]
        public string State { get; set; }

        [Display(Name = "Zip")]
        [StringLength(10)]
        public string Zip { get; set; }

        [Display(Name = "Home Phone")]
        [StringLength(12)]
        public string HomePhone { get; set; }

        [Display(Name = "Mobile Phone")]
        [StringLength(12)]
        public string CellPhone { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        [StringLength(80)]
        public string EmailAddress { get; set; }

        public DateTime? DateLastOn { get; set; }

        public bool IsActive { get; set; }

        public bool MustChangePassword { get; set; }

        // Derived fields
        [Computed]
        public string FullName => (FirstName + " " + LastName);

        [Computed]
        public string OriginalUserId { get; set; }
    }
}