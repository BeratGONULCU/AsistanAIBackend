using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.CihazKomutlariQueries;

public sealed record CheckDuplicateCihazKomutlariQuery(string metin) : IRequest<bool?>;

