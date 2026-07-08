using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

// bu sadece ilk komut geldiğinde çalışacak ve sessionid üretilmesini sağlar
public sealed record CreateSessionCommand(
    string asistan_yanit,
    AsistanYanitTuru yanit_turu // burada komut olarak gelecek
    ) : IRequest<AsistanSendResponse>;