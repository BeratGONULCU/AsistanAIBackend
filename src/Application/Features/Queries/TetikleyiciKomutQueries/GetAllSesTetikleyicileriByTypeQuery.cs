using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.TetikleyiciKomutQueries;

public sealed record GetAllSesTetikleyicileriByTypeQuery(string type) : IRequest<IReadOnlyCollection<SesTetikleyiciResponse>>; 

