using AutoMapper;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.Roles;

namespace ImageStoreBase.Api.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map từ Entity sang DTO
            CreateMap<Role, RoleResponse>();

            // Map ngược từ DTO sang Entity
        }
    }
}
