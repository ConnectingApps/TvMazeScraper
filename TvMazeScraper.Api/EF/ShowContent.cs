using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TvMaze.Client;

namespace TvMazeScraper.Api.EF;

public class ShowContent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int ExternalId { get; set; } // Unique constraint is set in OnModelCreating

    public Show Content { get; set; } = null!;
}