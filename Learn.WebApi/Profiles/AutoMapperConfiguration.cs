using AutoMapper;
using Learn.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn.WebApi.Profiles
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(s => s.Name, g => g.MapFrom(c =>$"{c.FirstName} {c.LastName}"))
                .ForMember(s => s.Age, g => g.MapFrom(c => DateTime.Now.Year - c.Birthday.Year));
        }
    }
}
