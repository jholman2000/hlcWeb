using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_Practice")]
    public class Practice
    {
        public int Id { get; set; }
        public string PracticeName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string OfficePhone1 { get; set; }
        public string OfficePhone2 { get; set; }
        public string Fax { get; set; }
        public string WebsiteUrl { get; set; }
        public string OfficeManager { get; set; }
    }
}