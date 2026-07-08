using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using MediatR;

namespace GeminiAsistanBackend.Application.Features.Commands.CihazKomutuCommands;

public sealed record CreateCihazKomutuCommand
(
    /*
     Cihaz Komutu eklemek için dışarıdan (örneğin API'den) gelecek olan verileri taşıyan, 
     yolda giderken kimsenin değiştiremeyeceği, miras alınamaz, tertemiz ve hafif bir istek (Command) nesnesiyim
     */
    string type,
    string? domain,
    string? target,
    string operation,
    string? CalisacakKod,
    string? Aciklama
) : IRequest<CihazKomutuResponse>;
