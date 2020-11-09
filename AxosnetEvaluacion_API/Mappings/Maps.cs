using AutoMapper;
using AxosnetEvaluacion_API.Data;
using AxosnetEvaluacion_API.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxosnetEvaluacion_API.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Moneda, MonedaGetDTO>().ReverseMap();
            CreateMap<Moneda, MonedaPostDTO>().ReverseMap();
            CreateMap<Moneda, MonedaUpdateDTO>().ReverseMap();
        }
    }
}
