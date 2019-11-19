using AutoMapper;
using ST.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreSample
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Test, TestDto>().ReverseMap();
            CreateMap<CreateTestInput, Test>();
            CreateMap<UpdateTestInput, Test>();
        }
    }
}
