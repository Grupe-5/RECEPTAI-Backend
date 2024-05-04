using receptai.api.Dtos.Subfooddit;
using receptai.api.Helpers;
using receptai.data;

namespace receptai.api.Interfaces;

public interface ISubfoodditRepository
{
    Task<List<Subfooddit>> GetAllAsync(
        QuerySubfooddit query);

    Task<Subfooddit?> GetByIdAsync(int id);

    Task<Subfooddit> CreateAsync(Subfooddit subfoodditModel);

    Task<Subfooddit?> UpdateAsync(int id,
        UpdateSubfoodditRequestDto subfoodditDto);

    Task<Subfooddit?> DeleteAsync(int id);
}
