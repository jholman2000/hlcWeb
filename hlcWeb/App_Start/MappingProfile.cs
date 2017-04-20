using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using hlcWeb.Models;
using hlcWeb.ViewModels;

namespace hlcWeb.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Doctor, DoctorContactViewModel>();
        }
    }
}