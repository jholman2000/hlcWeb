using AutoMapper;
using hlcWeb.ViewModels;

namespace hlcWeb.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Models.Doctor, DoctorContactViewModel>();
            Mapper.CreateMap<Models.Doctor, DoctorAttitudesViewModel>();
        }
    }
}