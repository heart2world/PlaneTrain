using ACMS.EF;
using ACMS.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ACMS.Mappers
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            //User Mapper
            base.CreateMap<User, UserDto>();
            base.CreateMap<UserDto, User>();


            base.CreateMap<Menu, MenuDto>();
            base.CreateMap<MenuDto, Menu>();

            base.CreateMap<Role, RoleDto>();
            base.CreateMap<RoleDto, Role>();
        }
    }
}