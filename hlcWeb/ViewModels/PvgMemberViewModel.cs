using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    public class PvgMemberViewModel
    {
        public PvgMember PvgMember { get; set; }
        public List<PvgMemberHospital> Hospitals { get; set; }
    }
}