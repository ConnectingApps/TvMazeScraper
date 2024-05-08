﻿using Microsoft.EntityFrameworkCore;
using TvMaze.Client;

namespace TvMazeScraper.Api.EF;

public class ShowContentRepository : IShowContentRepository
{
    private readonly MazeContext _context;

    public ShowContentRepository(MazeContext context)
    {
        _context = context;
    }

    // Async method to check if a record exists based on external ID
    public async Task<bool> RecordExistsAsync(int externalId)
    {
        return await _context.MyModels.AnyAsync(sc => sc.ExternalId == externalId);
    }

    public async Task<ShowContent?> GetAsync(int externalId)
    {
        return await _context.MyModels.FirstOrDefaultAsync(m => m.ExternalId == externalId);
    }
    
    public async Task CreateRecordAsync(int externalId, Show content)
    {
        var newRecord = new ShowContent
        {
            ExternalId = externalId,
            Content = content
        };

        _context.MyModels.Add(newRecord);
        await _context.SaveChangesAsync();
    }
}