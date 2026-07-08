using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.EgitimDatasetQueries;

public sealed record GetAllEgitimDatasetQuery : IRequest<List<EgitimDatasetResponse>>;

