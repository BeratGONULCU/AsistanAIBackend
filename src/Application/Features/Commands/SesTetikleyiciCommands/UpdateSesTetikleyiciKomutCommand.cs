using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.SesTetikleyiciKomutCommands;

public sealed record UpdateSesTetikleyiciKomutCommand
(
    int Id,
    string TetikleyiciMetin,
    EklenmeTuru? EklenmeTuru
    ) : IRequest<SesTetikleyiciResponse?>;
