using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Application.DTOs.EgitimDataset;

namespace GeminiAsistanBackend.Application.Queries;

public sealed record GetAllUntrainedEgitimDataQuery(): IRequest<List<EgitimDatasetResponse>>;
