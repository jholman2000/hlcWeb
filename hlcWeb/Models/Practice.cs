using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_Practice")]
    public class Practice
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Practice Name")]
        [StringLength(80)]
        public string PracticeName { get; set; }

        [Display(Name = "Address")]
        [StringLength(80)]
        public string Address1 { get; set; }

        [StringLength(80)]
        public string Address2 { get; set; }

        [StringLength(80)]
        public string Address3 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [Display(Name = "Main Phone")]
        [StringLength(12)]
        public string OfficePhone1 { get; set; }

        [Display(Name = "Secondary Phone")]
        [StringLength(12)]
        public string OfficePhone2 { get; set; }

        [Display(Name = "Fax")]
        [StringLength(12)]
        public string Fax { get; set; }

        [Display(Name = "URL")]
        [StringLength(250)]
        public string WebsiteUrl { get; set; }

        [Display(Name = "Office Manager")]
        [StringLength(50)]
        public string OfficeManager { get; set; }
    }
}