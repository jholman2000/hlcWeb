using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Dapper.Contrib.Extensions;
using hlcWeb.Infrastructure;

namespace hlcWeb.Models
{
    [Table("hlc_Presentation")]
    public class Presentation
    {
        public Presentation()
        {
            PresentationFacilityType = PresentationFacilityType.Hospital;
        }

        public int Id { get; set; }


        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

        [Required]
        [Display(Name = Constants.PresentationFacilityType)]
        public PresentationFacilityType PresentationFacilityType { get; set; }

        [Required]
        [Display(Name = Constants.FacilityId)]
        public int FacilityId { get; set; }

        [Required]
        [Display(Name = Constants.DepartmentId)]
        public int DepartmentId { get; set; }

        [Display(Name = Constants.Name)]
        [StringLength(80)]
        public string ContactName { get; set; }

        [Display(Name = Constants.Title)]
        [StringLength(80)]
        public string ContactTitle { get; set; }

        [Display(Name = Constants.Address)]
        [StringLength(80)]
        public string ContactAddress { get; set; }

        [Display(Name = Constants.City)]
        [StringLength(50)]
        public string ContactCity { get; set; }

        [Display(Name = Constants.State)]
        [StringLength(2)]
        public string ContactState { get; set; }

        [Display(Name = Constants.Zip)]
        [StringLength(10)]
        public string ContactZip { get; set; }

        [Display(Name = Constants.Phone)]
        [StringLength(12)]
        public string ContactPhone { get; set; }

        [Required]
        [Display(Name = Constants.EmailAddress)]
        [EmailAddress]
        [StringLength(80)]
        public string ContactEmailAddress { get; set; }

        [Required]
        [Display(Name = Constants.CoordinatorId)]
        public int CoordinatorId { get; set; }

        [AllowHtml]
        [Display(Name = Constants.Goal)]
        [StringLength(5000)]
        public string Goal { get; set; }

        [AllowHtml]
        [Display(Name = Constants.Notes)]
        [StringLength(5000)]
        public string Notes { get; set; }

        [AllowHtml]
        [Display(Name = Constants.ReferredBy)]
        [StringLength(5000)]
        public string ReferredBy { get; set; }

        [Display(Name = Constants.IsCVRequired)]
        public bool IsCVRequired { get; set; }

        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Preliminary Date")]
        public DateTime DatePlanned { get; set; }

        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Preparation Meeting")]
        public DateTime DatePreparation { get; set; }

        // Following fields are populated after the Presentation is completed

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Case Date")]
        public DateTime DatePresented { get; set; }

        [Display(Name = Constants.Audience)]
        [StringLength(250)]
        public string Audience { get; set; }

        [AllowHtml]
        [Display(Name = Constants.PVGElders)]
        [StringLength(5000)]
        public string PVGElders { get; set; }

        [AllowHtml]
        [Display(Name = Constants.PresentationTopic)]
        [StringLength(5000)]
        public string PresentationTopic { get; set; }

        [AllowHtml]
        [Display(Name = Constants.QuestionsAddressed)]
        [StringLength(5000)]
        public string QuestionsAddressed { get; set; }

        [AllowHtml]
        [Display(Name = Constants.MaterialGiven)]
        [StringLength(5000)]
        public string MaterialGiven { get; set; }

        [AllowHtml]
        [Display(Name = Constants.FollowUpTasks)]
        [StringLength(5000)]
        public string FollowUpTasks { get; set; }


        // Derived fields

        [Computed]
        public string CoordinatorName { get; set; }

        [Computed]
        public string FacilityName { get; set; }

        [Computed]
        public string DepartmentName { get; set; }

        [Computed]
        public string FacilityTypeName { get; set; }
    }
}