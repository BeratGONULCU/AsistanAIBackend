using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.CihazKomutuCommands;
public sealed record UpdateCihazKomutuCommand
(
    int Id,
    string type,
    string? domain,
    string? target,
    string? operation,
    string? CalisacakKod,
    string? Aciklama
) : IRequest<CihazKomutuResponse?>;

