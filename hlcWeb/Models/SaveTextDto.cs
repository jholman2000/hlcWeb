using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hlcWeb.Models
{
    public class SaveTextDto
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string FieldText { get; set; }
        public int DoctorId { get; set; }
        public string UserId { get; set; }
    }
}