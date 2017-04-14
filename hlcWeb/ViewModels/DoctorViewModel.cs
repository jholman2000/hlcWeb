using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace hlcWeb.ViewModels
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public int PracticeId { get; set; }

        [StringLength(12)]
        public string MobilePhone { get; set; }

        [StringLength(12)]
        public string HomePhone { get; set; }

        [StringLength(12)]
        public string Pager { get; set; }

        [StringLength(80)]
        public string EmailAddress { get; set; }

    }
}