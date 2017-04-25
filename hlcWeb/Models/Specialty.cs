using Dapper.Contrib.Extensions;

namespace hlcWeb.Models
{
    [Table("hlc_Specialty")]
    public class Specialty
    {
        public int ID { get; set; }
        public string SpecialtyName { get; set; }
        public string SpecialtyCode { get; set; }
        public string AreaOfExpertise { get; set; }

        [Computed]
        public int NumberOfDoctors { get; set; }
    }
}