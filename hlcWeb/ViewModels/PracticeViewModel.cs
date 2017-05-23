﻿using System.Collections.Generic;
using hlcWeb.Models;

namespace hlcWeb.ViewModels
{
    /// <summary>
    /// Practices search drilldown: List Practice info and list of Doctors at this Practice
    /// </summary>
    public class PracticeViewModel
    {
        //TODO: Need to split out the individual fields of Practice instead of the object. Or not?
        public Practice Practice { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}