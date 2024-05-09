using Microsoft.EntityFrameworkCore;
using TvMaze.Client;
using TvMazeScraper.Api.InternalModels;
using Cast = TvMazeScraper.Api.InternalModels.Cast;

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
    
    public async Task<ShowContent?[]> GetMultiAsyncOld(int[]? externalIds)
    {
        if (externalIds == null || externalIds.Length == 0)
        {
            return Array.Empty<ShowContent?>().ToArray();
        }

        var records = await _context.MyModels
            .Where(m => externalIds.Contains(m.ExternalId))
            .ToArrayAsync();
        return records;
    }
    
    public async Task<ShowResponse[]> GetMultiAsync(int[]? externalIds)
    {
        if (externalIds == null || externalIds.Length == 0)
        {
            return Array.Empty<ShowResponse>().ToArray();
        }
        
        var records = await _context.MyModels
            .Where(m => externalIds.Contains(m.ExternalId))
            .Select(r => new 
            {
                r.ExternalId,
                r.Content
            })
            .ToListAsync();

        var result = records.Select(r => new ShowResponse
        {
            Name = r.Content.Name,
            Id = r.ExternalId,
            Cast = r.Content.Embedded.Cast.Select(c => new Cast()
                {
                    Id = c.Character.Id,
                    Name = c.Person.Name,
                    Birthday = c.Person.Birthday
                }).OrderByDescending(d => d.Birthday == null ? DateOnly.MinValue : DateOnly.Parse(d.Birthday))
                .ToArray()
        }).ToArray();
        return result;
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
    
    public async Task CreateRecordsAsync((int externalId, Show content)[] records)
    {
        _context.MyModels.AddRange(records.Select(r => new ShowContent
        {
            ExternalId = r.externalId,
            Content = r.content
        }));
        await _context.SaveChangesAsync();
    }
}