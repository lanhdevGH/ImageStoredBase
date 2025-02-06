using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.DTOs.CommandDTO;
using ImageStoreBase.Api.DTOs.GenericDTO;

namespace ImageStoreBase.Api.Services
{

    public interface ICommandService
    {
        Task<PagedResult<Command>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Command>> GetAllAsync();
        Task<Command> GetByIdAsync(string id);
        Task<string> CreateAsync(CommandCreateRequestDTO entity);
        Task<bool> UpdateAsync(string id, CommandUpdateRequestDTO entity);
        Task<bool> DeleteAsync(string id);
    }
}
