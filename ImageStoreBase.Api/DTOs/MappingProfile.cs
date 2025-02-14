using AutoMapper;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CollectionDTOs;
using ImageStoreBase.Api.DTOs.CommandDTO;
using ImageStoreBase.Api.DTOs.FunctionDTOs;
using ImageStoreBase.Api.DTOs.PermissionDTO;
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
            CreateMap<Command, CommandResponseDTO>();
            CreateMap<Collection, CollectionResponseDTO>();
            CreateMap<User, UserResponseDTO>();
            CreateMap<Function, FunctionResponseDTO>()
                .ForMember(dest => dest.ParentName, otps => otps.MapFrom(src => src.FunctionParent.Name))
                .ForMember(dest => dest.ChildFunctions, opt => opt.Ignore())
                .AfterMap((src, dest, rc) =>
                {
                    if (src.ChildFunctions != null)
                    {
                        foreach (var item in src.ChildFunctions)
                        {
                            if (item != src) // Prevent circular reference
                            {
                                dest.ChildFunctions.Add(rc.Mapper.Map<FunctionResponseDTO>(item));
                            }
                        }
                    }
                });

            // Map ngược từ DTO sang Entity
            CreateMap<PermissionVMDTO, Permission>();
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
