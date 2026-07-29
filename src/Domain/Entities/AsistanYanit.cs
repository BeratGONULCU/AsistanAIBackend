using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Enums;

namespace GeminiAsistanBackend.Domain.Entities;

public class AsistanYanit
{
    public int id {  get; set; }
    public string asistan_yanit { get; set; } = null!;
    public AsistanYanitTuru yanitTuru { get; set; }     
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
    public int SessionId { get; set; }
    public string? feedback {  get; set; }
    public string? KullaniciGeriBildirimi { get; set; }
    public int? cihaz_komut_id {  get; set; }
    public CihazKomutu cihazkomutu { get; set; } = null!;
    public bool IsArchived { get; set; } = false;

    // burada get gibi işlemlerde gelen json değeri kaydetmek için kullanılacak.
    [Column(TypeName = "jsonb")]
    public JsonElement? JsonData { get; set; }
}

/*
 Table asistan_yanit {
  session_id varchar [not null, note: 'UI tarafından üretilen benzersiz sohbet IDsi']
  kullanici_geri_bildirimi varchar [null, note: 'Eğer REJECTED ise kullanıcının girdiği açıklama']
}
 */