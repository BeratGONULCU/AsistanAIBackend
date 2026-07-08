using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.DTOs.TetikleyiciKomutlar;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.TetikleyiciKomutCommands;

public sealed record CreateTetikleyiciKomutCommand(  
    int tetikleyiciId,
    int komutId
    ) : IRequest<TetikleyiciKomutReponse>; 


