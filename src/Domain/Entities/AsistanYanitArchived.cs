using System.Text.Json;

namespace GeminiAsistanBackend.Domain.Entities;

public class AsistanYanitArchived
{
    public int Id { get; set; }

    public string AsistanYanit { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset DeletedAt { get; set; }

    public int? CihazKomutId { get; set; }

    public string YanitTuru { get; set; } = string.Empty;

    public string? KullaniciGeriBildirimi { get; set; }

    public string SessionId { get; set; } = string.Empty;

    public JsonDocument? JsonData { get; set; }

    public CihazKomutu? CihazKomut { get; set; }
}