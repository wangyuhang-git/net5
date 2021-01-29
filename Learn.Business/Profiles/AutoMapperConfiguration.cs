using AutoMapper;
using Learn.Models.Entity;
using System;

namespace Learn.Business.Profiles
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<ManagePostHistoryAtt, ManagePostAtt>();
        }
    }
}
