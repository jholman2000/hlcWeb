﻿using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    public class DoctorSpecialtiesViewModel
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public List<DoctorSpecialty> Specialties { get; set; }
    }
}