using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hlcWeb.Models
{
    public class Specialty
    {
        public int ID { get; set; }
        public string SpecialtyName { get; set; }
        public string SpecialtyCode { get; set; }
        public string AreaOfExpertise { get; set; }
    }
}