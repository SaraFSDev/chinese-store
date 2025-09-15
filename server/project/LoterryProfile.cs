using AutoMapper;
using project.Models;
using project.Models.ModelsDto;
using System;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace project
{
    public class LoterryProfile : Profile
    {
        public LoterryProfile()
        {
            CreateMap<Gift, GiftDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src =>
             src.Category != null ? src.Category.name : null)); 

            CreateMap<GiftDto, Gift>()
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore()) 
                .ForMember(dest => dest.Category, opt => opt.Ignore()); 

            CreateMap<Donator, DonatorDto>();
        }

    }
}


