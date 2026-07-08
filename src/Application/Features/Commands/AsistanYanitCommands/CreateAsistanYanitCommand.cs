using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

public sealed record CreateAsistanYanitCommand(
    string asistan_yanit,
    AsistanYanitTuru yanit_turu,
    int? cihaz_komut_id,
    int session_id,
    string? feedback
    ) : IRequest<AsistanSendResponse>;

/*
  Table asistan_yanit {
  id integer [primary key , increment]
  cihaz_komut_id int [not null] // bu kısım gelen komuta bağlanacak
  asistan_yanit varchar [not null] // burası asistan yanıtı
  created_at datetime 
  updated_at datetime
}
 */
