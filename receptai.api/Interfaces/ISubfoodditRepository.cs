using receptai.api.Dtos.Subfooddit;
using receptai.data;

namespace receptai.api.Interfaces;

public interface ISubfoodditRepository
{
    Task<List<Subfooddit>> GetAllAsync();

    Task<Subfooddit?> GetByIdAsync(int id);

    Task<Subfooddit> CreateAsync(Subfooddit subfoodditModel);

    Task<Subfooddit?> UpdateAsync(int id,
        UpdateSubfoodditRequestDto subfoodditDto);

    Task<Subfooddit?> DeleteAsync(int id);
    Task<bool> AddUserToSubfooddit(int subfoodditId, int userId);
    Task<bool> RemoveUserFromSubfooddit(int subfoodditId, int userId);
    Task<List<UserSubfoodditDto>> GetSubfoodditsByUserId(int userId);
    Task<List<UserSummaryDto>> GetUsersBySubfoodditId(int subfoodditId);
}
