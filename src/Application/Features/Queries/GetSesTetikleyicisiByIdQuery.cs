using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Queries;

public sealed record GetSesTetikleyicisiByIdQuery(int id) : IRequest<string?>;
