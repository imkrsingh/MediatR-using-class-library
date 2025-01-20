using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using data.viewmodel.models;
using Data.ViewModel.Models;
using Microsoft.AspNetCore.Identity;
using Service.RequestModel;

namespace Data.Profiles
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<IdentityRole, RoleResponse>();
            // .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
            //.ForMember(dest => dest.RoleName, act => act.MapFrom(src => src.Name));
            // .ReverseMap();

            CreateMap<UserRegisterRequest, ApplicationUser>();

        }
    }
}
