using AutoMapper;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.RoleDTOs;
using ImageStoreBase.Api.DTOs.Roles;
using ImageStoreBase.Api.DTOs.UserDTOs;

namespace ImageStoreBase.Api.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map từ Entity sang DTO
            CreateMap<Role, RoleResponse>();

            // Map ngược từ DTO sang Entity
            CreateMap<UserCreateRequestDTO, User>();
            CreateMap<UserUpdateRequestDTO, User>();
            CreateMap<RoleCreateRequestDTO, Role>();
            CreateMap<RoleUpdateRequestDTO, Role>();
            CreateMap<CollectionCreateRequestDTO, Collection>();
            CreateMap<CollectionUpdateRequestDTO, Collection>();
            CreateMap<FunctionCreateRequestDTO, Function>();
            CreateMap<FunctionUpdateRequestDTO, Function>();
        }
    }
}
