using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LibrarieModele;
using eBooks_API.Mapping;
using AutoMapper;
using eBooks_API.Models;

namespace eBooks_API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<Autor, AutorDto>();
            CreateMap<AutorCreateDto, Autor>()
                .ForMember(dest => dest.id_autor, opt => opt.Ignore())
                .ForMember(dest => dest.data_nasterii, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.data_nasterii) ? default(DateTime) : DateTime.Parse(src.data_nasterii)
                ));

            CreateMap<AutorUpdateDto, Autor>()
                .ForMember(dest => dest.id_autor, opt => opt.Ignore())
                .ForMember(dest => dest.data_nasterii, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.data_nasterii) ? default(DateTime) : DateTime.Parse(src.data_nasterii)
                ));
        }
    }
}