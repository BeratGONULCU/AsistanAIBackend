using GeminiAsistanBackend.Application.DTOs.IslemLog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;

public sealed record GetIslemLogByDurumQuery(string durum) : IRequest<IEnumerable<IslemLogResponse>>;
