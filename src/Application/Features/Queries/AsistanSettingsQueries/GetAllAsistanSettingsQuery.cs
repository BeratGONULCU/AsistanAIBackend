using GeminiAsistanBackend.Application.DTOs.AsistanSettings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.AsistanSettingsQueries;

public sealed class GetAllAsistanSettingsQuery : IRequest<List<AsistanSettingsResponse>>;