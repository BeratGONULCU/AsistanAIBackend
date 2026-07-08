using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.SesTetikleyicileriQueries;

public sealed record CheckDuplicateSesTetikleyiciQuery(string metin) : IRequest<bool?>;
