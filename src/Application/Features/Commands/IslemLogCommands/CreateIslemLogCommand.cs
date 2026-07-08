using GeminiAsistanBackend.Application.DTOs.IslemLog;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.IslemLogCommands;

public sealed record CreateIslemLogCommand(
    string DuyulanSes,
    IslemDurum Durum,
    string? cevapMetni,
    //DateTime tarihSaat,
    int? komutId,
    string raw_ai_json
    ) : IRequest<IslemLogResponse>;
