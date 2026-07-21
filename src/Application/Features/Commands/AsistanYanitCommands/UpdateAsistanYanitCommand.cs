using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using GeminiAsistanBackend.Domain.Enums;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

// 1. Command record'unu ID alacak þekilde güncelliyoruz
public sealed record UpdateAsistanYanitCommand(
    int id,
    AsistanYanitTuru yanit_turu
) : IRequest<AsistanSendResponse>;