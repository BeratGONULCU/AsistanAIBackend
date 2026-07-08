using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.AsistanYanitQueries;

public sealed record GetAsistanYanitQuery : IRequest<List<AsistanSendResponse>>;