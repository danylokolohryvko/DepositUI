using AutoMapper;
using DepositUI.BLL.DTOs;
using DepositUI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DepositUI.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<DepositCalcDTO, DepositCalc>();
            CreateMap<DepositDTO, DepositModel>();
        }
    }
}
