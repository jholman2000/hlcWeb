using System;
using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_Department")]
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
    }
}