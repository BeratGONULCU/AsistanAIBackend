using GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.RedmineEgitimdatasetCommands;

public sealed record class CreateRedmineEgitimdatasetCommand(List<CreateRedmineDatasetRequest> items) : IRequest<List<RedmineEgitimdatasetResponse>>;
