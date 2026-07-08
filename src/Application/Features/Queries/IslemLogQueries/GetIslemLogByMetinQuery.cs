using GeminiAsistanBackend.Application.DTOs.IslemLog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;

public record GetIslemLogByMetinQuery(string metin) : IRequest<List<IslemLogResponse?>>;
