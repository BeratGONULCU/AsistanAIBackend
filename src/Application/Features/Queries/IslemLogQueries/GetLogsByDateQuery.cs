using GeminiAsistanBackend.Application.DTOs.IslemLog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;

// burada tarihe göre log değerleri gelecek.
// bu entity için durum kısmında AI_LEARNED yerine çalıştı,hata vs yazılacak.
public sealed record GetLogsByDateQuery : IRequest<IEnumerable<IslemLogResponse>>;
