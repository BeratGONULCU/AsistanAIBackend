using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

// ilgili SessionID değeri içerisindeki tüm sohbetler silinecek.
public sealed record DeleteSessionCommand (
    int sessionID
    ) : IRequest<Boolean>;