using GeminiAsistanBackend.Application.DTOs.RedMineDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.RedMineQueries;

public sealed record RedMineTasksQuery(string ApiKey) : IRequest<RedMineDataResponse>;
