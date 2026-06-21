using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;

namespace GeminiAsistanBackend.Application.Commands;

public sealed record CreateSesTetikleyiciCommand
(
    string TetikleyiciMetin,
    EklenmeTuru? EklenmeTuru,
    double? aiConfidenceScore
    
): IRequest<SesTetikleyiciResponse>;
