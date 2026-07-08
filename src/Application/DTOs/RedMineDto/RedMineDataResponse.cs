using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GeminiAsistanBackend.Application.DTOs.RedMineDto;

public class RedMineDataResponse
{
    [JsonPropertyName("issues")]
    public List<RedMineIssue> Issues { get; set; } = new();
}

public class RedMineIssue
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("project")]
    public RedMineProject? Project { get; set; }

    [JsonPropertyName("tracker")]
    public RedMineTracker? Tracker { get; set; }

    [JsonPropertyName("status")]
    public RedMineStatus? Status { get; set; }

    [JsonPropertyName("priority")]
    public RedMinePriority? Priority { get; set; }

    [JsonPropertyName("author")]
    public RedMineAuthor? Author { get; set; }

    [JsonPropertyName("assigned_to")]
    public RedMineAssignedTo? AssignedTo { get; set; }

    [JsonPropertyName("subject")]
    public string? Subject { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime? Start_date { get; set; }

    [JsonPropertyName("due_date")]
    public DateTime? End_date { get; set; }

    [JsonPropertyName("done_ratio")]
    public int DoneRatio { get; set; }

    [JsonPropertyName("is_private")]
    public bool? IsPrivate { get; set; }

    [JsonPropertyName("estimated_hours")]
    public decimal? EstimatedHours { get; set; } // decimal yapıldı

    [JsonPropertyName("total_estimated_hours")]
    public decimal? TotalEstimatedHours { get; set; } // decimal yapıldı

    [JsonPropertyName("spent_hours")]
    public decimal? SpentHours { get; set; } // decimal yapıldı

    [JsonPropertyName("total_spent_hours")]
    public decimal? TotalSpentHours { get; set; } // decimal yapıldı

    [JsonPropertyName("custom_fields")]
    public List<RedMineCustomFields> CustomFields { get; set; } = new();

    [JsonPropertyName("created_on")]
    public DateTime? CreatedOn { get; set; }

    [JsonPropertyName("updated_on")]
    public DateTime UpdatedOn { get; set; }

    [JsonPropertyName("closed_on")]
    public DateTime? ClosedOn { get; set; }
}

public class RedMineProject
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty; // Uyarı düzeltildi
}

public class RedMineTracker
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty; // Uyarı düzeltildi
}

public class RedMineStatus
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty; // Uyarı düzeltildi

    [JsonPropertyName("is_closed")]
    public bool Status { get; set; }
}

public class RedMinePriority
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty; // Uyarı düzeltildi
}

public class RedMineAuthor
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty; // Uyarı düzeltildi
}

public class RedMineAssignedTo
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty; // Uyarı düzeltildi
}

public class RedMineCustomFields
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty; // Uyarı düzeltildi

    [JsonPropertyName("multiple")]
    public bool? Multiple { get; set; }

    [JsonPropertyName("value")]
    public object? Value { get; set; } // Nullable yapıldı (Bazen string, bazen array gelebildiği için)
}
