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
            CreateMap<Test, TestDto>().ReverseMap(); //ReverseMap表示Test和TestDto可以互转,不加ReverseMap()只能从Test转到TestDto 
            CreateMap(typeof(PagedList<>), typeof(PagedListDto<>));
            //CreateMap<Blog, BlogDto>().ReverseMap();
        }
    }
}
