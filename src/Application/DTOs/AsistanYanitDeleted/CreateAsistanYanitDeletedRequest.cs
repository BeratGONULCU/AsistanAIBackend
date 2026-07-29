using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GeminiAsistanBackend.Application.DTOs.AsistanYanitDeleted;

public class CreateAsistanYanitDeletedRequest
{
    [Required]
    [MaxLength(1000)]
    public string AsistanYanit { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public int? CihazKomutId { get; set; }

    [Required]
    [MaxLength(50)]
    public string YanitTuru { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? KullaniciGeriBildirimi { get; set; }

    [Required]
    public string SessionId { get; set; } = string.Empty;

    public JsonDocument? JsonData { get; set; }
}