using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Recipe;
using receptai.api.Dtos.Subfooddit;
using receptai.api.Helpers;
using receptai.api.Interfaces;
using receptai.data;

namespace receptai.api.Repositories;

public class SubfoodditRepository : ISubfoodditRepository
{
    private readonly RecipePlatformDbContext _context;

    public SubfoodditRepository(RecipePlatformDbContext context)
    {
        _context = context;
    }

    public async Task<Subfooddit> CreateAsync(Subfooddit subfoodditModel)
    {
        await _context.Subfooddits.AddAsync(subfoodditModel);
        await _context.SaveChangesAsync();

        return subfoodditModel;
    }

    public async Task<Subfooddit?> DeleteAsync(int id)
    {
        var subfoodditModel = await _context.Subfooddits
            .FirstOrDefaultAsync(s => s.SubfoodditId == id);

        if (subfoodditModel is null)
        {
            return null;
        }

        _context.Subfooddits.Remove(subfoodditModel);
        await _context.SaveChangesAsync();

        return subfoodditModel;
    }

    //TODO Get all subfooddits, where user is in
    public async Task<List<Subfooddit>> GetAllAsync(QuerySubfooddit query)
    {
        var subfooddits = _context.Subfooddits.AsQueryable();

        // Filter by UserId if provided
        if (query.UserId.HasValue)
        {
            //TODO subfooddits = subfooddits.Where(r => r.UserId == query.UserId.Value);
        }

        return await subfooddits.ToListAsync();
    }

    public async Task<Subfooddit?> GetByIdAsync(int id)
    {
        return await _context.Subfooddits.FindAsync(id);
    }

    public async Task<Subfooddit?> UpdateAsync(int id,
        UpdateSubfoodditRequestDto subfoodditDto)
    {
        var existingSubfooddit = await _context.Subfooddits
            .FirstOrDefaultAsync(xs => xs.SubfoodditId == id);

        if (existingSubfooddit is null)
        {
            return null;
        }

        existingSubfooddit.Title = subfoodditDto.Title;
        existingSubfooddit.Description = subfoodditDto.Description;
        existingSubfooddit.CreationDate = subfoodditDto.CreationDate;

        await _context.SaveChangesAsync();

        return existingSubfooddit;
    }
}