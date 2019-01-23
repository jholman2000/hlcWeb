using System;

namespace hlcWeb.Models
{
    public class LookupCode
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string AdditionalText { get; set; }
        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }

    }
}