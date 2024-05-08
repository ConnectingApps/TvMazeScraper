using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMazeScraper.Api.EF;

public class ShowContent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public int ExternalId { get; set; } // Unique constraint is set in OnModelCreating

    public JsonContent Content { get; set; } = null!;
}