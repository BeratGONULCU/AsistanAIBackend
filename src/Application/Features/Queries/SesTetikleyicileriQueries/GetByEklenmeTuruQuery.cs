using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.SesTetikleyicileriQueries;

public class GetByEklenmeTuruQuery : IRequest<IReadOnlyCollection<SesTetikleyiciResponse>>
{
    public EklenmeTuru eklenmeTuru { get; set; }

    public GetByEklenmeTuruQuery(string eklenmeTuruStr)
    {
        // if (Enum.TryParse(eklenmeTuruStr, true, out EklenmeTuru parsedEnum))
        if (Enum.TryParse(eklenmeTuruStr, true, out EklenmeTuru parsedEnum))
        {
            eklenmeTuru = parsedEnum;
        }
        else
        {
            throw new ArgumentException("geçersiz veri tipi");
        }
    }
}

