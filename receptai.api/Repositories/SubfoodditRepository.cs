﻿using Microsoft.EntityFrameworkCore;
using receptai.api.Dtos.Subfooddit;
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

    public async Task<List<Subfooddit>> GetAllAsync()
    {
        var subfooddits = _context.Subfooddits.AsQueryable();

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

        await _context.SaveChangesAsync();

        return existingSubfooddit;
    }

    public async Task<bool> AddUserToSubfooddit(int subfoodditId, int userId)
    {
        var user = await _context.Users.Include(s => s.Subfooddits)
                    .FirstOrDefaultAsync(s => s.Id == userId);
        if (user == null) {
            return false;
        }

        var subfooddit = await _context.Subfooddits.FindAsync(subfoodditId);
        if (subfooddit == null) {
            return false;
        }

        if (user.Subfooddits.Contains(subfooddit)) {
            return false;
        }

        user.Subfooddits.Add(subfooddit);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveUserFromSubfooddit(int subfoodditId, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) {
            return false;
        }

        var sub = await _context.Subfooddits.Include(s => s.Users)
                    .Where(s => s.Users.Contains(user))
                    .FirstOrDefaultAsync(s => s.SubfoodditId == subfoodditId);
        if (sub == null) {
            return false;
        }

        sub.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<SubfoodditDto>> GetSubfoodditsByUserId(int userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Subfooddits)
            .Select(u => new SubfoodditDto
            {
                SubfoodditId = u.SubfoodditId,
                Title = u.Title,
                Description = u.Description,
                CreationDate = u.CreationDate,
            })
            .ToListAsync();
    }

    public async Task<List<UserSummaryDto>> GetUsersBySubfoodditId(int subfoodditId)
    {
        return await _context.Subfooddits
            .Where(s => s.SubfoodditId == subfoodditId)
            .SelectMany(s => s.Users)
            .Select(u => new UserSummaryDto
            {
                UserId = u.Id,
                UserName = u.UserName,
                ImageId = u.ImgId       
            })
            .ToListAsync();
    }

}